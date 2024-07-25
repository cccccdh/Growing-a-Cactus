using UnityEngine;
using TMPro;

public class QuestScript : MonoBehaviour
{
    public TextMeshProUGUI questText; // ��ư�� �ؽ�Ʈ ������Ʈ

    public int killedMonsters = 0; // óġ�� ���� ��
    private int totalMonsters = 10; // �� ���� ��
    private int reward = 500; // ���� ��

    void Start()
    {
        // �ؽ�Ʈ �ʱ�ȭ
        UpdateQuestText();
    }

    public void UpdateQuestText()
    {
        // ����Ʈ �ؽ�Ʈ ����
        questText.text = $"�ݺ� ����Ʈ\n���� óġ ({killedMonsters}/{totalMonsters})\n����: {reward} ��";
    }

    // ���� óġ ���� ������Ʈ�ϴ� �޼���
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