using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private Vector3 playerVector;

    public GameObject prefab_zombie;

    private List<GameObject> zombies;

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
        zombies = new List<GameObject>();
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

            GameObject zombieClone = Instantiate(prefab_zombie, vector, Quaternion.Euler(Vector3.zero));
            
            // 좌측 좀비면 Flip.
            if (leftOrRight == 0)
                zombieClone.GetComponent<Zombie>().Flip();

            zombies.Add(zombieClone);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.GetNextTurn())
        {
            Player.instance.SetNextTurn(false);

            // 화면 밖 대기 좀비 생성.
            int leftOrRight = Random.Range(0, 2);

            Vector3 vector = playerVector;
            if (leftOrRight == 0)
            {
                vector.x = playerVector.x + -10;
            }
            else
            {
                vector.x = playerVector.x + 10;
            }

            GameObject zombieClone = Instantiate(prefab_zombie, vector, Quaternion.Euler(Vector3.zero));

            Zombie zb = zombieClone.GetComponent<Zombie>();

            if (vector.x - playerVector.x < 0)
                zb.Flip();

            zombies.Add(zombieClone);

            // 모든 좀비 히어로에게 한 칸 이동.
            for (int i = 0; i < zombies.Count; i++)
            {
                GameObject zombie_prefab = zombies[i];
                Zombie zombie = zombie_prefab.GetComponent<Zombie>();
                zombie.Move();
            }
        }
    }

    public void RemoveZombie(GameObject zombiePrefab)
    {
        zombies.Remove(zombiePrefab);
    }
}
