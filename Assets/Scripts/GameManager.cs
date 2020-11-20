using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Camera mainCamera;
    
    private int score;
    public Text scoreText;

    public Slider hpSlider;
    public float hpDecreaseSpeed;

    private bool attackZombie = false;

    public RightTargetZone rightTargetZone;
    public LeftTargetZone leftTargetZone;

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
        int ran = Random.Range(0, 3);
        if (ran == 1)
        {
            mainCamera.transform.position = new Vector3(0.5f, 0f, -10f);
            Player.instance.transform.position = new Vector3(0.5f, -0.425f, 0f);
        }
        else
        {
            mainCamera.transform.position = new Vector3(0.5f, -14f, -10f);
            Player.instance.transform.position = new Vector3(0.5f, -14.425f, 0f);

            Vector3 rtp = rightTargetZone.transform.position;
            rtp.y -= 14f;
            rightTargetZone.transform.position = rtp;

            Vector3 ltp = leftTargetZone.transform.position;
            ltp.y -= 14f;
            leftTargetZone.transform.position = ltp;
        }
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

    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();

        if (hpDecreaseSpeed < 30)
            hpDecreaseSpeed += 0.05f; 
    }

    public void RecoveryHP(int recoverHP)
    {
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
}
