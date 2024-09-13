using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class Ads : MonoBehaviour
{
    // �� ���� ���ֵ��� �׻� �׽�Ʈ ���� �����ϵ��� �����Ǿ� �ֽ��ϴ�.
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
        // Google Mobile Ads SDK �ʱ�ȭ.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {            
            // �� �ݹ��� MobileAds SDK�� �ʱ�ȭ�� �� ȣ��˴ϴ�.
        }); 

        LoadRewardedAd();
    }   

    // ������ ���� �ε��ϴ� �Լ�
    public void LoadRewardedAd()
    {
        // ������ �ε�� ���� �ִ��� Ȯ���ϰ�, �ִٸ� �����ϰ� �����մϴ�.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("������ ���� �ε��մϴ�.");

        // ���� �ε��ϴ� �� ����� ��û(request)�� �����մϴ�.
        var adRequest = new AdRequest();

        // ���� �ε� ��û�� �����ϴ�.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // error�� null�� �ƴϸ� �ε� ��û�� ������ ���Դϴ�.
                if (error != null || ad == null)
                {
                    Debug.LogError("������ ���� ���� �ε忡 �����߽��ϴ�. " +
                                   "����: " + error);
                    return;
                }

                Debug.Log("������ ���� �ε�Ǿ����ϴ�. ����: "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
    }

    // ������ ���� ȭ�鿡 ǥ���ϴ� �Լ�
    public void ShowRewardedAd(string type)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                if(type == "��")
                {
                    GameManager.instance.IncreaseGem(1500);
                    playerController.CloseDie();
                }
                else if(type == "���")
                {
                    gachaManager.PerformGachaWithEquip(20);
                }
                else if (type == "��")
                {
                    gachaManager.PerformGachaWithPet(20);
                }
                else if (type == "�ǻ�")
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

    // ���� �̺�Ʈ �ڵ鷯�� ����ϴ� �Լ�
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // ���� ������ â���� ������ ������ �� ȣ��˴ϴ�.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("������ ���� {0} {1}�� �����߽��ϴ�.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // ���� ���� ������ ��ϵǾ��� �� ȣ��˴ϴ�.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("������ ������ ������ ��ϵǾ����ϴ�.");
        };
        // ���� ���� Ŭ���� ��ϵǾ��� �� ȣ��˴ϴ�.
        ad.OnAdClicked += () =>
        {
            Debug.Log("������ ���� Ŭ���Ǿ����ϴ�.");
        };
        // ���� ��ü ȭ�� �������� ������ �� ȣ��˴ϴ�.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("������ ������ ��ü ȭ�� �������� ���Ƚ��ϴ�.");
        };
        // ���� ��ü ȭ�� �������� �ݾ��� �� ȣ��˴ϴ�.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("������ ������ ��ü ȭ�� �������� �������ϴ�.");
        };
        // ���� ��ü ȭ�� �������� ���� �� �������� �� ȣ��˴ϴ�.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("������ ���� ��ü ȭ�� �������� ���� �� �����߽��ϴ�. " +
                           "����: " + error);
        };
    }

    // ���� �����ų� �ε� ���� �� �ٽ� �ε��ϴ� �ڵ鷯�� ����ϴ� �Լ�
    private void RegisterReloadHandler(RewardedAd ad)
    {
        // ���� ��ü ȭ�� �������� �ݾ��� �� ȣ��˴ϴ�.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("������ ������ ��ü ȭ�� �������� �������ϴ�.");

            // ������ ���� �ٸ� ���� ������ �� �ֵ��� ���� �ٽ� �ε��մϴ�.
            LoadRewardedAd();
        };
        // ���� ��ü ȭ�� �������� ���� �� �������� �� ȣ��˴ϴ�.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("������ ���� ��ü ȭ�� �������� ���� �� �����߽��ϴ�. " +
                           "����: " + error);

            // ������ ���� �ٸ� ���� ������ �� �ֵ��� ���� �ٽ� �ε��մϴ�.
            LoadRewardedAd();
        };
    }
}
