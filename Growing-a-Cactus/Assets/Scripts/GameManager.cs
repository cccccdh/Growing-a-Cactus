using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI StageText;

    private int gold = 0;
    private int stageNumber = 1;  // "1-1"의 뒤의 숫자를 추적하기 위한 변수
    private int roundNumber = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 게임 매니저가 씬 전환 시 파괴되지 않게 합니다
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
            enemyManager.SetStageInfo(stageNumber, roundNumber); // 현재 스테이지 정보 업데이트
        }
    }
}
