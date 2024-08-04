using TMPro;
using UnityEngine;

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


    public void Update_Text(string status, float stat, int level, int cost)
    {
        void UpdateTextFields(TextMeshProUGUI levelText, TextMeshProUGUI costText)
        {
            levelText.text = "Lv." + level.ToString();
            costText.text = TextFormatter.FormatText(cost);
        }

        switch (status)
        {
            case "Attack":
                UpdateTextFields(Attack_Level, Attack_Cost);
                Attack.text = TextFormatter.FormatText(stat);
                break;
            case "Hp":
                UpdateTextFields(Hp_Level, Hp_Cost);
                Hp.text = TextFormatter.FormatText(stat);
                break;
            case "Hp_Recovery":
                UpdateTextFields(Hp_Recovery_Level, Hp_Recovery_Cost);
                Hp_Recovery.text = stat.ToString("N1");
                break;
            case "Attack_Speed":
                UpdateTextFields(Attack_Speed_Level, Attack_Speed_Cost);
                Attack_Speed.text = stat.ToString("N2");
                break;
            case "Critical":
                UpdateTextFields(Critical_Level, Critical_Cost);
                Critical.text = stat.ToString("N2") + "%";
                break;
            case "Critical_Damage":
                UpdateTextFields(Critical_Damage_Level, Critical_Damage_Cost);
                Critical_Damage.text = stat.ToString() + "%";
                break;
            case "DoubleAttack":
                UpdateTextFields(DoubleAttack_Level, DoubleAttack_Cost);
                DoubleAttack.text = stat.ToString("N1") + "%";
                break;
            case "TripleAttack":
                UpdateTextFields(TripleAttack_Level, TripleAttack_Cost);
                TripleAttack.text = stat.ToString("N1") + "%";
                break;
        }
    }

    public void PowerLevelTEXT(float power)
    {
        PowerLevel.text = $"ÀüÅõ·Â : {TextFormatter.FormatText(power)}";
    }
}
