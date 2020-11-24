﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private Vector3 playerVector;

    public GameObject prefab_zombie;
    public GameObject prefab_zombie2;
    public GameObject zombiePrefabs;  // 하이라키에 생성되는 좀비 프리팹을 얘의 자식으로 정리.

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.GetNextTurn())
        {
            Player.instance.SetNextTurn(false);

            // 화면 밖 대기 좀비 생성.
            int leftOrRight = Random.Range(0, 100);

            if (leftOrRight < 45)
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

    public void CreateZombie(Vector3 vector)
    {
        GameObject zombieClone = CreateRandomZombie(vector);

        Zombie zb = zombieClone.GetComponent<Zombie>();
        if (vector.x - playerVector.x < 0)
            zb.Flip();

        zombies.Add(zombieClone);
    }

    public GameObject CreateRandomZombie(Vector3 vector)
    {
        int ran = Random.Range(0, 2);
        GameObject zombieClone = null;
        if (ran == 0)
        {
            zombieClone = Instantiate(prefab_zombie, vector, Quaternion.Euler(Vector3.zero));
        }
        else if (ran == 1)
        {
            zombieClone = Instantiate(prefab_zombie2, vector, Quaternion.Euler(Vector3.zero));
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

            // 좌측 좀비면 Flip.
            if (leftOrRight == 0)
                zombieClone.GetComponent<Zombie>().Flip();

            zombies.Add(zombieClone);
        }
    }

    public void ResetEnemy()
    {
        for (int i = 0; i < zombies.Count; i++)
        {
            GameObject zombie = zombies[i];
            Destroy(zombie);
        }

        zombies.Clear();
    }
}
