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
    public GameObject prefab_armor_zombie = null;
    public GameObject zombiePrefabs;  // 하이라키에 생성되는 좀비 프리팹을 얘의 자식으로 정리.

    public GameObject armorHelmetPrefab = null;

    // 현재 씬에 존재하는 좀비들.
    private List<GameObject> existSceneZombies = new List<GameObject>();

    // 오브젝트 풀링으로 관리하는 좀비들.
    private Queue<GameObject> zombies1 = new Queue<GameObject>();
    private Queue<GameObject> zombies2 = new Queue<GameObject>();
    private Queue<GameObject> armorZombies = new Queue<GameObject>();

    private bool restOneTurnCreateZombie = false;

    public Sprite spriteZombie1;
    public Sprite spriteArmorZombie;

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

            // ArmorZombie 미리 생성.
            GameObject armorZombieObject = Instantiate(prefab_armor_zombie, Vector3.zero, Quaternion.identity);
            armorZombies.Enqueue(armorZombieObject);
            armorZombieObject.SetActive(false);
            armorZombieObject.transform.parent = zombiePrefabs.transform;
        }

        playerVector = Player.instance.transform.position;
    }

    public void GoNextTurn(GameObject collisionGO1 = null, GameObject collisionGO2 = null)
    {
        // collisionGO2 가 들어온다는건 양쪽 좀비라는 뜻.
        // 나머지는 다 collisionGO1으로 들어옴.

        bool isArmorZombie1 = false;
        bool isArmorZombie2 = false;
        int armorZombieDir1 = 0;
        int armorZombieDir2 = 0;

        if (collisionGO1)
        {
            Zombie collisionZombie = collisionGO1.GetComponent<Zombie>();
            isArmorZombie1 = collisionZombie.hp == 1 ? true : false;
            armorZombieDir1 = collisionZombie.GetMoveDir();
        }

        if (collisionGO2)
        {
            Zombie collisionZombie = collisionGO2.GetComponent<Zombie>();
            isArmorZombie2 = collisionZombie.hp == 1 ? true : false;
            armorZombieDir2 = collisionZombie.GetMoveDir();
        }

        // 양쪽이면 리턴.
        if (isArmorZombie1 && isArmorZombie2)
        {
            return;
        }

        bool createZombie = true;
        int armorZombieDir = 0;
        bool includeArmorZombie = false;
        int moveIndex = -1;
        List<Zombie> sameDirZombies = new List<Zombie>();
        if (isArmorZombie1 || isArmorZombie2)
        {
            includeArmorZombie = true;
            if (isArmorZombie1)
                armorZombieDir = armorZombieDir1;
            else
                armorZombieDir = armorZombieDir2;

            // 아머좀비랑 같은 방향 좀비들 모으기.
            for (int i = 0; i < existSceneZombies.Count; i++)
            {
                GameObject zombie_prefab = existSceneZombies[i];
                Zombie existZombie = zombie_prefab.GetComponent<Zombie>();

                if (armorZombieDir != existZombie.GetMoveDir())
                    continue;

                sameDirZombies.Add(existZombie);
            }

            // 빈공간 찾기.
            for (int i = 0; i < sameDirZombies.Count - 1; i++)
            {
                Zombie zb = sameDirZombies[i];
                Vector2 targetVec = new Vector2(zb.transform.position.x + (-1 * armorZombieDir), zb.transform.position.y);

                zb.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Collider2D collision = CheckCollision(zb.transform.position, targetVec);
                zb.gameObject.GetComponent<BoxCollider2D>().enabled = true;

                if (collision)
                {
                    createZombie = false;
                    continue;
                }

                moveIndex = i;
                createZombie = true;
                break;
            }
        }

        if (!restOneTurnCreateZombie)
        {
            // 화면 밖 대기 좀비 생성.
            int leftOrRight = Random.Range(0, 100);

            if (leftOrRight < 45 && createZombie)
            {
                CreateZombie(new Vector3(playerVector.x - 10, playerVector.y + 0.3f, playerVector.z));
            }
            else if (leftOrRight >= 45 && leftOrRight < 90 && createZombie)
            {
                CreateZombie(new Vector3(playerVector.x + 10, playerVector.y + 0.3f, playerVector.z));
            }
            else if (leftOrRight >= 90 && createZombie)
            {
                CreateZombie(new Vector3(playerVector.x - 10, playerVector.y + 0.3f, playerVector.z));
                CreateZombie(new Vector3(playerVector.x + 10, playerVector.y + 0.3f, playerVector.z));
            }

            // 모든 좀비 히어로방향으로 한 칸 이동.
            moveExistSceneZombies(collisionGO1, null, moveIndex, includeArmorZombie, armorZombieDir, sameDirZombies);
        }
        else if (restOneTurnCreateZombie)
        {
            // Revive시 생성된 좀비가 뒤로 한 칸 물러나는데 이때 좀비를 생성을 하게되면 겹치게 되는 문제 발생.
            // 따라서 restOneTurnCreateZomie 플래그를 보고 한 턴 좀비 생성을 막는다.
            restOneTurnCreateZombie = false;

            // 모든 좀비 히어로방향으로 한 칸 이동.
            moveExistSceneZombies(collisionGO1, null, moveIndex, includeArmorZombie, armorZombieDir, sameDirZombies);
        }
    }

    public void moveExistSceneZombies(GameObject collisionGO1 = null, GameObject collisionGO2 = null, int moveIndex = -1, bool isArmorZombie = false, int armorZombieDir = 0, List<Zombie> sameDirZombies = null)
    {
        if (!isArmorZombie)
        {
            // 모든 좀비 히어로방향으로 한 칸 이동.
            for (int i = 0; i < existSceneZombies.Count; i++)
            {
                GameObject zombie_prefab = existSceneZombies[i];
                Zombie existZombie = zombie_prefab.GetComponent<Zombie>();
                existZombie.Move();
            }
        }
        else
        {
            if (moveIndex != -1 && sameDirZombies.Count != 0)
            {
                // 모든 좀비 히어로방향으로 한 칸 이동.
                for (int i = 0; i < existSceneZombies.Count; i++)
                {
                    GameObject zombie_prefab = existSceneZombies[i];
                    Zombie existZombie = zombie_prefab.GetComponent<Zombie>();

                    if (sameDirZombies.Contains(existZombie))
                        continue;

                    existZombie.Move();
                }

                for (int i = moveIndex + 1; i < sameDirZombies.Count; i++)
                {
                    Zombie existZombie = sameDirZombies[i];
                    existZombie.Move();
                }
            }
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

    public void InsertQueueArmorZombie(GameObject obj)
    {
        armorZombies.Enqueue(obj);
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
        zb.hp = 1;

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
        zb.hp = 1;

        return obj;
    }

    public GameObject GetQueueArmorZombie(Vector3 vector)
    {
        GameObject obj = armorZombies.Dequeue();
        obj.transform.position = vector;
        obj.SetActive(true);
        obj.gameObject.GetComponent<BoxCollider2D>().enabled = true;

        Zombie zb = obj.GetComponent<Zombie>();
        zb.CalcMoveDir();
        zb.hp = 2;

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
        else if (zombiePrefab.GetComponent<Zombie>().name == "ArmorZombie")
        {
            zombiePrefab.GetComponent<SpriteRenderer>().sprite = spriteArmorZombie;
            InsertQueueArmorZombie(zombiePrefab);
        }
    }

    public void CreateZombie(Vector3 vector)
    {
        GameObject zombieClone = CreateRandomZombie(vector);

        // 좀비가 왼쪽에 있는데 플립이 안되어있는 경우 플립.
        // 좀비가 오른쪽에 있는데 플립이 되어있는 경우 플립.
        Zombie zb = zombieClone.GetComponent<Zombie>();

        // Flip 해야되면 한다.
        CalcFlip(vector, zb);

        existSceneZombies.Add(zombieClone);
    }

    public void CalcFlip(Vector3 vector, Zombie zb)
    {
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
    }

    public GameObject CreateRandomZombie(Vector3 vector)
    {
        int ran = Random.Range(0, 100);
        GameObject zombieClone = null;
        if (ran <= 49)
        {
            zombieClone = GetQueueZombie1(vector);
        }
        else if (ran > 49 && ran <= 94)
        {
            zombieClone = GetQueueZombie2(vector);
        }
        else if (ran > 94 && ran < 100 && GameManager.instance.GetScore() >= 150)
        {
            zombieClone = GetQueueArmorZombie(vector);
        }
        else
        {
            int ran1 = Random.Range(0, 2);
            if (ran1 == 1)
                zombieClone = GetQueueZombie1(vector);
            else
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
            else if (zombie.GetComponent<Zombie>().name == "ArmorZombie")
            {
                InsertQueueArmorZombie(zombie);
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

    public Collider2D CheckCollision(Vector2 start, Vector2 end)
    {
        RaycastHit2D hit;

        hit = Physics2D.Linecast(start, end, Player.instance.zombieLayerMask);

        if (hit.transform != null)
            return hit.collider;

        return null;
    }
}
