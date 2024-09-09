using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI StageText;
    public TextMeshProUGUI GemText;
    public GameObject ShopPage;
    public GameObject GachaPage;
    public GameObject CharacterPage;
    public GameObject WeaponPage;
    public GameObject ArmorPage;
    public GameObject PetPage;
    public GameObject OptionPage;
    public Image waveBar; // 웨이브 바 이미지 추가

    [Header("하단 버튼")]
    public GameObject characterName;
    public GameObject characterX;
    public GameObject petName;
    public GameObject petX;
    public GameObject shopName;
    public GameObject shopX;

    [Header("재화")]
    public double gold = 0;
    public int gem = 0;

    [Header("라운드 & 웨이브")]
    public int stageNumber = 1;
    public int roundNumber = 1;
    private int wave = 0; // 현재 웨이브를 추적하는 변수 추가
    private const int maxWave = 100; // 최대 웨이브 값

    [Header("팝업 창 bool 값")]
    public bool isOpenShop = false;
    public bool isOpenGacha = false;
    public bool isOpenCharacter = false;
    public bool isOpenWeapon = false;
    public bool isOpenArmor = false;
    public bool isOpenPet = false;
    public bool isOpenOption = false;

    void Start()
    {
        UpdateGoldText();
        UpdateGemText();
        UpdateStageText();
        UpdateWaveBar();
    }

    private void Update()
    {
        PageControl();

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isOpenGacha)
            {
                isOpenGacha = false;
            }
            if (isOpenWeapon)
            {
                isOpenWeapon = false;
            }
        }
    }

    // 모든 페이지 초기화 함수
    private void DeactivateAllPages()
    {
        ShopPage.SetActive(false);
        GachaPage.SetActive(false);
        CharacterPage.SetActive(false);
        WeaponPage.SetActive(false);
        ArmorPage.SetActive(false);
        PetPage.SetActive(false);
        OptionPage.SetActive(false);
    }

    private void PageControl()
    {
        DeactivateAllPages();

        if (isOpenShop && !isOpenCharacter && !isOpenPet)
        {
            ShopPage.SetActive(true);
            GachaPage.SetActive(isOpenGacha);
        }
        else if (isOpenCharacter && !isOpenShop && !isOpenPet)
        {
            CharacterPage.SetActive(true);
            WeaponPage.SetActive(isOpenWeapon);
            ArmorPage.SetActive(isOpenArmor);
        }
        else if (isOpenPet && !isOpenCharacter && !isOpenShop)
        {
            PetPage.SetActive(true);
        }
        else if (isOpenOption)
        {
            OptionPage.SetActive(true);
        }
    }

    void UpdateGoldText()
    {
        GoldText.text = TextFormatter.FormatText(gold);
    }

    public void UpdateGemText()
    {
        GemText.text = gem.ToString();
    }

    public void UpdateStageText()
    {
        StageText.text = stageNumber.ToString() + "-" + roundNumber.ToString();
    }

    void UpdateWaveBar()
    {
        waveBar.fillAmount = (float)wave / maxWave;
    }

    public double Gold
    {
        get { return gold; }
    }

    public void IncreaseGold(double amount)
    {
        gold += amount;
        UpdateGoldText();
    }

    public void DecreaseGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldText();
        }
    }
    public void IncreaseGem(int amount)
    {
        gem += amount;
        UpdateGemText();
    }

    public void DecreaseGem(int amount)
    {
        if(gem >= amount)
        {
            gem -= amount;
            OpenRandomPick();
            UpdateGemText();
        }
    }

    public void IncreaseStage()
    {
        roundNumber++;

        if (roundNumber > 10)
        {
            roundNumber = 1;
            stageNumber++;
        }

        UpdateStageText();

        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.SetStageInfo(stageNumber, roundNumber);
        }
    }

    public void IncreaseWave(int amount)
    {
        wave += amount;
        if (wave > maxWave)
        {
            wave = maxWave;
        }
        UpdateWaveBar();
    }

    public void ResetWave()
    {
        wave = 0;
        UpdateWaveBar();
    }

    // 모든 페이지 상태 초기화 함수
    private void ResetAllFlags()
    {
        isOpenShop = false;
        isOpenGacha = false;
        isOpenCharacter = false;
        isOpenWeapon = false;
        isOpenArmor = false;
        isOpenPet = false;
        isOpenOption = false;
        characterX.SetActive(false);
        petX.SetActive(false);
        shopX.SetActive(false);
        characterName.SetActive(true);
        petName.SetActive(true);
        shopName.SetActive(true);
    }

    public void OpenShop()
    {
        bool newState = !isOpenShop;
        ResetAllFlags();
        isOpenShop = newState;

        shopName.SetActive(!isOpenShop);
        shopX.SetActive(isOpenShop);
    }

    public void OpenRandomPick()
    {
        isOpenGacha = !isOpenGacha;
    }

    public void OpenCharacter()
    {
        bool newState = !isOpenCharacter;
        ResetAllFlags();
        isOpenCharacter = newState;

        characterName.SetActive(!isOpenCharacter);
        characterX.SetActive(isOpenCharacter);
    }

    public void OpenPet()
    {
        bool newState = !isOpenPet;
        ResetAllFlags();
        isOpenPet = newState;

        petName.SetActive(!isOpenPet);
        petX.SetActive(isOpenPet);
    }

    public void OpenOption()
    {
        bool newState = !isOpenOption;
        ResetAllFlags();
        isOpenOption = newState;
    }

    public void OpenWeapon()
    {
        isOpenWeapon = !isOpenWeapon;
    }

    public void OpenArmor()
    {
        isOpenArmor = !isOpenArmor;
    }
}
