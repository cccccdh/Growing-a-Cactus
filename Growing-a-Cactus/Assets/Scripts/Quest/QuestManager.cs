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
            Debug.LogWarning("QuestManager 인스턴스가 이미 존재합니다.");
        }
    }

    public PlayerStatus playerStatus;
    public GachaManager gachaManager;

    private List<Quest> quests = new List<Quest>();

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

                    quest.IsActive = true;

                    if (quest.UnlockFeature != "None")
                    {
                        Debug.Log("뽑기 해금");
                        UnlockFeature(quest.UnlockFeature);
                    }

                    // UI 업데이트
                    UpdateQuestProgress(0, "");
                    QuestUI.instance.UpdateQuestUI(quest);
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
            if (quest.IsActive)
            {                
                // 적 처치
                if (quest.Description == description)
                {
                    quest.GoalCount += increment;
                }
                // 공격력 강화
                else if (quest.Description == "공격력 강화")
                {
                    quest.GoalCount = playerStatus.Attack_Level;                    
                }
                // 체력 강화
                else if (quest.Description == "체력 강화")
                {
                    quest.GoalCount = playerStatus.Hp_Level;
                }
                // 스테이지 1-10 클리어
                else if (quest.Description == "1 - 10 스테이지 클리어")
                {
                    // 스테이지 클리어와 관련된 목표는 플레이어의 스테이지 상태를 확인해야 합니다.
                    // 이를 위한 로직을 추가합니다.
                }

                QuestUI.instance.UpdateQuestUI(quest);

                break; // 현재 진행 중인 퀘스트 찾은 후 반복문 종료
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
