using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("QuestManager �ν��Ͻ��� �̹� �����մϴ�.");
        }
    }

    private int equipmentcount;
    private int petcount;
    private int clothescount;
    
    [Header("��ũ��Ʈ ����")]
    public PlayerStatus playerStatus;
    public GachaManager gachaManager;
    public GameManager gameManager;
    public ItemManager itemManager;

    public List<Quest> quests = new List<Quest>();        

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

                // ����Ʈ UI �ʱ�ȭ
                UpdateQuestProgress(0, "���ݷ� ��ȭ");
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

                    // ����Ʈ �Ϸ�
                    quest.IsActive = true;

                    // ����Ʈ UI �ʱ�ȭ
                    UpdateQuestProgress(0, quest.Description);

                    if (quest.UnlockFeature != "None")
                    {
                        Debug.Log("�̱� �ر�");
                        UnlockFeature(quest.UnlockFeature);
                    }
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
            if (quest.IsActive && quest.Description == description)
            {
                switch (description)
                {
                    case "�� óġ":
                        quest.GoalCount += increment;
                        break;

                    case "��� �̱�":
                        equipmentcount += increment;
                        quest.GoalCount = equipmentcount;
                        break;

                    case "�� �̱�":
                        petcount += increment;
                        quest.GoalCount = petcount;
                        break;

                    case "���ݷ� ��ȭ":
                        quest.GoalCount = playerStatus.Attack_Level;
                        break;

                    case "ü�� ��ȭ":
                        quest.GoalCount = playerStatus.Hp_Level;
                        break;

                    case "�������� Ŭ����":
                        quest.GoalCount = gameManager.roundNumber;
                        break;

                    case "ü����� ��ȭ":
                        quest.GoalCount = playerStatus.Hp_Recovery_Level;
                        break;

                    case "ġ��ŸȮ�� ��ȭ":
                        quest.GoalCount = playerStatus.Critical_Level;
                        break;

                    case "���ݼӵ� ��ȭ":
                        quest.GoalCount = playerStatus.Attack_Speed_Level;
                        break; 

                    case "��� ��ȭ":
                        quest.GoalCount = itemManager.GetItemsLevelUp(2);
                        break;

                    case "���� óġ":
                        quest.GoalCount += increment;
                        break;

                    case "�ǻ� �̱�":
                        clothescount += increment;
                        quest.GoalCount = clothescount;
                        break;

                    case "ġ��Ÿ ���� ��ȭ":
                        quest.GoalCount = playerStatus.Critical_Damage_Level;
                        break;

                    default:
                        Debug.LogWarning($"�� �� ���� ����Ʈ ����: {description}");
                        break;
                }

                QuestUI.instance.UpdateQuestUI(quest);
                break;
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

    public void SkipQuest()
    {
        foreach (var quest in quests)
        {
            if (quest.IsActive)
            {
                quest.GoalCount = quest.Goal;
                CompleteQuest(quest.Id); // ����Ʈ �Ϸ� ó��
                break; // �ϳ��� ����Ʈ�� ��ŵ
            }
        }
    }
}
