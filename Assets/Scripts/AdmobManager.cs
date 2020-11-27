using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;

    // 테스트 할 때, 테스트 모드가 아닌 ID로 테스트하면 계정 정지 먹을 수 있음.
    private bool isTestMode = true;
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
        LoadBannerAd();
        LoadFrontAd();
        LoadRewardAd();
    }

    void Update()
    {
        //FrontAdsBtn.interactable = frontAd.IsLoaded();
        //RewardAdsBtn.interactable = rewardAd.IsLoaded();
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().AddTestDevice("847703F67789680E").Build();
    }



    #region 배너 광고
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "";
    BannerView bannerAd;


    void LoadBannerAd()
    {
        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,
            AdSize.SmartBanner, AdPosition.Top);
        bannerAd.LoadAd(GetAdRequest());
        //ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
    #endregion



    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "";
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
    const string rewardID = "";
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
}
