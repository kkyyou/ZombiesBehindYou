using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private int score;
    public Text scoreText;

    public Slider hpSlider;
    public float hpDecreaseSpeed;

    private bool attackZombie = false;

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
