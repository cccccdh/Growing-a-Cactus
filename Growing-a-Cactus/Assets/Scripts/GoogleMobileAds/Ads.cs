using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class Ads : MonoBehaviour
{
    // 이 광고 유닛들은 항상 테스트 광고를 제공하도록 설정되어 있습니다.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _adUnitId = "unused";
#endif

    public GachaManager gachaManager;
    public PlayerController playerController;

    private RewardedAd _rewardedAd;

    public void Start()
    {
        // Google Mobile Ads SDK 초기화.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {            
            // 이 콜백은 MobileAds SDK가 초기화된 후 호출됩니다.
        }); 

        LoadRewardedAd();
    }   

    // 리워드 광고를 로드하는 함수
    public void LoadRewardedAd()
    {
        // 이전에 로드된 광고가 있는지 확인하고, 있다면 제거하고 해제합니다.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("리워드 광고를 로드합니다.");

        // 광고를 로드하는 데 사용할 요청(request)을 생성합니다.
        var adRequest = new AdRequest();

        // 광고 로드 요청을 보냅니다.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // error가 null이 아니면 로드 요청이 실패한 것입니다.
                if (error != null || ad == null)
                {
                    Debug.LogError("리워드 광고가 광고 로드에 실패했습니다. " +
                                   "오류: " + error);
                    return;
                }

                Debug.Log("리워드 광고가 로드되었습니다. 응답: "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
    }

    // 리워드 광고를 화면에 표시하는 함수
    public void ShowRewardedAd(string type)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                if(type == "젬")
                {
                    GameManager.instance.IncreaseGem(1500);
                    playerController.CloseDie();
                }
                else if(type == "장비")
                {
                    gachaManager.PerformGachaWithEquip(20);
                }
                else if (type == "펫")
                {
                    gachaManager.PerformGachaWithPet(20);
                }
                else if (type == "의상")
                {
                    gachaManager.PerformGachaWithClothes(20);
                }
            });
        }
        else
        {
            LoadRewardedAd();
        }
    }

    // 광고 이벤트 핸들러를 등록하는 함수
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // 광고가 수익을 창출한 것으로 추정될 때 호출됩니다.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("리워드 광고가 {0} {1}를 지불했습니다.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // 광고에 대한 노출이 기록되었을 때 호출됩니다.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("리워드 광고의 노출이 기록되었습니다.");
        };
        // 광고에 대한 클릭이 기록되었을 때 호출됩니다.
        ad.OnAdClicked += () =>
        {
            Debug.Log("리워드 광고가 클릭되었습니다.");
        };
        // 광고가 전체 화면 콘텐츠로 열렸을 때 호출됩니다.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("리워드 광고의 전체 화면 콘텐츠가 열렸습니다.");
        };
        // 광고가 전체 화면 콘텐츠를 닫았을 때 호출됩니다.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("리워드 광고의 전체 화면 콘텐츠가 닫혔습니다.");
        };
        // 광고가 전체 화면 콘텐츠를 여는 데 실패했을 때 호출됩니다.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("리워드 광고가 전체 화면 콘텐츠를 여는 데 실패했습니다. " +
                           "오류: " + error);
        };
    }

    // 광고가 닫히거나 로드 실패 시 다시 로드하는 핸들러를 등록하는 함수
    private void RegisterReloadHandler(RewardedAd ad)
    {
        // 광고가 전체 화면 콘텐츠를 닫았을 때 호출됩니다.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("리워드 광고의 전체 화면 콘텐츠가 닫혔습니다.");

            // 가능한 빨리 다른 광고를 보여줄 수 있도록 광고를 다시 로드합니다.
            LoadRewardedAd();
        };
        // 광고가 전체 화면 콘텐츠를 여는 데 실패했을 때 호출됩니다.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("리워드 광고가 전체 화면 콘텐츠를 여는 데 실패했습니다. " +
                           "오류: " + error);

            // 가능한 빨리 다른 광고를 보여줄 수 있도록 광고를 다시 로드합니다.
            LoadRewardedAd();
        };
    }
}
