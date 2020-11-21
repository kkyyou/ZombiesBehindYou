using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Map
    {
        GREEN,
        DARK,
        PINK
    };

    public static GameManager instance;

    public Camera mainCamera;
    
    private int score;
    public Text scoreText;

    public Slider hpSlider;
    public float hpDecreaseSpeed;

    private bool attackZombie = false;

    public RightTargetZone rightTargetZone;
    public LeftTargetZone leftTargetZone;
    public GameOverZone gameOverZone;

    public GameObject scoreCanvas;
    public GameObject controlCanvas;
    public GameObject TitleCanvas;

    private Map currentMap; 

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
        // 랜덤 맵 지정.
        RandomMap();

        // 카메라 위치 변경.
        mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(mainCamera.transform.position);

        // 처음 시작 시 좀비 생성.
        EnemyManager.instance.CreateStartZombies();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackZombie)
        {
            attackZombie = false;

            // 점수 획득.
            AddScore(1);

            // Hp 회복.
            RecoveryHP(10);
        }

        hpSlider.value -= Time.deltaTime * hpDecreaseSpeed;
    }

    IEnumerator ResetHpFillSprite()
    {
        GameObject hpFillArea = hpSlider.transform.Find("Fill Area").gameObject;
        GameObject hpFill = hpFillArea.transform.Find("Fill").gameObject;

        Sprite origFillSprite = hpFill.GetComponent<Image>().sprite;
        hpFill.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/hp_white_bg.png");

        yield return new WaitForSeconds(0.05f);

        hpFill.GetComponent<Image>().sprite = origFillSprite;
    }

    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();

        if (hpDecreaseSpeed < 30)
            hpDecreaseSpeed += 0.05f; 
    }

    public void RecoveryHP(int recoverHP)
    {
        StopAllCoroutines();

        // Hp 반짝임 효과.
        StartCoroutine(ResetHpFillSprite());

        hpSlider.value += recoverHP;
    }

    public void FasterDecreaseHP(float faster)
    {
        hpDecreaseSpeed += faster;
    }

    public void SetAttackZombie(bool attack)
    {
        attackZombie = attack;
    }

    public void RandomMap()
    {
        int ran = Random.Range(0, 3);
        if (ran == (int)Map.GREEN)
        {
            mainCamera.transform.position = new Vector3(0.5f, 0f, -10f);
            Player.instance.transform.position = new Vector3(0.5f, -0.425f, 0f);
        }
        else if (ran == (int)Map.DARK)
        {
            mainCamera.transform.position = new Vector3(0.5f, -14f, -10f);
            Player.instance.transform.position = new Vector3(0.5f, -14.425f, 0f);

            Vector3 rtp = rightTargetZone.transform.position;
            rtp.y -= 14f;
            rightTargetZone.transform.position = rtp;

            Vector3 ltp = leftTargetZone.transform.position;
            ltp.y -= 14f;
            leftTargetZone.transform.position = ltp;

            Vector3 gop = gameOverZone.transform.position;
            gop.y -= 14f;
            gameOverZone.transform.position = gop;
        }
        else if (ran == (int)Map.PINK)
        {
            mainCamera.transform.position = new Vector3(0.5f, -28f, -10f);
            Player.instance.transform.position = new Vector3(0.5f, -28.425f, 0f);

            Vector3 rtp = rightTargetZone.transform.position;
            rtp.y -= 28f;
            rightTargetZone.transform.position = rtp;

            Vector3 ltp = leftTargetZone.transform.position;
            ltp.y -= 28f;
            leftTargetZone.transform.position = ltp;

            Vector3 gop = gameOverZone.transform.position;
            gop.y -= 28f;
            gameOverZone.transform.position = gop;
        }

        currentMap = (Map)ran;

        CloudManager.instance.SetCurrentCloud(currentMap);
    }

    public void GameStart()
    {
        scoreCanvas.SetActive(true);
        controlCanvas.SetActive(true);
        TitleCanvas.SetActive(false);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        scoreCanvas.SetActive(false);
        controlCanvas.SetActive(false);
        TitleCanvas.SetActive(true);
    }
}
