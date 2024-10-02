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
            Debug.LogWarning("QuestManager 가 이미 존재합니다.");
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
    public PetManager petManager;

    public List<Quest> quests = new List<Quest>();        

    private void Update()
    {
        //UpdateQuestProgress(0);
    }

    // 퀘스트 리스트 세팅
    public void SetQuest(List<Quest> questList)
    {
        quests = questList;
        ActivateInitialQuest();
    }

    // 퀘스트의 개수 가져오기
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

    // 초반 퀘스트 활성화
    public void ActivateInitialQuest()
    {
        foreach(var quest in quests)
        {
            if(quest.Requirement == "None" && !quest.IsActive)
            {
                quest.IsActive = true;

                Debug.Log("퀘스트 활성화");

                UpdateQuestProgress(0, "");
                QuestUI.instance.UpdateQuestUI(quest);
            }
        }
    }

    // 퀘스트 완료
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
                    Debug.Log($"다음 퀘스트 : {quest.Title}");

                    // 퀘스트 활성화
                    quest.IsActive = true;

                    UpdateQuestProgress(0, quest.Description);

                    if (quest.UnlockFeature != "None")
                    {
                        Debug.Log("해금");
                        UnlockFeature(quest.UnlockFeature);
                    }
                }
            }
        }
    }

    // 장비, 펫, 의상 해금
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
        else if (feature == "의상 뽑기 해금")
        {
            if (!gachaManager.UnLockPet)
            {
                gachaManager.Unlock("의상");
            }
        }
    }

    // 퀘스트 진행상황 갱신
    public void UpdateQuestProgress(int increment, string description)
    {
        foreach (var quest in quests)
        {
            if (quest.IsActive && quest.Description == description)
            {
                switch (description)
                {
                    case "공격력 강화":
                        quest.GoalCount = playerStatus.Attack_Level;
                        break;

                    case "체력 강화":
                        quest.GoalCount = playerStatus.Hp_Level;
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

                    case "치명타피해 강화":
                        quest.GoalCount = playerStatus.Critical_Damage_Level;
                        break;

                    case "적 처치":
                        quest.GoalCount += increment;
                        break;

                    case "보스 처치":
                        quest.GoalCount += increment;
                        break;

                    case "장비 뽑기":
                        quest.GoalCount = equipmentcount;
                        break;

                    case "펫 뽑기":
                        quest.GoalCount = petcount;
                        break;

                    case "의상 뽑기":
                        quest.GoalCount = clothescount;
                        break;

                    case "장비 강화":
                        quest.GoalCount = itemManager.GetItemsLevelUp(2);
                        break;

                    case "펫 강화":
                        quest.GoalCount = petManager.GetPetsLevelUp(2);
                        break;

                    case "스테이지 클리어":
                        UpdateStageClearQuest(quest);
                        break;

                    case "라운드 클리어":
                        // 해야함
                        break;

                    default:
                        Debug.LogWarning($"확인되지 않은 퀘스트: {description}");
                        break;
                }

                QuestUI.instance.UpdateQuestUI(quest);
                break;
            }
        }
    }

    // 장비 뽑기 누적 횟수
    public void DrawEquipment(int amount)
    {
        equipmentcount += amount;
        UpdateQuestProgress(amount, "장비 뽑기");
    }


    // 펫 뽑기 누적 횟수
    public void DrawPet(int amount)
    {
        petcount += amount;
        UpdateQuestProgress(amount, "펫 뽑기");
    }

    // 의상 뽑기 누적 횟수
    public void DrawClothes(int amount)
    {
        clothescount += amount;
        UpdateQuestProgress(amount, "의상 뽑기");
    }



    // 스테이지 클리어 함수
    private void UpdateStageClearQuest(Quest quest)
    {
        // 1-10 스테이지 클리어
        if (gameManager.stageNumber >= 2)
        {
            quest.GoalCount = quest.Goal;
        }
        // 2-8 스테이지 클리어
        else if (gameManager.stageNumber >= 3 || (gameManager.stageNumber == 2 && gameManager.roundNumber >= 9))
        {
            quest.GoalCount = quest.Goal;
        }

        // 5-1 스테이지 클리어
        else if (gameManager.stageNumber >= 6 || (gameManager.stageNumber == 5 && gameManager.roundNumber >= 2))
        {
            quest.GoalCount = quest.Goal;
        }

        // 진행 중
        else
        {
            quest.GoalCount = gameManager.roundNumber - 1; 
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
                CompleteQuest(quest.Id);
                break; 
            }
        }
    }    
}