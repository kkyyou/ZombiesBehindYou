using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public Image outRectRight;
    public Image outRectLeft;
    public Text touchText;

    public Button attackBtn;
    public Button leftRightAttackBtn;
    public Button turnBtn;

    public GameObject tutoControlCanvas;
    public GameObject tutoSuccessCanvas;
    
    private TutorialEnemyManager tutoEnemyManager;
    private TutorialDBManager tutoDBManager;

    private int tutoStep = 0;

    private bool viewTutorialFlag = true;

    private void Start()
    {
        tutoEnemyManager = FindObjectOfType<TutorialEnemyManager>();
        tutoDBManager = FindObjectOfType<TutorialDBManager>();

        StartCoroutine(FadeTextToZeroAlpha(touchText));
    }

    // Update is called once per frame
    void Update()
    {
        if (tutoStep > 0 && tutoStep < 14)
        {
            if (tutoStep == 1 || tutoStep == 2 || tutoStep == 3)
            {
                touchText.gameObject.transform.position = new Vector3(4.3f, touchText.transform.position.y, touchText.transform.position.z); ;
                touchText.gameObject.SetActive(true);
                outRectRight.gameObject.SetActive(true);

                leftRightAttackBtn.enabled = false;
                turnBtn.enabled = false;
            }
            else if (tutoStep == 4)
            {
                touchText.gameObject.transform.position = new Vector3(-3.8f, touchText.transform.position.y, touchText.transform.position.z);
                outRectRight.gameObject.SetActive(false);
                outRectLeft.gameObject.SetActive(true);

                turnBtn.enabled = true;
                leftRightAttackBtn.enabled = false;
                attackBtn.enabled = false;
            }
            else if (tutoStep == 5 || tutoStep == 6)
            {
                touchText.gameObject.transform.position = new Vector3(4.3f, touchText.transform.position.y, touchText.transform.position.z); ;

                turnBtn.enabled = false;
                leftRightAttackBtn.enabled = false;
                attackBtn.enabled = true;
            }
            else if (tutoStep == 7)
            {
                touchText.gameObject.transform.position = new Vector3(-3.8f, touchText.transform.position.y, touchText.transform.position.z);
                outRectRight.gameObject.SetActive(true);
                outRectLeft.gameObject.SetActive(false);

                turnBtn.enabled = true;
                leftRightAttackBtn.enabled = false;
                attackBtn.enabled = false;
            }
            else if (tutoStep == 8 || tutoStep == 9)
            {
                touchText.gameObject.transform.position = new Vector3(4.3f, touchText.transform.position.y, touchText.transform.position.z);

                turnBtn.enabled = false;
                leftRightAttackBtn.enabled = false;
                attackBtn.enabled = true;
            }
            else if (tutoStep > 9 && tutoStep < 13)
            {
                outRectRight.gameObject.SetActive(true);
                outRectLeft.gameObject.SetActive(true);
                touchText.gameObject.transform.position = new Vector3(0.3f, touchText.transform.position.y, touchText.transform.position.z);

                turnBtn.enabled = false;
                leftRightAttackBtn.enabled = true;
                attackBtn.enabled = false;
            }
            else if (tutoStep == 13)
            {
                outRectRight.gameObject.SetActive(false);
                outRectLeft.gameObject.SetActive(false);
                touchText.gameObject.SetActive(false);

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

        tutoEnemyManager.CreateTutorialZombies();

        touchText.gameObject.SetActive(true);

        touchText.color = new Color(touchText.color.r, touchText.color.g, touchText.color.b, 1);
        StartCoroutine(FadeTextToZeroAlpha(touchText));

    }

    public void GoRealGameWorld()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public IEnumerator FadeTextToFullAlpha(Text text) // 알파값 0 -> 1
    {
        touchText.color = new Color(touchText.color.r, touchText.color.g, touchText.color.b, 0);

        while (touchText.color.a < 1.0f)
        {
            touchText.color = new Color(touchText.color.r, touchText.color.g, touchText.color.b, touchText.color.a + Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FadeTextToZeroAlpha(text));
    }

    public IEnumerator FadeTextToZeroAlpha(Text text) // 알파값 1 -> 0
    {
        touchText.color = new Color(touchText.color.r, touchText.color.g, touchText.color.b, 1);
        while (touchText.color.a > 0.0f)
        {
            touchText.color = new Color(touchText.color.r, touchText.color.g, touchText.color.b, touchText.color.a - Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FadeTextToFullAlpha(touchText));
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
