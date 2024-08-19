using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI Attack_Level;
    public TextMeshProUGUI Attack;
    public TextMeshProUGUI Attack_Cost;

    public TextMeshProUGUI Hp_Level;
    public TextMeshProUGUI Hp;
    public TextMeshProUGUI Hp_Cost;

    public TextMeshProUGUI Hp_Recovery_Level;
    public TextMeshProUGUI Hp_Recovery;
    public TextMeshProUGUI Hp_Recovery_Cost;

    public TextMeshProUGUI Attack_Speed_Level;
    public TextMeshProUGUI Attack_Speed;
    public TextMeshProUGUI Attack_Speed_Cost;

    public TextMeshProUGUI Critical_Level;
    public TextMeshProUGUI Critical;
    public TextMeshProUGUI Critical_Cost;

    public TextMeshProUGUI Critical_Damage_Level;
    public TextMeshProUGUI Critical_Damage;
    public TextMeshProUGUI Critical_Damage_Cost;

    public TextMeshProUGUI DoubleAttack_Level;
    public TextMeshProUGUI DoubleAttack;
    public TextMeshProUGUI DoubleAttack_Cost;

    public TextMeshProUGUI TripleAttack_Level;
    public TextMeshProUGUI TripleAttack;
    public TextMeshProUGUI TripleAttack_Cost;

    public TextMeshProUGUI PowerLevel;

    public Button[] buttons;
    public TextMeshProUGUI[] costs;

    public void Update_Text(string status, double stat, int level, int cost)
    {
        static void UpdateTextFields(TextMeshProUGUI levelText, TextMeshProUGUI costText, int level, int cost)
        {
            // StringBuilder를 이용한 문자열 연결 최적화
            var sb = new StringBuilder();
            sb.Append("Lv.").Append(level);
            levelText.text = sb.ToString();

            // 비용 포맷팅 (TextFormatter를 가정)
            costText.text = TextFormatter.FormatText(cost);
        }

        switch (status)
        {
            case "Attack":
                UpdateTextFields(Attack_Level, Attack_Cost, level, cost);
                Attack.text = TextFormatter.FormatText(stat);
                break;
            case "Hp":
                UpdateTextFields(Hp_Level, Hp_Cost, level, cost);
                Hp.text = TextFormatter.FormatText(stat);
                break;
            case "Hp_Recovery":
                UpdateTextFields(Hp_Recovery_Level, Hp_Recovery_Cost, level, cost);
                Hp_Recovery.text = stat.ToString("N1");
                break;
            case "Attack_Speed":
                UpdateTextFields(Attack_Speed_Level, Attack_Speed_Cost, level, cost);
                Attack_Speed.text = stat.ToString("N2");
                break;
            case "Critical":
                UpdateTextFields(Critical_Level, Critical_Cost, level, cost);
                Critical.text = $"{stat:N2}%";  // 문자열 보간 사용 (GC 최소화)
                break;
            case "Critical_Damage":
                UpdateTextFields(Critical_Damage_Level, Critical_Damage_Cost, level, cost);
                Critical_Damage.text = $"{stat}%";
                break;
            case "DoubleAttack":
                UpdateTextFields(DoubleAttack_Level, DoubleAttack_Cost, level, cost);
                DoubleAttack.text = $"{stat:N1}%";
                break;
            case "TripleAttack":
                UpdateTextFields(TripleAttack_Level, TripleAttack_Cost, level, cost);
                TripleAttack.text = $"{stat:N1}%";
                break;
        }
    }

    public void PowerLevelTEXT(double power)
    {
        PowerLevel.text = $"전투력 : {TextFormatter.FormatText(power)}";
    }

    // 버튼 상태 업데이트 메서드
    public void UpdateButtonInteractivity(string status, bool isInteractable)
    {
        int index = GetIndexFromStatus(status);
        if (index >= 0 && index < buttons.Length)
        {
            buttons[index].interactable = isInteractable;
        }
    }

    public void Update_Cost(string status, int cost)
    {
        int index = GetIndexFromStatus(status);
        if (index >= 0 && index < costs.Length)
        {
            costs[index].text = TextFormatter.FormatText(cost);
        }
    }

    // 상태에 따른 인덱스를 반환하는 메서드
    private int GetIndexFromStatus(string status)
    {
        switch (status)
        {
            case "Attack": return 0;
            case "Hp": return 1;
            case "Hp_Recovery": return 2;
            case "Attack_Speed": return 3;
            case "Critical": return 4;
            case "Critical_Damage": return 5;
            case "DoubleAttack": return 6;
            case "TripleAttack": return 7;
            default: return -1;  // 상태에 맞는 인덱스가 없을 경우
        }
    }
}
