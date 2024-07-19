using TMPro;
using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트를 사용하기 위해 추가

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI StageText;
    public GameObject ShopPage;
    public GameObject RandomPickPage;
    public GameObject CharacterPage;
    public GameObject WeaponsPage;
    public Image waveBar; // 웨이브 바 이미지 추가

    private int gold = 0;
    private int stageNumber = 1;
    private int roundNumber = 1;

    private int wave = 0; // 현재 웨이브를 추적하는 변수 추가
    private const int maxWave = 100; // 최대 웨이브 값

    public bool isOpenShop = false;
    public bool isOpenRandomPick = false;
    public bool isOpenCharacter = false;
    public bool isOpenWeapon = false;

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
        UpdateStageText();
        UpdateWaveBar(); // 초기 웨이브 바 업데이트
    }

    private void Update()
    {
        if (isOpenShop && !isOpenCharacter)
        {
            ShopPage.SetActive(true);
            CharacterPage.SetActive(false);

            if (isOpenRandomPick)
            {
                ShopPage.SetActive(false);
                RandomPickPage.SetActive(true);
            }
            else
            {
                ShopPage.SetActive(true);
                RandomPickPage.SetActive(false);
            }
        }
        else if (isOpenCharacter && !isOpenShop)
        {
            ShopPage.SetActive(false);
            CharacterPage.SetActive(true);

            if(isOpenWeapon)
            {
                WeaponsPage.SetActive(true);
            }
            else
            {
                WeaponsPage.SetActive(false);
            }
        }
        else
        {
            ShopPage.SetActive(false);
            RandomPickPage.SetActive(false);
            CharacterPage.SetActive(false);
            WeaponsPage.SetActive(false);
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

    void UpdateStageText()
    {
        StageText.text = stageNumber.ToString() + "-" + roundNumber.ToString();
    }

    void UpdateWaveBar() // 웨이브 바 업데이트 메서드 추가
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

    public void IncreaseWave(int amount) // 웨이브를 증가시키는 메서드 추가
    {
        wave += amount;
        if (wave > maxWave)
        {
            wave = maxWave;
        }
        UpdateWaveBar();
    }

    public void ResetWave() // 웨이브를 초기화하는 메서드 추가
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
    }

    public void OpenRandomPick()
    {
        isOpenRandomPick = !isOpenRandomPick;
    }

    public void OpenArmor()
    {
        isOpenCharacter = !isOpenCharacter;
        if (isOpenShop)
        {
            isOpenShop = false;
        }
    }

    public void OpenWeapon()
    {
        isOpenWeapon = !isOpenWeapon;
    }
}