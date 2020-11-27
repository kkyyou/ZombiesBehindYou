using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance;

    public float time = 0;

    float min = 0;
    float hour = 0;

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

    private void Start()
    {
        StartCoroutine(StopWatch());
    }

    IEnumerator StopWatch()
    {
        while (true)
        {
            time += Time.deltaTime;

            if (hour < (int)(time / 3600))
            {
                hour = (int)(time / 3600);
                GameManager.instance.SetTotalHour(GameManager.instance.GetTotalHour() + 1);

                if (Player.instance.GetCharacterNumber() == (int)CharSelectManager.Characters.Archer)
                {
                    CharSelectManager.instance.RequireInfoText.text = GameManager.instance.GetTotalPlayTimeText() + "/01:00";
                }
                else if (Player.instance.GetCharacterNumber() == (int)CharSelectManager.Characters.AxeHeroGirl)
                {
                    CharSelectManager.instance.RequireInfoText.text = GameManager.instance.GetTotalPlayTimeText() + "/02:00";
                }

                DBManager.instance.data.totalPlayHour += 1;
                DBManager.instance.SaveCurrentData();
            }

            if (min < (int)(time / 60 % 60))
            {
                min = (int)(time / 60 % 60);
                GameManager.instance.SetTotalMin(GameManager.instance.GetTotalMin() + 1);

                if (Player.instance.GetCharacterNumber() == (int)CharSelectManager.Characters.Archer)
                {
                    CharSelectManager.instance.RequireInfoText.text = GameManager.instance.GetTotalPlayTimeText() + "/01:00";
                }
                else if (Player.instance.GetCharacterNumber() == (int)CharSelectManager.Characters.AxeHeroGirl)
                {
                    CharSelectManager.instance.RequireInfoText.text = GameManager.instance.GetTotalPlayTimeText() + "/02:00";
                }

                DBManager.instance.data.totalPlayMin += 1;
                DBManager.instance.SaveCurrentData();
            }

            yield return null;
        }
    }
}
