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
        PINK,
        DEEP_GREEN,
        ICE_MOUNTAIN,
        PINK_ICE_MOUNTAIN
    };

    public static GameManager instance;

    public Camera mainCamera;
    
    private int score;
    private int bestScore;
    private int totalScore;
    public Text scoreText;
    private int totalHour;
    private int totalMin;
    private int totalGameOverCount;

    private bool listenSfx = true;
    private bool listenBgm = true;

    public Slider hpSlider;
    public float hpDecreaseSpeed;

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
    public GameObject reviveCanvas;

    private Map currentMap;

    private bool firstGame = true;
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

    private int gameCount = 0;

    private bool canRevive = true;

    private bool gameOverCheck = true;

    private bool showFrontAds = true;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            hpSlider.value -= Time.deltaTime * hpDecreaseSpeed;

            if (hpSlider.value <= 0 && gameOverCheck)
            {
                GameOver();
                gameOverCheck = false;
            }
        }
    }

    IEnumerator ResetHpFillSprite()
    {
        GameObject hpFillArea = hpSlider.transform.Find("Fill Area").gameObject;
        GameObject hpFill = hpFillArea.transform.Find("Fill").gameObject;

        Sprite origFillSprite = hpFill.GetComponent<Image>().sprite;
        hpFill.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/hp_white_bg.png");

        yield return new WaitForSeconds(0.01f);

        hpFill.GetComponent<Image>().sprite = origFillSprite;
    }

    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();

        if (hpDecreaseSpeed < 20)
            hpDecreaseSpeed += 0.03f; 
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

    public void attackZombieSuccess()
    {
        // 점수 획득.
        AddScore(1);

        // Hp 회복.
        RecoveryHP(5);
    }

    public void RandomMap()
    {
        // 맵에 따른 플레이어, 박스컬라이더 위치 이동.
        int ran = Random.Range(0, 6);
        if (ran == (int)Map.GREEN)
        {
            // 카메라 흔들기 위치 초기화.
            mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(new Vector3(0f, 0f, -10f));

            mainCamera.transform.position = new Vector3(0f, 0f, -10f);
            Player.instance.transform.position = new Vector3(0f, -0.44f, 0f);

            rightTargetZone.transform.position = rightTargetZoneInitPos;
            leftTargetZone.transform.position = leftTargetZoneInitPos;
            gameOverZone.transform.position = gameOverZoneInitPos;
        }
        else if (ran == (int)Map.DARK)
        {
            // 카메라 흔들기 위치 초기화.
            mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(new Vector3(0f, -14f, -10f));
            
            mainCamera.transform.position = new Vector3(0f, -14f, -10f);
            Player.instance.transform.position = new Vector3(0f, -14.44f, 0f);

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
            Player.instance.transform.position = new Vector3(0f, -28.44f, 0f);

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
        else if (ran == (int)Map.DEEP_GREEN)
        {
            // 카메라 흔들기 위치 초기화.
            mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(new Vector3(23f, 0f, -10f));

            mainCamera.transform.position = new Vector3(23f, 0f, -10f);
            Player.instance.transform.position = new Vector3(23f, -0.44f, 0f);

            Vector3 rtp = rightTargetZoneInitPos;
            rtp.x += 23f;
            rightTargetZone.transform.position = rtp;

            Vector3 ltp = leftTargetZoneInitPos;
            ltp.x += 23f;
            leftTargetZone.transform.position = ltp;

            Vector3 gop = gameOverZoneInitPos;
            gop.x += 23f;
            gameOverZone.transform.position = gop;
        }
        else if (ran == (int)Map.ICE_MOUNTAIN)
        {
            // 카메라 흔들기 위치 초기화.
            mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(new Vector3(23f, -14f, -10f));

            mainCamera.transform.position = new Vector3(23f, -14f, -10f);
            Player.instance.transform.position = new Vector3(23f, -14.44f, 0f);

            Vector3 rtp = rightTargetZoneInitPos;
            rtp.x += 23f;
            rtp.y -= 14f;
            rightTargetZone.transform.position = rtp;

            Vector3 ltp = leftTargetZoneInitPos;
            ltp.x += 23f;
            ltp.y -= 14f;
            leftTargetZone.transform.position = ltp;

            Vector3 gop = gameOverZoneInitPos;
            gop.x += 23f;
            gop.y -= 14f;
            gameOverZone.transform.position = gop;
        }
        else if (ran == (int)Map.PINK_ICE_MOUNTAIN)
        {
            // 카메라 흔들기 위치 초기화.
            mainCamera.GetComponent<ShakeCamera>().SetInitialPosition(new Vector3(23f, -28f, -10f));

            mainCamera.transform.position = new Vector3(23f, -28f, -10f);
            Player.instance.transform.position = new Vector3(23f, -28.44f, 0f);

            Vector3 rtp = rightTargetZoneInitPos;
            rtp.x += 23f;
            rtp.y -= 28f;
            rightTargetZone.transform.position = rtp;

            Vector3 ltp = leftTargetZoneInitPos;
            ltp.x += 23f;
            ltp.y -= 28f;
            leftTargetZone.transform.position = ltp;

            Vector3 gop = gameOverZoneInitPos;
            gop.x += 23f;
            gop.y -= 28f;
            gameOverZone.transform.position = gop;            
        }

        currentMap = (Map)ran;

        CloudManager.instance.SetCurrentCloud(currentMap);
    }

    public void GameStart()
    {
        controlCanvas.transform.Find("Info").gameObject.SetActive(true);

        canRevive = true;

        if (!AudioManager.instance.IsPlaying("background") && listenBgm)
            AudioManager.instance.Play("background");

        if (showFrontAds)
        {
            if (gameCount < 5)
                gameCount++;
            else if (gameCount >= 5)
            {
                gameCount = 1;
                AdmobManager.instance.ShowFrontAd();
            }
        }

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
        settingCanvas.SetActive(false);
        gameOverCheck = true;

        // 좀비 리셋.
        EnemyManager.instance.ResetEnemy();

        // 랜덤 맵.
        if (!firstGame)
        {
            RandomMap();
            ControlInfoBoxHighlight.instance.ViewInfoWhenGameStart();
        }
        // 좀비 생성.
        EnemyManager.instance.CreateStartZombies();

        firstGame = false;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        scoreCanvas.SetActive(false);
        controlCanvas.SetActive(false);

        // 게임오버 사운드 출력.
        if (listenBgm)
            AudioManager.instance.Stop("background");

        if (listenSfx)
            AudioManager.instance.Play("gameover");

        // Die Animation.
        Player.instance.GetAnimator().SetBool("Die", true);

        playing = false;

        // 내 점수에서 50점을 더하면 최대점수가 될때 Revive 찬스!
        if (score + 100 >= bestScore && canRevive && score >= 20)           
        {
            // 리바이브 한번 떳으니까 다음 게임 시작까지는 안뜨도록.
            canRevive = false;

            // No 나 광고보기 버튼 둘 중 아무거나 눌릴때까지 or 시간초 다 될때까지 기다리기.
            reviveCanvas.SetActive(true);
            yield return new WaitUntil(() => ReviveManager.instance.GetAnyBtnClicked());

            // 다시 No 나 광고보기 어떤 버튼이든 눌렸냐 false로 초기화.
            ReviveManager.instance.SetAnyBtnClicked(false);

            // 광고 보기 클릭 시.
            if (ReviveManager.instance.GetShowRewardAds())
            {
                // 광고 다 보거나 중간에 닫을때까지 기다린다.
                yield return new WaitUntil(() => AdmobManager.instance.GetSuccessOrClosedAds());

                // 광고 다 보거나 중간에 닫을때까지 체크 플래그 false로 초기화.
                AdmobManager.instance.SetSuccessOrClosedAds(false);

                // 광고를 다 본 경우.
                if (AdmobManager.instance.GetSuccessRewardAds())
                {
                    // 광고 다 봤는지 플래그 false 초기화.
                    AdmobManager.instance.SetSuccessRewardAds(false);

                    // 플레이 재개.
                    //playing = true;
                    reviveCanvas.SetActive(false);
                    scoreCanvas.SetActive(true);
                    controlCanvas.SetActive(true);

                    // 광고 다 봤는지 체크 플래그 false 초기화.
                    AdmobManager.instance.SetSuccessRewardAds(false);

                    // 다시 살리기.
                    Player.instance.GetAnimator().SetBool("Die", false);

                    // 좀비들 플레이어 반대방향으로 한 칸씩 이동 시키기.
                    EnemyManager.instance.MoveReverseExistSceneZombies();

                    // Revive시 생성된 좀비가 뒤로 한 칸 물러나는데 이때 좀비를 생성을 하게되면 겹치게 되는 문제 발생.
                    // 따라서 restOneTurnCreateZomie 플래그를 보고 한 턴 좀비 생성을 막게하기 위함.
                    EnemyManager.instance.SetRestOneTurnCreateZombie(true);

                    if (listenBgm)
                        AudioManager.instance.Play("background");

                    // 0.5초 대기 후 게임 오버 체크 시작.
                    yield return new WaitForSeconds(0.5f);

                    gameOverCheck = true;

                    yield break;
                }
            }

            reviveCanvas.SetActive(false);
        }

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

        TitleCanvas.transform.Find("Panel").transform.Find("Title Panel").gameObject.SetActive(false);
        TitleCanvas.transform.Find("Panel").transform.Find("GameOver Panel").gameObject.SetActive(true);

        // 게임 오버 시 현재 스코어 보여주기.
        gameOverScoreText.text = score.ToString();

        gameOverBestScoreText.text = bestScore.ToString();

        // 게임 오버 시 정보 저장.
        totalGameOverCount++;
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

        if (Player.instance.GetAnimator().GetBool("Die"))
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
        else
        {
            if (!Player.instance.GetIsRight())
            {
                Player.instance.Flip();
                Player.instance.SetIsRight(true);
            } 
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

        SetControlButtonEnabled(true);

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

    public string GetTotalPlayTimeText()
    {
        return string.Format("{0:00}:{1:00}", totalHour, totalMin);
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

    public bool GetCanRevive()
    {
        return canRevive;
    }

    public void SetCanRevive(bool _canRevive)
    {
        canRevive = _canRevive;
    }

    public void SetTotalHour(int hour)
    {
        totalHour = hour;
    }

    public int GetTotalHour()
    {
        return totalHour;
    }

    public void SetTotalMin(int min)
    {
        totalMin = min;
    }

    public int GetTotalMin()
    {
        return totalMin;
    }

    public int GetTotalGameOverCount()
    {
        return totalGameOverCount;
    }

    public void SetTotalGameOverCount(int gameOverCount)
    {
        totalGameOverCount = gameOverCount;
    }

    public void SetGameOverCheck(bool check)
    {
        gameOverCheck = check;
    }

    public bool GetGameOverCheck()
    {
        return gameOverCheck;
    }
}
