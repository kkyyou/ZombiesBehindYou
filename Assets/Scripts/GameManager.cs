﻿using System.Collections;
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
    private int bestScore;
    private int totalScore;
    public Text scoreText;
    private bool listenSfx = true;
    private bool listenBgm = true;

    public Slider hpSlider;
    public float hpDecreaseSpeed;

    private bool attackZombie = false;

    public RightTargetZone rightTargetZone;
    public LeftTargetZone leftTargetZone;
    public GameOverZone gameOverZone;

    private Vector3 rightTargetZoneInitPos;
    private Vector3 leftTargetZoneInitPos;
    private Vector3 gameOverZoneInitPos;

    public GameObject scoreCanvas;
    public GameObject controlCanvas;
    public GameObject TitleCanvas;
    public GameObject shopCanvas;
    public GameObject pauseCanvas;
    public GameObject settingCanvas;

    private Map currentMap;

    //private bool firstGame = true;
    private bool playing = false;

    public Text gameOverScoreText;
    public Text gameOverBestScoreText;
    public Text newRecordText;
    public Text shopBestScoreText;
    public Text shopTotalScoreText;

    public Button shopSelectButton;
    public Button turnBtn;
    public Button attackLRBtn;
    public Button attackBtn;


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
        TitleCanvas.SetActive(true);
        controlCanvas.SetActive(false);
        scoreCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        settingCanvas.SetActive(false);

        // 게임 시작 시 정보 로드.
        DBManager.instance.CallLoad();

        // 게임 시작 시 캐릭터 이전에 선택했었던 캐릭터로 변경.
        CharSelectManager.instance.ChangeCharacter(Player.instance.GetSelectedCharacterNumber());

        if (GetListenBgm())
            AudioManager.instance.Play("background");

        rightTargetZoneInitPos = rightTargetZone.transform.position;
        leftTargetZoneInitPos = leftTargetZone.transform.position;
        gameOverZoneInitPos = gameOverZone.transform.position;

        // 랜덤 맵 지정.
        RandomMap();

        // 처음 시작 시 좀비 생성.
        //EnemyManager.instance.CreateStartZombies();
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
            RecoveryHP(5);
        }

        if (shopCanvas.activeSelf && Player.instance.GetAnimator().GetBool("Die"))
        {
            Player.instance.GetAnimator().SetBool("Die", false);

            if (!Player.instance.GetIsRight())
            {
                Player.instance.Flip();
                Player.instance.SetIsRight(true);
            }

            // 좀비 리셋.
            EnemyManager.instance.ResetEnemy();
        }

        //if (playing)
            //hpSlider.value -= Time.deltaTime * hpDecreaseSpeed;
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
            hpDecreaseSpeed += 0.08f; 
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
        // 맵에 따른 플레이어, 박스컬라이더 위치 이동.

        int ran = Random.Range(0, 3);
        if (ran == (int)Map.GREEN)
        {
            // 카메라 흔들기 위치 초기화.
            mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(new Vector3(0f, 0f, -10f));

            mainCamera.transform.position = new Vector3(0f, 0f, -10f);
            Player.instance.transform.position = new Vector3(0f, -0.425f, 0f);

            rightTargetZone.transform.position = rightTargetZoneInitPos;
            leftTargetZone.transform.position = leftTargetZoneInitPos;
            gameOverZone.transform.position = gameOverZoneInitPos;
        }
        else if (ran == (int)Map.DARK)
        {
            // 카메라 흔들기 위치 초기화.
            mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(new Vector3(0f, -14f, -10f));
            
            mainCamera.transform.position = new Vector3(0f, -14f, -10f);
            Player.instance.transform.position = new Vector3(0f, -14.425f, 0f);

            Vector3 rtp = rightTargetZoneInitPos;
            rtp.y -= 14f;
            rightTargetZone.transform.position = rtp;

            Vector3 ltp = leftTargetZoneInitPos;
            ltp.y -= 14f;
            leftTargetZone.transform.position = ltp;

            Vector3 gop = gameOverZoneInitPos;
            gop.y -= 14f;
            gameOverZone.transform.position = gop;
        }
        else if (ran == (int)Map.PINK)
        {
            // 카메라 흔들기 위치 초기화.
            mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(new Vector3(0f, -28f, -10f));

            mainCamera.transform.position = new Vector3(0f, -28f, -10f);
            Player.instance.transform.position = new Vector3(0f, -28.425f, 0f);

            Vector3 rtp = rightTargetZoneInitPos;
            rtp.y -= 28f;
            rightTargetZone.transform.position = rtp;

            Vector3 ltp = leftTargetZoneInitPos;
            ltp.y -= 28f;
            leftTargetZone.transform.position = ltp;

            Vector3 gop = gameOverZoneInitPos;
            gop.y -= 28f;
            gameOverZone.transform.position = gop;
        }

        currentMap = (Map)ran;

        CloudManager.instance.SetCurrentCloud(currentMap);
    }

    public void GameStart()
    {
        if (!AudioManager.instance.IsPlaying("background") && listenBgm)
            AudioManager.instance.Play("background");

        // 초기화
        score = 0;
        scoreText.text = "0";
        hpDecreaseSpeed = 4;
        hpSlider.value = 15f;

        Player.instance.GetAnimator().SetBool("Die", false);

        scoreCanvas.SetActive(true);
        controlCanvas.SetActive(true);
        TitleCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        gameOverZone.SetGameOverCheck(true);
        settingCanvas.SetActive(false);

        // 랜덤 맵 지정.
        //if (!firstGame)
        //{
        // 좀비 리셋.
        EnemyManager.instance.ResetEnemy();

        // 랜덤 맵.
        RandomMap();

        // 좀비 생성.
        EnemyManager.instance.CreateStartZombies();
        //}

        //firstGame = false;

        // 카메라 위치 변경.
        mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(mainCamera.transform.position);

    }

    public void GameOver()
    {
        // 게임오버 사운드 출력.
        if (listenBgm)
            AudioManager.instance.Stop("background");
        
        if (listenSfx)
            AudioManager.instance.Play("gameover");

        playing = false;

        scoreCanvas.SetActive(false);
        controlCanvas.SetActive(false);
        TitleCanvas.SetActive(true);
        newRecordText.gameObject.SetActive(false);

        // 최대 스코어 저장.
        if (bestScore < score)
        {
            bestScore = score;
            newRecordText.gameObject.SetActive(true);
            StartCoroutine(NewRecordHighlightCoroutine());
        }

        // 총합 스코어 업데이트.
        totalScore += score;

        // Die Animation.
        Player.instance.GetAnimator().SetBool("Die", true);

        TitleCanvas.transform.Find("Panel").transform.Find("Title Panel").gameObject.SetActive(false);
        TitleCanvas.transform.Find("Panel").transform.Find("GameOver Panel").gameObject.SetActive(true);

        // 게임 오버 시 현재 스코어 보여주기.
        gameOverScoreText.text = score.ToString();

        gameOverBestScoreText.text = bestScore.ToString();

        // 게임 오버 시 정보 저장.
        DBManager.instance.CallSave();
    }

    IEnumerator NewRecordHighlightCoroutine()
    {
        Color origColor = newRecordText.color;

        while (newRecordText.gameObject.activeSelf)
        {
            newRecordText.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            newRecordText.color = origColor;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetPlaying(bool isPlay)
    {
        playing = isPlay;
    }

    public bool GetPlaying()
    {
        return playing;
    }

    public void ShowSelectCharcter()
    {
        scoreCanvas.SetActive(false);
        controlCanvas.SetActive(false);
        TitleCanvas.SetActive(false);
        shopCanvas.SetActive(true);
        newRecordText.gameObject.SetActive(false);

        shopBestScoreText.text = bestScore.ToString();
        shopTotalScoreText.text = totalScore.ToString();

        if (!Player.instance.GetIsRight())
        {
            Player.instance.Flip();
            Player.instance.SetIsRight(true);
        } 

        CharSelectManager.instance.CharacterInfo(Player.instance.GetSelectedCharacterNumber());
    }

    public void ShowSettingView()
    {
        scoreCanvas.SetActive(false);
        controlCanvas.SetActive(false);
        TitleCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        settingCanvas.SetActive(true);
    }

    public void ShowTitleView()
    {
        scoreCanvas.SetActive(false);
        controlCanvas.SetActive(false);
        TitleCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        settingCanvas.SetActive(false);

        TitleCanvas.transform.Find("Panel").transform.Find("Title Panel").gameObject.SetActive(true);
        TitleCanvas.transform.Find("Panel").transform.Find("GameOver Panel").gameObject.SetActive(false);

        GameManager.instance.SetControlButtonEnabled(true);

        // 좀비 생성.
        //if (!firstGame)
        //EnemyManager.instance.CreateStartZombies();

        EnemyManager.instance.ResetEnemy();

        //firstGame = true;
    }

    public int GetBestScore()
    {
        return bestScore;
    }

    public void SetBestScore(int bs)
    {
        bestScore = bs;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void SetTotalScore(int ts)
    {
        totalScore = ts;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int s)
    {
        score = s;
    }

    public void SetControlButtonEnabled(bool enabled)
    {
        turnBtn.enabled = enabled;
        attackBtn.enabled = enabled;
        attackLRBtn.enabled = enabled;
    }

    public Map GetCurrentMap()
    {
        return currentMap;
    }

    public void SetListenSfx(bool _listen)
    {
        listenSfx = _listen;
    }

    public bool GetListenSfx()
    {
        return listenSfx;
    }

    public void SetListenBgm(bool _listen)
    {
        listenBgm = _listen;
    }

    public bool GetListenBgm()
    {
        return listenBgm;
    }
}
