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


    public void Update_Text(string status, float stat, int level, int cost)
    {
        switch (status)
        {
            case "Attack":
                Attack_Level.text = "Lv." + level.ToString();
                Attack.text = stat.ToString();
                Attack_Cost.text = cost.ToString();
                break;
            case "Hp":
                Hp_Level.text = "Lv." + level.ToString();
                Hp.text = stat.ToString();
                Hp_Cost.text = cost.ToString();
                break;
            case "Hp_Recovery":
                Hp_Recovery_Level.text = "Lv." + level.ToString();
                Hp_Recovery.text = stat.ToString("N1");
                Hp_Recovery_Cost.text = cost.ToString();
                break;
            case "Attack_Speed":
                Attack_Speed_Level.text = "Lv." + level.ToString();
                Attack_Speed.text = stat.ToString("N2");
                Attack_Speed_Cost.text = cost.ToString();
                break;
            case "Critical":
                Critical_Level.text = "Lv." + level.ToString();
                Critical.text = stat.ToString("N2") + "%";
                Critical_Cost.text = cost.ToString();
                break;
            case "Critical_Damage":
                Critical_Damage_Level.text = "Lv." + level.ToString();
                Critical_Damage.text = stat.ToString() + "%";
                Critical_Damage_Cost.text = cost.ToString();
                break;
        }
    }    
}
