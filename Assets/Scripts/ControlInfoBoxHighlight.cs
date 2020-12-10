using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlInfoBoxHighlight : MonoBehaviour
{
    public static ControlInfoBoxHighlight instance;

    public Text text1 = null;
    public Text text2 = null;
    public Text text3 = null;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        if (text1)
            StartCoroutine(FadeTextToZeroAlpha(text1));

        if (text2)
            StartCoroutine(FadeTextToZeroAlpha(text2));

        if (text3)
            StartCoroutine(FadeTextToZeroAlpha(text3));
    }

    public IEnumerator FadeTextToFullAlpha(Text text) // 알파값 0 -> 1
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FadeTextToZeroAlpha(text));
    }

    public IEnumerator FadeTextToZeroAlpha(Text text) // 알파값 1 -> 0
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FadeTextToFullAlpha(text));
    }

    public void ViewInfoWhenGameStart()
    {
        text1.color = new Color(text1.color.r, text1.color.g, text1.color.b, 1);
        text2.color = new Color(text2.color.r, text2.color.g, text2.color.b, 1);
        text3.color = new Color(text3.color.r, text3.color.g, text3.color.b, 1);

        StartCoroutine(FadeTextToZeroAlpha(text1));
        StartCoroutine(FadeTextToZeroAlpha(text2));
        StartCoroutine(FadeTextToZeroAlpha(text3));
    }
}
