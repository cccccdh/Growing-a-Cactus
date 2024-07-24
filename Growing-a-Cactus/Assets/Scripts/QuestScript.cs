using UnityEngine;
using TMPro;

public class QuestScript : MonoBehaviour
{
    public TextMeshProUGUI questText; // ��ư�� �ؽ�Ʈ ������Ʈ

    private int killedMonsters = 0; // óġ�� ���� ��
    private int totalMonsters = 100; // �� ���� ��
    private int reward = 500; // ���� ��

    void Start()
    {
        // �ؽ�Ʈ �ʱ�ȭ
        UpdateQuestText();
    }

    void UpdateQuestText()
    {
        // ����Ʈ �ؽ�Ʈ ����
        questText.text = $"�ݺ� ����Ʈ\n���� óġ({killedMonsters}/{totalMonsters})\n����: {reward} ��";
    }

    // ���� óġ ���� ������Ʈ�ϴ� �޼���
    public void IncrementMonsterKillCount()
    {
        killedMonsters++;
        UpdateQuestText();
    }
}