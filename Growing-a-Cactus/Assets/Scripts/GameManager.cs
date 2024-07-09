using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI StageText;

    private int gold = 0;
    private int stageNumber = 1;  // "1-1"�� ���� ���ڸ� �����ϱ� ���� ����
    private int roundNumber = 1;

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

    void UpdateGoldText()
    {
        if (gold >= 1000)
        {
            float goldInK = gold / 1000f;
            GoldText.text = goldInK.ToString("0.0") + "k";
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
}
