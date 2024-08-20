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
        questNameText.text = $"Æ©Åä¸®¾ó {quest.Id}";

        questProgressText.text = $"{quest.Description} ( {quest.GoalCount} / {quest.Goal} )";

        if (quest.Reward != 0)
        {
            questRewardText.text = $"º¸»ó : {quest.Reward} Áª";
        }
        else
        {
            questRewardText.text = "";
        }
    }
}
