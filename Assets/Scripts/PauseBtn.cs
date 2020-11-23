using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBtn : MonoBehaviour
{
    public Button sfxBtn;
    public Button bgmBtn;
    public Button homeBtn;
    public Button rePlayBtn;

    public Sprite sfxBtnNoSprite;
    public Sprite bgmBtnNoSprite;

    public Sprite sfxBtnSprite;
    public Sprite bgmBtnSprite;


    private void Start()
    {
        if (GameManager.instance.GetListenSfx())
            sfxBtn.image.sprite = sfxBtnSprite;
        else
            sfxBtn.image.sprite = sfxBtnNoSprite;

        if (GameManager.instance.GetListenBgm())
            bgmBtn.image.sprite = bgmBtnSprite;
        else
            bgmBtn.image.sprite = bgmBtnNoSprite;
    }

    public void ClickPauseBtn()
    {
        GameManager.instance.SetPlaying(false);
        GameManager.instance.SetControlButtonEnabled(false);

        GameManager.instance.pauseCanvas.SetActive(true);

        if (GameManager.instance.GetListenSfx())
            sfxBtn.image.sprite = sfxBtnSprite;
        else
            sfxBtn.image.sprite = sfxBtnNoSprite;

        if (GameManager.instance.GetListenBgm())
            bgmBtn.image.sprite = bgmBtnSprite;
        else
            bgmBtn.image.sprite = bgmBtnNoSprite;
    }

    public void ClickReplay()
    {
        if (GameManager.instance.GetPlaying())
            return;

        GameManager.instance.SetPlaying(true);
        GameManager.instance.SetControlButtonEnabled(true);

        GameManager.instance.pauseCanvas.SetActive(false);
    }

    public void ClickSfx()
    {
        string str = "";
        if (GameManager.instance.GetListenSfx())
        {
            GameManager.instance.SetListenSfx(false);
            sfxBtn.image.sprite = sfxBtnNoSprite;

            DBManager.instance.data.listenSfx = false;
            str = "Sfx False";
        }
        else
        {
            GameManager.instance.SetListenSfx(true);
            sfxBtn.image.sprite = sfxBtnSprite;
            DBManager.instance.data.listenSfx = true;
            str = "Sfx True";
        }

        Debug.Log("Save : " + str);
        DBManager.instance.SaveCurrentData();
    }

    public void ClickBgm()
    {
        string str = "";
        if (GameManager.instance.GetListenBgm())
        {
            GameManager.instance.SetListenBgm(false);
            bgmBtn.image.sprite = bgmBtnNoSprite;

            DBManager.instance.data.listenBgm = false;

            AudioManager.instance.Stop("background");

            str = "Bgm False";
        }
        else
        {
            GameManager.instance.SetListenBgm(true);
            bgmBtn.image.sprite = bgmBtnSprite;

            DBManager.instance.data.listenBgm = true;

            AudioManager.instance.Play("background");

            str = "Bgm True";
        }

        Debug.Log("Save : " + str);
        DBManager.instance.SaveCurrentData();
    }

    public void ClickHome()
    {
        GameManager.instance.ShowTitleView();
    }

    public void ClickBack()
    {
        GameManager.instance.ShowTitleView();
    }
}
