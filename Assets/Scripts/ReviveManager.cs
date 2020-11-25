using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveManager : MonoBehaviour
{
    public static ReviveManager instance;

    public Slider reviveTimeSlider;

    private bool anyBtnClicked = false;
    private bool showRevive = false;
    private bool showRewardAds = false;

    public Button noBtn;
    public Button showRewardAdsBtn;


    private void OnEnable()
    {
        reviveTimeSlider.value = reviveTimeSlider.maxValue;
        showRevive = true;
    }

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

    // Update is called once per frame
    void Update()
    {
        if (showRevive)
        {
            reviveTimeSlider.value -= 1 * Time.deltaTime; // 1초에 1씩 줄어듬. 

            if (reviveTimeSlider.value <= 0)
            {
                showRevive = false;
                anyBtnClicked = true;
                showRewardAds = false;
            }
        }
    }

    public void ClickNoBtn()
    {
        anyBtnClicked = true;
        showRewardAds = false;
        showRevive = false;
    }

    public void ShowRewardAds()
    {
        anyBtnClicked = true;
        showRewardAds = true;
        showRevive = false;
        AdmobManager.instance.ShowRewardAd();
    }

    public bool GetAnyBtnClicked()
    {
        return anyBtnClicked;
    }

    public void SetAnyBtnClicked(bool _anyBtnClicked)
    {
        anyBtnClicked = _anyBtnClicked;
    }

    public bool GetShowRewardAds()
    {
        return showRewardAds;
    }

    public void SetShowRewardAds(bool _showRewardAds)
    {
        showRewardAds = _showRewardAds;
    }
}
