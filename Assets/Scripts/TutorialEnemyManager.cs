using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEnemyManager : MonoBehaviour
{
    public GameObject tutoPlayer;
    public GameObject prefab_zombie = null;
    private TutorialManager tutorialManger;
    private TutorialDBManager tutoDBManager;

    private List<GameObject> tutoZombie = new List<GameObject>();

    public GameObject loadSceneCancas;

    private void Awake()
    {
        loadSceneCancas.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        tutorialManger = FindObjectOfType<TutorialManager>();
        tutoDBManager = FindObjectOfType<TutorialDBManager>();
       
        tutoDBManager.CallLoad();

        // 튜토리얼 항상 안보기 클릭 시.
        if (!tutorialManger.GetViewTutorialFlag())
        {
            tutorialManger.GoRealGameWorld();
            return;
        }

        loadSceneCancas.SetActive(false);

        CreateTutorialZombies();
    }

    public void GoNextTurn()
    {
        for (int i = 0; i < tutoZombie.Count; i++)
        {
            if (tutoZombie[i])
            {
                TutoZombie tzb = tutoZombie[i].GetComponent<TutoZombie>();
                tzb.Move();
            }
        }
    }

    public void CreateTutorialZombies()
    {
        Vector3 tutoPlayerVector = tutoPlayer.transform.position;
        
        // Right 3
        GameObject zombieObject1 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x + 1, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        GameObject zombieObject2 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x + 2, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        GameObject zombieObject3 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x + 3, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);

        // Left 3
        GameObject zombieObject4 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x - 4, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        zombieObject4.GetComponent<TutoZombie>().Flip();
        GameObject zombieObject5 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x - 5, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        zombieObject5.GetComponent<TutoZombie>().Flip();
        GameObject zombieObject6 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x - 6, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        zombieObject6.GetComponent<TutoZombie>().Flip();

        // Right 3
        GameObject zombieObject7 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x + 7, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        GameObject zombieObject8 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x + 8, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        GameObject zombieObject9 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x + 9, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);

        // Left Right 3
        GameObject zombieObject10 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x + 10, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        GameObject zombieObject11 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x - 10, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        zombieObject11.GetComponent<TutoZombie>().Flip();

        GameObject zombieObject12 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x + 11, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        GameObject zombieObject13 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x - 11, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        zombieObject13.GetComponent<TutoZombie>().Flip();

        GameObject zombieObject14 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x + 12, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        GameObject zombieObject15 = Instantiate(prefab_zombie, new Vector3(tutoPlayerVector.x - 12, tutoPlayerVector.y, tutoPlayerVector.z), Quaternion.identity);
        zombieObject15.GetComponent<TutoZombie>().Flip();

        tutoZombie.Add(zombieObject1);
        tutoZombie.Add(zombieObject2);
        tutoZombie.Add(zombieObject3);
        tutoZombie.Add(zombieObject4);
        tutoZombie.Add(zombieObject5);
        tutoZombie.Add(zombieObject6);
        tutoZombie.Add(zombieObject7);
        tutoZombie.Add(zombieObject8);
        tutoZombie.Add(zombieObject9);
        tutoZombie.Add(zombieObject10);
        tutoZombie.Add(zombieObject11);
        tutoZombie.Add(zombieObject12);
        tutoZombie.Add(zombieObject13);
        tutoZombie.Add(zombieObject14);
        tutoZombie.Add(zombieObject15);


        tutorialManger.SetTutoStep(1);
    }
}
