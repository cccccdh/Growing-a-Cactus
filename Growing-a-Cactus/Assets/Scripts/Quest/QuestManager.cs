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
            Debug.LogWarning("QuestManager 인스턴스가 이미 존재합니다.");
        }
    }

    private int equipmentcount;
    private int petcount;
    private int clothescount;
    
    [Header("스크립트 참조")]
    public PlayerStatus playerStatus;
    public GachaManager gachaManager;
    public GameManager gameManager;
    public ItemManager itemManager;

    public List<Quest> quests = new List<Quest>();        

    private void Update()
    {
        //UpdateQuestProgress(0);
    }

    // 퀘스트 리스트 초기화
    public void SetQuest(List<Quest> questList)
    {
        quests = questList;
        ActivateInitialQuest();
    }

    // 퀘스트 진행 개수를 가져오는 메서드
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

    // 초기 퀘스트 활성화
    public void ActivateInitialQuest()
    {
        foreach(var quest in quests)
        {
            if(quest.Requirement == "None" && !quest.IsActive)
            {
                quest.IsActive = true;

                // 퀘스트 활성화
                Debug.Log("퀘스트 활성화");

                // 퀘스트 UI 초기화
                UpdateQuestProgress(0, "공격력 강화");
                QuestUI.instance.UpdateQuestUI(quest);
            }
        }
    }

    // 퀘스트 완료 및 다음 퀘스트 활성화
    public void CompleteQuest(int questId)
    {
        Quest completedQuest = quests.Find(q => q.Id == questId);

        if(completedQuest != null)
        {
            Debug.Log($"퀘스트 완료 : {completedQuest.Title}");

            completedQuest.IsActive = false;

            // 보상 증정
            GameManager.instance.IncreaseGem(completedQuest.Reward);

            foreach (var quest in quests)
            {
                if (quest.Requirement == $"Complete Quest {completedQuest.Id}" && !quest.IsActive)
                {
                    Debug.Log($"다음 퀘스트 시작 : {quest.Title}");

                    // 퀘스트 완료
                    quest.IsActive = true;

                    // 퀘스트 UI 초기화
                    UpdateQuestProgress(0, quest.Description);

                    if (quest.UnlockFeature != "None")
                    {
                        Debug.Log("뽑기 해금");
                        UnlockFeature(quest.UnlockFeature);
                    }
                }
            }
        }
    }

    // 퀘스트 뽑기 해금
    private void UnlockFeature(string feature)
    {
        if(feature == "장비 뽑기 해금")
        {
            if (!gachaManager.UnLockEquipment)
            {
                gachaManager.Unlock("장비");
            }
        }
        else if(feature == "펫 뽑기 해금")
        {
            if (!gachaManager.UnLockPet)
            {
                gachaManager.Unlock("펫");
            }
        }
    }

    // 퀘스트 진행 상황 업데이트
    public void UpdateQuestProgress(int increment, string description)
    {
        foreach (var quest in quests)
        {
            if (quest.IsActive && quest.Description == description)
            {
                switch (description)
                {
                    case "적 처치":
                        quest.GoalCount += increment;
                        break;

                    case "장비 뽑기":
                        equipmentcount += increment;
                        quest.GoalCount = equipmentcount;
                        break;

                    case "펫 뽑기":
                        petcount += increment;
                        quest.GoalCount = petcount;
                        break;

                    case "공격력 강화":
                        quest.GoalCount = playerStatus.Attack_Level;
                        break;

                    case "체력 강화":
                        quest.GoalCount = playerStatus.Hp_Level;
                        break;

                    case "스테이지 클리어":
                        quest.GoalCount = gameManager.roundNumber;
                        break;

                    case "체력재생 강화":
                        quest.GoalCount = playerStatus.Hp_Recovery_Level;
                        break;

                    case "치명타확률 강화":
                        quest.GoalCount = playerStatus.Critical_Level;
                        break;

                    case "공격속도 강화":
                        quest.GoalCount = playerStatus.Attack_Speed_Level;
                        break; 

                    case "장비 강화":
                        quest.GoalCount = itemManager.GetItemsLevelUp(2);
                        break;

                    case "보스 처치":
                        quest.GoalCount += increment;
                        break;

                    case "의상 뽑기":
                        clothescount += increment;
                        quest.GoalCount = clothescount;
                        break;

                    case "치명타 피해 강화":
                        quest.GoalCount = playerStatus.Critical_Damage_Level;
                        break;

                    default:
                        Debug.LogWarning($"알 수 없는 퀘스트 설명: {description}");
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
                CompleteQuest(quest.Id); // 퀘스트 완료 처리
                break; // 하나의 퀘스트만 스킵
            }
        }
    }
}
