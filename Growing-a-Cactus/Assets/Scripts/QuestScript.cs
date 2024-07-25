using UnityEngine;
using TMPro;

public class QuestScript : MonoBehaviour
{
    public TextMeshProUGUI questText; // 버튼의 텍스트 컴포넌트

    public int killedMonsters = 0; // 처치한 몬스터 수
    private int totalMonsters = 10; // 총 몬스터 수
    private int reward = 500; // 보상 젬

    void Start()
    {
        // 텍스트 초기화
        UpdateQuestText();
    }

    public void UpdateQuestText()
    {
        // 퀘스트 텍스트 변경
        questText.text = $"반복 퀘스트\n몬스터 처치 ({killedMonsters}/{totalMonsters})\n보상: {reward} 젬";
    }

    // 몬스터 처치 수를 업데이트하는 메서드
    public void IncrementMonsterKillCount()
    {
        killedMonsters++;
        UpdateQuestText();
    }

    public void QuestClear()
    {
        if (killedMonsters >= 10)
        {
            GameManager.instance.gem += reward;
            GameManager.instance.UpdateGemText();
            killedMonsters -= 10;
            UpdateQuestText();
        }
    }
}