using UnityEngine;
using TMPro;

public class QuestScript : MonoBehaviour
{
    public TextMeshProUGUI questText; // ��ư�� �ؽ�Ʈ ������Ʈ

    private int killedMonsters = 0; // óġ�� ���� ��
    private int totalMonsters = 100; // �� ���� ��
    private int reward = 100; // ���� ��

    void Start()
    {
        // �ؽ�Ʈ �ʱ�ȭ
        UpdateQuestText();
    }

    void UpdateQuestText()
    {
        // ����Ʈ �ؽ�Ʈ ����
        questText.text = string.Format("�ݺ� ����Ʈ\n���� óġ({0}/{1})\n����: {2} ��", killedMonsters, totalMonsters, reward);
    }

    // ���� óġ ���� ������Ʈ�ϴ� �޼���
    public void IncrementMonsterKillCount()
    {
        killedMonsters++;
        UpdateQuestText();
    }
}