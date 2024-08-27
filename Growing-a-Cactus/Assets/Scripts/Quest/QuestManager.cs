using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("QuestManager �ν��Ͻ��� �̹� �����մϴ�.");
        }
    }

    public PlayerStatus playerStatus;
    public GachaManager gachaManager;

    private List<Quest> quests = new List<Quest>();

    private void Update()
    {
        //UpdateQuestProgress(0);
    }

    // ����Ʈ ����Ʈ �ʱ�ȭ
    public void SetQuest(List<Quest> questList)
    {
        quests = questList;
        ActivateInitialQuest();
    }

    // ����Ʈ ���� ������ �������� �޼���
    public int GetQuestGoalCountInList(int questId)
    {
        int count = 0;
        foreach (var quest in quests)
        {
            if (quest.Id == questId)
            {
                return quest.GoalCount;
            }
        }
        return count;
    }

    // �ʱ� ����Ʈ Ȱ��ȭ
    public void ActivateInitialQuest()
    {
        foreach(var quest in quests)
        {
            if(quest.Requirement == "None" && !quest.IsActive)
            {
                quest.IsActive = true;
                // ����Ʈ Ȱ��ȭ
                Debug.Log("����Ʈ Ȱ��ȭ");
                QuestUI.instance.UpdateQuestUI(quest);
            }
        }
    }

    // ����Ʈ �Ϸ� �� ���� ����Ʈ Ȱ��ȭ
    public void CompleteQuest(int questId)
    {
        Quest completedQuest = quests.Find(q => q.Id == questId);

        if(completedQuest != null)
        {
            Debug.Log($"����Ʈ �Ϸ� : {completedQuest.Title}");

            completedQuest.IsActive = false;

            // ���� ����
            GameManager.instance.IncreaseGem(completedQuest.Reward);

            foreach (var quest in quests)
            {
                if (quest.Requirement == $"Complete Quest {completedQuest.Id}" && !quest.IsActive)
                {
                    Debug.Log($"���� ����Ʈ ���� : {quest.Title}");

                    quest.IsActive = true;

                    if (quest.UnlockFeature != "None")
                    {
                        Debug.Log("�̱� �ر�");
                        UnlockFeature(quest.UnlockFeature);
                    }

                    // UI ������Ʈ
                    UpdateQuestProgress(0, "");
                    QuestUI.instance.UpdateQuestUI(quest);
                }
            }
        }
    }

    // ����Ʈ �̱� �ر�
    private void UnlockFeature(string feature)
    {
        if(feature == "��� �̱� �ر�")
        {
            if (!gachaManager.UnLockEquipment)
            {
                gachaManager.Unlock("���");
            }
        }
        else if(feature == "�� �̱� �ر�")
        {
            if (!gachaManager.UnLockPet)
            {
                gachaManager.Unlock("��");
            }
        }
    }

    // ����Ʈ ���� ��Ȳ ������Ʈ
    public void UpdateQuestProgress(int increment, string description)
    {
        foreach (var quest in quests)
        {
            if (quest.IsActive)
            {                
                // �� óġ
                if (quest.Description == description)
                {
                    quest.GoalCount += increment;
                }
                // ���ݷ� ��ȭ
                else if (quest.Description == "���ݷ� ��ȭ")
                {
                    quest.GoalCount = playerStatus.Attack_Level;                    
                }
                // ü�� ��ȭ
                else if (quest.Description == "ü�� ��ȭ")
                {
                    quest.GoalCount = playerStatus.Hp_Level;
                }
                // �������� 1-10 Ŭ����
                else if (quest.Description == "1 - 10 �������� Ŭ����")
                {
                    // �������� Ŭ����� ���õ� ��ǥ�� �÷��̾��� �������� ���¸� Ȯ���ؾ� �մϴ�.
                    // �̸� ���� ������ �߰��մϴ�.
                }

                QuestUI.instance.UpdateQuestUI(quest);

                break; // ���� ���� ���� ����Ʈ ã�� �� �ݺ��� ����
            }
        }
    }

    public void Reward()
    {
        foreach(var quest in quests)
        {
            if (quest.GoalCount >= quest.Goal)
            {
                CompleteQuest(quest.Id);
                quest.GoalCount = 0;
                break;
            }
        }
    }
}
