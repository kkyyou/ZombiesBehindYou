using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;

    // 테스트 할 때, 테스트 모드가 아닌 ID로 테스트하면 계정 정지 먹을 수 있음.
    private bool isTestMode = true; // 무료 / 유료 출시 APK 만들고나서 true로 바꿨음 2020-12-21 09:41
    //public Text LogText;
    //public Button FrontAdsBtn, RewardAdsBtn;

    private bool successRewardAds = false;
    private bool adsSuccessOrClosed = false;

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

    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        if (GameManager.instance.GetShowFrontAds())
        {
            GameManager.instance.loadingCanvas.SetActive(true);
            LoadBannerAd();
            LoadFrontAd();
            StartCoroutine(LoadingCanvas());
        }

        LoadRewardAd();
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().AddTestDevice("847703F67789680E").AddTestDevice("BCEA9DBFD7EDE507").
            AddTestDevice("F3C046386A840A11").Build();
    }

    #region 배너 광고
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "ca-app-pub-1174509462280243/6730663372";
    BannerView bannerAd;

    void LoadBannerAd()
    {
        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,
            AdSize.Banner, AdPosition.Top);
        bannerAd.LoadAd(GetAdRequest());

        bannerAd.OnAdOpening += (sender, e) =>
        {
            GameManager.instance.loadingCanvas.SetActive(false);
        };

         bannerAd.OnAdLoaded += (sender, e) =>
        {
            GameManager.instance.loadingCanvas.SetActive(false);
        };

        bannerAd.OnAdFailedToLoad += (sender, e) =>
        {
            GameManager.instance.loadingCanvas.SetActive(false);
        };
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
    #endregion



    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "ca-app-pub-1174509462280243/5417581707";
    InterstitialAd frontAd;


    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
        frontAd.OnAdClosed += (sender, e) =>
        {
        };
    }

    public void ShowFrontAd()
    {
        frontAd.Show();
        LoadFrontAd();
    }
    #endregion



    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "ca-app-pub-1174509462280243/7976489950";
    RewardedAd rewardAd;


    void LoadRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            // 사용자가 보상형 광고 다 봄.
            adsSuccessOrClosed = true;
            successRewardAds = true;
        };

        rewardAd.OnAdClosed += (sender, e) =>
        {
            // 사용자가 보상형 광고를 중간에 끔.
            adsSuccessOrClosed = true;

            // 여기서 successRewardAds false 초기화하면 안됨.
            // 광고 다 보고나서도 Close하면 호출되는 함수라서.
        };

        rewardAd.OnAdFailedToShow += (sender, e) =>
        {
            adsSuccessOrClosed = true;
        };

        rewardAd.OnAdFailedToLoad += (sender, e) =>
        {
            adsSuccessOrClosed = true;
        };

        rewardAd.OnAdLoaded += (sender, e) =>
        {
            adsSuccessOrClosed = false;
        };
    }

    public void ShowRewardAd()
    {
        rewardAd.Show();
        LoadRewardAd();
    }
    #endregion

    public bool GetSuccessRewardAds()
    {
        return successRewardAds;
    }

    public void SetSuccessRewardAds(bool _successRewardAds)
    {
        successRewardAds = _successRewardAds;
    }

    public bool GetSuccessOrClosedAds()
    {
        return adsSuccessOrClosed;
    }

    public void SetSuccessOrClosedAds(bool _adsSuccessOrClosed)
    {
        adsSuccessOrClosed = _adsSuccessOrClosed;
    }

    IEnumerator LoadingCanvas()
    {
        yield return new WaitForSeconds(10f);

        GameManager.instance.loadingCanvas.SetActive(false);
    }
}
