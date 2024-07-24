using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI StageText;
    public TextMeshProUGUI GemText;
    public GameObject ShopPage;
    public GameObject GachaPage;
    public GameObject CharacterPage;
    public GameObject WeaponsPage;
    public GameObject OptionPage;
    public Image waveBar; // 웨이브 바 이미지 추가


    public int gold = 0;
    public int gem = 0;
    public int stageNumber = 1;
    public int roundNumber = 1;


    private int wave = 0; // 현재 웨이브를 추적하는 변수 추가
    private const int maxWave = 100; // 최대 웨이브 값

    public bool isOpenShop = false;
    public bool isOpenGacha = false;
    public bool isOpenCharacter = false;
    public bool isOpenWeapon = false;
    public bool isOpenOption = false;

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

    void Start()
    {
        UpdateGoldText();
        UpdateGemText();
        UpdateStageText();
        UpdateWaveBar(); // 초기 웨이브 바 업데이트
    }

    private void Update()
    {
        if (isOpenShop && !isOpenCharacter)
        {
            ShopPage.SetActive(true);
            GachaPage.SetActive(false);
            CharacterPage.SetActive(false);
            WeaponsPage.SetActive(false);
            OptionPage.SetActive(false);


            if (isOpenGacha)
            {
                GachaPage.SetActive(true);
            }
            else
            {
                GachaPage.SetActive(false);
                OptionPage.SetActive(false);

            }
        }
        else if (isOpenCharacter && !isOpenShop)
        {
            ShopPage.SetActive(false);
            GachaPage.SetActive(false);
            CharacterPage.SetActive(true);
            WeaponsPage.SetActive(false);
            OptionPage.SetActive(false);


            if (isOpenWeapon)
            {
                WeaponsPage.SetActive(true);
            }
            else
            {
                WeaponsPage.SetActive(false);
                OptionPage.SetActive(false);

            }
        }
        else if (isOpenOption)
        {
            OptionPage.SetActive(true);
            ShopPage.SetActive(false);
            GachaPage.SetActive(false);
            CharacterPage.SetActive(false);
            WeaponsPage.SetActive(false);

        }
        else 
        {
            ShopPage.SetActive(false);
            GachaPage.SetActive(false);
            CharacterPage.SetActive(false);
            WeaponsPage.SetActive(false);
            OptionPage.SetActive(false);

        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }            
    }

    void UpdateGoldText()
    {
        if (gold >= 1000)
        {
            int unitIndex = -1;
            float displayGold = gold;

            while (displayGold >= 1000 && unitIndex < 25)
            {
                displayGold /= 1000f;
                unitIndex++;
            }

            char unitChar = (char)('A' + unitIndex);
            GoldText.text = displayGold.ToString("0.0") + unitChar;
        }
        else
        {
            GoldText.text = gold.ToString();
        }
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

    public int Gold
    {
        get { return gold; }
    }

    public void IncreaseGold(int amount)
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

    public void IncreaseStage()
    {
        roundNumber++;
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

    public void OpenShop()
    {
        isOpenShop = !isOpenShop;
        if (isOpenCharacter)
        {
            isOpenCharacter = false;            
        }

        if(isOpenWeapon)
        {
            isOpenWeapon = false;
        }

        if (isOpenGacha)
        {
            isOpenGacha = false;
        }

        if (isOpenOption)
        {
            isOpenOption = false;
        }
    }

    public void OpenRandomPick()
    {
        isOpenGacha = !isOpenGacha;
    }

    public void OpenArmor()
    {
        isOpenCharacter = !isOpenCharacter;
        if (isOpenShop)
        {
            isOpenShop = false;
        }

        if (isOpenGacha)
        {
            isOpenGacha = false;
        }

        if (isOpenWeapon)
        {
            isOpenWeapon = false;
        }

        if (isOpenOption)
        {
            isOpenOption = false;
        }
    }

    public void OpenOption()
    {
        isOpenOption = !isOpenOption;
        if (isOpenShop)
        {
            isOpenShop = false;
        }

        if (isOpenGacha)
        {
            isOpenGacha = false;
        }

        if (isOpenWeapon)
        {
            isOpenWeapon = false;
        }

        if (isOpenCharacter)
        {
            isOpenCharacter = false;
        }
    }

    public void OpenWeapon()
    {
        isOpenWeapon = !isOpenWeapon;
    }
}