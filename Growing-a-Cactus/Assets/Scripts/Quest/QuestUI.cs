using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public static QuestUI instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        InitializeColor();
    }

    public Image questImage;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questDescriptionText;
    public TextMeshProUGUI questProgressText;
    public TextMeshProUGUI questRewardText;

    Color alpha;
    Color highlight;
    Color defalut;
       
    private void InitializeColor()
    {
        // 색상 초기화
        alpha = questImage.color;
        highlight = new Color(203f / 255f, 203f / 255f, 0, alpha.a);
        defalut = new Color(0f, 0f, 0f, alpha.a);
    }

    public void UpdateQuestUI(Quest quest)
    {
        questImage.color = quest.GoalCount >= quest.Goal ? highlight : defalut;

        questNameText.text = $"[ 퀘스트 {quest.Id} ]";

        questDescriptionText.text = $"{quest.Title}";

        questProgressText.text = $"( {quest.GoalCount} / {quest.Goal} )";

        questRewardText.text = quest.Reward != 0 ? $"보상 : {quest.Reward} 젬" : "";
    }
}
