using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI StageText;
    public GameObject ShopPage;
    public GameObject RandomPickPage;
    public GameObject ArmorPage;

    private int gold = 0;
    private int stageNumber = 1;  // "1-1"�� ���� ���ڸ� �����ϱ� ���� ����
    private int roundNumber = 1;

    public bool isOpenShop = false;
    public bool isOpenRandomPick = false;
    public bool isOpenArmor = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� �Ŵ����� �� ��ȯ �� �ı����� �ʰ� �մϴ�
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
    }

    private void Update()
    {
        if(isOpenShop && !isOpenArmor)
        {
            ShopPage.SetActive(true);
            ArmorPage.SetActive(false);

            if (isOpenRandomPick)
            {
                ShopPage.SetActive(false);
                RandomPickPage.SetActive(true);
            }else
            {
                ShopPage.SetActive(true);
                RandomPickPage.SetActive(false);
            }
        }else if(isOpenArmor && !isOpenShop)
        {
            ShopPage.SetActive(false);
            ArmorPage.SetActive(true);
        }
        else
        {
            ShopPage.SetActive(false);
            RandomPickPage.SetActive(false);
            ArmorPage.SetActive(false);
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
            enemyManager.SetStageInfo(stageNumber, roundNumber); // ���� �������� ���� ������Ʈ
        }
    }

    public void OpenShop()
    {
        isOpenShop = !isOpenShop;
        if (isOpenArmor)
        {
            isOpenArmor = false;
        }
    }

    public void OpenRandomPick()
    {
        isOpenRandomPick = !isOpenRandomPick;
    }

    public void OpenArmor()
    {
        isOpenArmor = !isOpenArmor;
        if (isOpenShop)
        {
            isOpenShop = false;
        }
    }
}