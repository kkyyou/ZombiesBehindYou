using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public static CloudManager instance;

    public float speed = 0.01f;
    public bool moveRight = true;

    public GameObject greenCloud;
    public GameObject darkCloud;
    public GameObject pinkCloud;
    public GameObject deepGreenCloud;
    public GameObject iceMountainCloud;
    public GameObject pinkIceMountainCloud;

    private GameObject currentCloud;

    private float fiveSecCount = 0; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fiveSecCount <= 5)
        {
            fiveSecCount += 1 * Time.deltaTime;
        }

        if (currentCloud)
        {
            // 5초 주기로 구름 좌우방향 변경.
            if (fiveSecCount >= 5)
            {
                fiveSecCount = 0;
                moveRight = !moveRight;
            }

            Vector3 cloudPosition = currentCloud.transform.position;                

            if (moveRight)
                cloudPosition.x += (speed * Time.deltaTime);
            else
                cloudPosition.x -= (speed * Time.deltaTime);

            currentCloud.transform.position = cloudPosition;
        }
    }

    // 현재 맵의 구름을 지정해줘야 움직임.
    public void SetCurrentCloud(GameManager.Map map)
    {
        if (map == GameManager.Map.GREEN)
        {
            currentCloud = greenCloud;
        }
        else if (map == GameManager.Map.DARK)
        {
            currentCloud = darkCloud;
        }
        else if (map == GameManager.Map.PINK)
        {
            currentCloud = pinkCloud;
        }
        else if (map == GameManager.Map.DEEP_GREEN)
        {
            currentCloud = deepGreenCloud;
        }
        else if (map == GameManager.Map.ICE_MOUNTAIN)
        {
            currentCloud = iceMountainCloud;
        }
        else if (map == GameManager.Map.PINK_ICE_MOUNTAIN)
        {
            currentCloud = pinkIceMountainCloud;
        }

        Debug.Log("Loc" + "X : " + GameManager.instance.gameOverZone.transform.position.x + "Y : " + GameManager.instance.gameOverZone.transform.position.y + "Z : " + GameManager.instance.gameOverZone.transform.position.z);
    }
}
