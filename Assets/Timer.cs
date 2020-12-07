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

            if (min < (int)(time / 60 % 60))
            {
                min = (int)(time / 60 % 60);

                GameManager.instance.SetTotalMin(GameManager.instance.GetTotalMin() + 1);
                DBManager.instance.data.totalPlayMin += 1;

                if (GameManager.instance.GetTotalMin() >= 60)
                {
                    DBManager.instance.data.totalPlayHour += 1;
                    DBManager.instance.data.totalPlayMin = 0;

                    GameManager.instance.SetTotalHour(GameManager.instance.GetTotalHour() + 1);
                    GameManager.instance.SetTotalMin(0);
                }

                if (Player.instance.GetCharacterNumber() == (int)CharSelectManager.Characters.Archer)
                {
                    CharSelectManager.instance.RequireInfoText.text = GameManager.instance.GetTotalPlayTimeText() + "/01:00";

                    if (GameManager.instance.GetTotalHour() >= 1)
                    {
                        CharSelectManager.instance.RequireInfoText.color = Color.green;
                        CharSelectManager.instance.SelectButtonEnableTrue();
                    }
                }
                else if (Player.instance.GetCharacterNumber() == (int)CharSelectManager.Characters.AxeHeroGirl)
                {
                    CharSelectManager.instance.RequireInfoText.text = GameManager.instance.GetTotalPlayTimeText() + "/02:00";

                    if (GameManager.instance.GetTotalHour() >= 2)
                    {
                        CharSelectManager.instance.RequireInfoText.color = Color.green;
                        CharSelectManager.instance.SelectButtonEnableTrue();
                    }
                }
                else if (Player.instance.GetCharacterNumber() == (int)CharSelectManager.Characters.CuteCat)
                {
                    CharSelectManager.instance.RequireInfoText.text = GameManager.instance.GetTotalPlayTimeText() + "/03:00";

                    if (GameManager.instance.GetTotalHour() >= 3)
                    {
                        CharSelectManager.instance.RequireInfoText.color = Color.green;
                        CharSelectManager.instance.SelectButtonEnableTrue();
                    }
                }

                DBManager.instance.SaveCurrentData();
            }

            yield return null;
        }
    }
}
