using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public Image outRectRight;
    public Image outRectLeft;

    public Button attackBtn;
    public Button leftRightAttackBtn;
    public Button turnBtn;

    public GameObject tutoControlCanvas;
    public GameObject tutoSuccessCanvas;
    
    private TutorialEnemyManager tutoEnemyManager;
    private TutorialDBManager tutoDBManager;

    private int tutoStep = 0;

    private bool viewTutorialFlag = true;

    public bool nextTuto = false;

    private void Start()
    {
        tutoEnemyManager = FindObjectOfType<TutorialEnemyManager>();
        tutoDBManager = FindObjectOfType<TutorialDBManager>();

        //StartCoroutine(FadeTextToZeroAlpha(touchText));
    }

    // Update is called once per frame
    void Update()
    {
        if (tutoStep > 0 && tutoStep < 14)
        {
            if ((tutoStep == 1 || tutoStep == 2 || tutoStep == 3) && nextTuto)
            {
                nextTuto = false;

                attackBtn.image.color = new Color(attackBtn.image.color.r, attackBtn.image.color.g, attackBtn.image.color.b, 1);
                StopAllCoroutines();
                StartCoroutine(FadeBtnToZeroAlpha(attackBtn));

                outRectRight.gameObject.SetActive(true);

                leftRightAttackBtn.enabled = false;
                turnBtn.enabled = false;
            }
            else if (tutoStep == 4 && nextTuto)
            {
                nextTuto = false;

                attackBtn.image.color = new Color(attackBtn.image.color.r, attackBtn.image.color.g, attackBtn.image.color.b, 1);
                turnBtn.image.color = new Color(turnBtn.image.color.r, turnBtn.image.color.g, turnBtn.image.color.b, 1);

                StopAllCoroutines();
                StartCoroutine(FadeBtnToZeroAlpha(turnBtn));

                outRectRight.gameObject.SetActive(false);
                outRectLeft.gameObject.SetActive(true);

                turnBtn.enabled = true;
                leftRightAttackBtn.enabled = false;
                attackBtn.enabled = false;
            }
            else if ((tutoStep == 5 || tutoStep == 6) && nextTuto)
            {
                nextTuto = false;

                attackBtn.image.color = new Color(attackBtn.image.color.r, attackBtn.image.color.g, attackBtn.image.color.b, 1);
                turnBtn.image.color = new Color(turnBtn.image.color.r, turnBtn.image.color.g, turnBtn.image.color.b, 1);

                StopAllCoroutines();
                StartCoroutine(FadeBtnToZeroAlpha(attackBtn));

                turnBtn.enabled = false;
                leftRightAttackBtn.enabled = false;
                attackBtn.enabled = true;
            }
            else if (tutoStep == 7 && nextTuto)
            {
                nextTuto = false;

                attackBtn.image.color = new Color(attackBtn.image.color.r, attackBtn.image.color.g, attackBtn.image.color.b, 1);
                turnBtn.image.color = new Color(turnBtn.image.color.r, turnBtn.image.color.g, turnBtn.image.color.b, 1);

                StopAllCoroutines();
                StartCoroutine(FadeBtnToZeroAlpha(turnBtn));

                outRectRight.gameObject.SetActive(true);
                outRectLeft.gameObject.SetActive(false);

                turnBtn.enabled = true;
                leftRightAttackBtn.enabled = false;
                attackBtn.enabled = false;
            }
            else if ((tutoStep == 8 || tutoStep == 9) && nextTuto)
            {
                nextTuto = false;

                attackBtn.image.color = new Color(attackBtn.image.color.r, attackBtn.image.color.g, attackBtn.image.color.b, 1);
                turnBtn.image.color = new Color(turnBtn.image.color.r, turnBtn.image.color.g, turnBtn.image.color.b, 1);

                StopAllCoroutines();
                StartCoroutine(FadeBtnToZeroAlpha(attackBtn));

                turnBtn.enabled = false;
                leftRightAttackBtn.enabled = false;
                attackBtn.enabled = true;
            }
            else if ((tutoStep > 9 && tutoStep < 13) && nextTuto)
            {
                nextTuto = false;

                attackBtn.image.color = new Color(attackBtn.image.color.r, attackBtn.image.color.g, attackBtn.image.color.b, 1);
                turnBtn.image.color = new Color(turnBtn.image.color.r, turnBtn.image.color.g, turnBtn.image.color.b, 1);
                leftRightAttackBtn.image.color = new Color(leftRightAttackBtn.image.color.r, leftRightAttackBtn.image.color.g, leftRightAttackBtn.image.color.b, 1);

                StopAllCoroutines();
                StartCoroutine(FadeBtnToZeroAlpha(leftRightAttackBtn));

                outRectRight.gameObject.SetActive(true);
                outRectLeft.gameObject.SetActive(true);

                turnBtn.enabled = false;
                leftRightAttackBtn.enabled = true;
                attackBtn.enabled = false;
            }
            else if (tutoStep == 13 && nextTuto)
            {
                nextTuto = false;

                attackBtn.image.color = new Color(attackBtn.image.color.r, attackBtn.image.color.g, attackBtn.image.color.b, 1);
                turnBtn.image.color = new Color(turnBtn.image.color.r, turnBtn.image.color.g, turnBtn.image.color.b, 1);
                leftRightAttackBtn.image.color = new Color(leftRightAttackBtn.image.color.r, leftRightAttackBtn.image.color.g, leftRightAttackBtn.image.color.b, 1);

                StopAllCoroutines();

                outRectRight.gameObject.SetActive(false);
                outRectLeft.gameObject.SetActive(false);

                tutoControlCanvas.SetActive(false);

                tutoSuccessCanvas.SetActive(true);

                leftRightAttackBtn.enabled = false;
                turnBtn.enabled = false;
                attackBtn.enabled = true;
            }
        }
    }

    public void SetTutoStep(int step)
    {
        tutoStep = step;
    }

    public int GetTutoStep()
    {
        return tutoStep;
    }

    public void AgainTutorial()
    {
        StopAllCoroutines();

        tutoControlCanvas.SetActive(true);
        tutoSuccessCanvas.SetActive(false);
        tutoStep = 0;
        nextTuto = true;

        tutoEnemyManager.CreateTutorialZombies();

        //touchText.gameObject.SetActive(true);

        //touchText.color = new Color(touchText.color.r, touchText.color.g, touchText.color.b, 1);
        //StartCoroutine(FadeTextToZeroAlpha(touchText));

    }

    public void GoRealGameWorld()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public IEnumerator FadeBtnToFullAlpha(Button btn) // 알파값 0 -> 1
    {
        btn.image.color = new Color(btn.image.color.r, btn.image.color.g, btn.image.color.b, 0.7f);

        while (btn.image.color.a < 1f)
        {
            btn.image.color = new Color(btn.image.color.r, btn.image.color.g, btn.image.color.b, btn.image.color.a + Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FadeBtnToZeroAlpha(btn));
    }

    public IEnumerator FadeBtnToZeroAlpha(Button btn) // 알파값 1 -> 0
    {
        btn.image.color = new Color(btn.image.color.r, btn.image.color.g, btn.image.color.b, 1);
        while (btn.image.color.a > 0.5f)
        {
            btn.image.color = new Color(btn.image.color.r, btn.image.color.g, btn.image.color.b, btn.image.color.a - Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FadeBtnToFullAlpha(btn));
    }

    public void AlwaysTutoSkip()
    {
        viewTutorialFlag = false;
        tutoDBManager.CallSave();
        GoRealGameWorld();
    }

    public bool GetViewTutorialFlag()
    {
        return viewTutorialFlag;
    }

    public void SetViewTutorialFlag(bool view)
    {
        viewTutorialFlag = view;
    }
}
