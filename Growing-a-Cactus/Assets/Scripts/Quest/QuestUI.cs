using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public static QuestUI instance;

    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questProgressText;
    public TextMeshProUGUI questRewardText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void UpdateQuestUI(Quest quest)
    {
        questNameText.text = $"Ʃ�丮�� {quest.Id}";

        questProgressText.text = $"{quest.Description} ( {quest.GoalCount} / {quest.Goal} )";

        if (quest.Reward != 0)
        {
            questRewardText.text = $"���� : {quest.Reward} ��";
        }
        else
        {
            questRewardText.text = "";
        }
    }
}
