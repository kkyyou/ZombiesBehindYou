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

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
