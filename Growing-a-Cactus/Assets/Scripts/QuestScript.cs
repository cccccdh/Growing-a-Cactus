using UnityEngine;
using TMPro;

public class QuestScript : MonoBehaviour
{
    public TextMeshProUGUI questText; // 버튼의 텍스트 컴포넌트

    private int killedMonsters = 0; // 처치한 몬스터 수
    private int totalMonsters = 100; // 총 몬스터 수
    private int reward = 100; // 보상 젬

    void Start()
    {
        // 텍스트 초기화
        UpdateQuestText();
    }

    void UpdateQuestText()
    {
        // 퀘스트 텍스트 변경
        questText.text = string.Format("반복 퀘스트\n몬스터 처치({0}/{1})\n보상: {2} 젬", killedMonsters, totalMonsters, reward);
    }

    // 몬스터 처치 수를 업데이트하는 메서드
    public void IncrementMonsterKillCount()
    {
        killedMonsters++;
        UpdateQuestText();
    }
}