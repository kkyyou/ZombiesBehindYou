using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀링 적용.
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private Vector3 playerVector;

    public GameObject prefab_zombie = null;
    public GameObject prefab_zombie2 = null;
    public GameObject zombiePrefabs;  // 하이라키에 생성되는 좀비 프리팹을 얘의 자식으로 정리.

    // 현재 씬에 존재하는 좀비들.
    private List<GameObject> existSceneZombies = new List<GameObject>();

    // 오브젝트 풀링으로 관리하는 좀비들.
    private Queue<GameObject> zombies1 = new Queue<GameObject>();
    private Queue<GameObject> zombies2 = new Queue<GameObject>();

    private bool restOneTurnCreateZombie = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 21; i++)
        {
            // Zombie 1 미리 생성.
            GameObject zombieObject = Instantiate(prefab_zombie, Vector3.zero, Quaternion.identity);
            zombies1.Enqueue(zombieObject);
            zombieObject.SetActive(false);
            zombieObject.transform.parent = zombiePrefabs.transform;

            // Zombie 2 미리 생성.
            GameObject zombieObject2 = Instantiate(prefab_zombie2, Vector3.zero, Quaternion.identity);
            zombies2.Enqueue(zombieObject2);
            zombieObject2.SetActive(false);
            zombieObject2.transform.parent = zombiePrefabs.transform;
        }

        playerVector = Player.instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.GetNextTurn() && !restOneTurnCreateZombie)
        {
            Player.instance.SetNextTurn(false);

            // 화면 밖 대기 좀비 생성.
            int leftOrRight = Random.Range(0, 100);

            if (leftOrRight < 101)
            {
                CreateZombie(new Vector3(playerVector.x - 10, playerVector.y + 0.3f, playerVector.z));
            }
            else if (leftOrRight >= 45 && leftOrRight < 90)
            {
                CreateZombie(new Vector3(playerVector.x + 10, playerVector.y + 0.3f, playerVector.z));
            }
            else if (leftOrRight >= 90)
            {
                CreateZombie(new Vector3(playerVector.x - 10, playerVector.y + 0.3f, playerVector.z));
                CreateZombie(new Vector3(playerVector.x + 10, playerVector.y + 0.3f, playerVector.z));
            }

            // 모든 좀비 히어로방향으로 한 칸 이동.
            moveExistSceneZombies();
        }
        else if (Player.instance.GetNextTurn() && restOneTurnCreateZombie)  
        {
            // Revive시 생성된 좀비가 뒤로 한 칸 물러나는데 이때 좀비를 생성을 하게되면 겹치게 되는 문제 발생.
            // 따라서 restOneTurnCreateZomie 플래그를 보고 한 턴 좀비 생성을 막는다.
            Player.instance.SetNextTurn(false);
            restOneTurnCreateZombie = false;

            // 모든 좀비 히어로방향으로 한 칸 이동.
            moveExistSceneZombies();
        }
    }

    public void moveExistSceneZombies()
    {
        // 모든 좀비 히어로방향으로 한 칸 이동.
        for (int i = 0; i < existSceneZombies.Count; i++)
        {
            GameObject zombie_prefab = existSceneZombies[i];
            Zombie zombie = zombie_prefab.GetComponent<Zombie>();
            zombie.Move();
        }
    }

    // 사용한 객체 큐에 반남.
    public void InsertQueueZombie1(GameObject obj)
    {
        zombies1.Enqueue(obj);
        obj.SetActive(false);
        obj.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void InsertQueueZombie2(GameObject obj)
    {
        zombies2.Enqueue(obj);
        obj.SetActive(false);
        obj.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public GameObject GetQueueZombie1(Vector3 vector)
    {
        GameObject obj = zombies1.Dequeue();
        obj.transform.position = vector;
        obj.SetActive(true);
        obj.gameObject.GetComponent<BoxCollider2D>().enabled = true;

        Zombie zb = obj.GetComponent<Zombie>();
        zb.CalcMoveDir();

        return obj;
    }

    public GameObject GetQueueZombie2(Vector3 vector)
    {
        GameObject obj = zombies2.Dequeue();
        obj.transform.position = vector;
        obj.SetActive(true);
        obj.gameObject.GetComponent<BoxCollider2D>().enabled = true;

        Zombie zb = obj.GetComponent<Zombie>();
        zb.CalcMoveDir();

        return obj;
    }

    public void RemoveZombie(GameObject zombiePrefab)
    {
        existSceneZombies.Remove(zombiePrefab);

        // 0.5초뒤 InsertQueue.
        StartCoroutine(DeleteZombieCoroutine(zombiePrefab));
    }

    IEnumerator DeleteZombieCoroutine(GameObject zombiePrefab)
    {
        yield return new WaitForSeconds(0.5f);

        if (zombiePrefab.GetComponent<Zombie>().name == "Zombie1")
        {
            InsertQueueZombie1(zombiePrefab);
        }
        else if (zombiePrefab.GetComponent<Zombie>().name == "Zombie2")
        {
            InsertQueueZombie2(zombiePrefab);
        }
    }

    public void CreateZombie(Vector3 vector)
    {
        GameObject zombieClone = CreateRandomZombie(vector);

        // 좀비가 왼쪽에 있는데 플립이 안되어있는 경우 플립.
        // 좀비가 오른쪽에 있는데 플립이 되어있는 경우 플립.
        Zombie zb = zombieClone.GetComponent<Zombie>();

        // 좀비의 위치가 플레이어의 왼쪽에 있나.
        bool zombieLeftLoc = true;
        if (vector.x - playerVector.x < 0)
            zombieLeftLoc = true;
        else if (vector.x - playerVector.x > 0)
            zombieLeftLoc = false;

        if ((zombieLeftLoc && zb.transform.localScale.x > 0)
            || (!zombieLeftLoc && zb.transform.localScale.x < 0))
        {
            zb.Flip();
        }

        existSceneZombies.Add(zombieClone);
    }

    public GameObject CreateRandomZombie(Vector3 vector)
    {
        int ran = Random.Range(0, 2);
        GameObject zombieClone = null;
        if (ran == 0)
        {
            zombieClone = GetQueueZombie1(vector);
        }
        else if (ran == 1)
        {
            zombieClone = GetQueueZombie2(vector);
        }

        zombieClone.transform.parent = zombiePrefabs.transform;
        return zombieClone;
    }
    public void CreateStartZombies()
    {
        playerVector = Player.instance.transform.position;

        // 시작시 생성되는 좀비.
        for (int i = 1; i < 10; i++)
        {
            int leftOrRight = Random.Range(0, 2);

            Vector3 vector = playerVector;
            if (leftOrRight == 0)
            {
                vector.x = playerVector.x + (-1 * i);
            }
            else
            {
                vector.x = playerVector.x + i;
            }

            vector.y = playerVector.y - 0.015f;

            GameObject zombieClone = CreateRandomZombie(vector);
            zombieClone.transform.parent = zombiePrefabs.transform;

            Zombie zb = zombieClone.GetComponent<Zombie>();
            bool zombieLeftLoc = true;
            if (leftOrRight == 1)
                zombieLeftLoc = false;

            if ((zombieLeftLoc && zb.transform.localScale.x > 0)
                 || (!zombieLeftLoc && zb.transform.localScale.x < 0))
            {
                zb.Flip();
            }

            existSceneZombies.Add(zombieClone);
        }
    }

    public void ResetEnemy()
    {
        for (int i = 0; i < existSceneZombies.Count; i++)
        {
            GameObject zombie = existSceneZombies[i];

            if (zombie.GetComponent<Zombie>().name == "Zombie1")
            {
                InsertQueueZombie1(zombie);
            }
            else if (zombie.GetComponent<Zombie>().name == "Zombie2")
            {
                InsertQueueZombie2(zombie);
            }
        }

        existSceneZombies.Clear();
    }

    public void MoveReverseExistSceneZombies()
    {
        // 모든 좀비 히어로방향으로 한 칸 이동.
        for (int i = existSceneZombies.Count - 1; i >= 0; i--)
        {
            GameObject zombie_prefab = existSceneZombies[i];
            Zombie zombie = zombie_prefab.GetComponent<Zombie>();
            zombie.MoveReverse();

        }
    }

    public bool GetRestOneTurnCreateZombie()
    {
        return restOneTurnCreateZombie;
    }

    public void SetRestOneTurnCreateZombie(bool _restOneTurnCreateZombie)
    {
        restOneTurnCreateZombie = _restOneTurnCreateZombie;
    }
}
