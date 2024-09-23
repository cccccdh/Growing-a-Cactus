using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public struct Texts
    {
        public TextMeshProUGUI at;
    }

    [Header("���ݷ�")]
    public TextMeshProUGUI Attack_Level;
    public TextMeshProUGUI Attack;
    public TextMeshProUGUI Attack_Cost;

    [Header("ü��")]
    public TextMeshProUGUI Hp_Level;
    public TextMeshProUGUI Hp;
    public TextMeshProUGUI Hp_Cost;

    [Header("ü�����")]
    public TextMeshProUGUI Hp_Recovery_Level;
    public TextMeshProUGUI Hp_Recovery;
    public TextMeshProUGUI Hp_Recovery_Cost;

    [Header("���ݼӵ�")]
    public TextMeshProUGUI Attack_Speed_Level;
    public TextMeshProUGUI Attack_Speed;
    public TextMeshProUGUI Attack_Speed_Cost;

    [Header("ġ��ŸȮ��")]
    public TextMeshProUGUI Critical_Level;
    public TextMeshProUGUI Critical;
    public TextMeshProUGUI Critical_Cost;

    [Header("ġ��Ÿ������")]
    public TextMeshProUGUI Critical_Damage_Level;
    public TextMeshProUGUI Critical_Damage;
    public TextMeshProUGUI Critical_Damage_Cost;

    [Header("������")]
    public TextMeshProUGUI DoubleAttack_Level;
    public TextMeshProUGUI DoubleAttack;
    public TextMeshProUGUI DoubleAttack_Cost;
    public GameObject LockDoubleAttack;

    [Header("Ʈ���ð���")]
    public TextMeshProUGUI TripleAttack_Level;
    public TextMeshProUGUI TripleAttack;
    public TextMeshProUGUI TripleAttack_Cost;
    public GameObject LockTripleAttack;

    [Header("������")]
    public TextMeshProUGUI PowerLevel;

    [Header("���� ��ư & ���")]
    public Button[] buttons;
    public TextMeshProUGUI[] costs;

    private StringBuilder sb = new StringBuilder();

    public void Update_Text(string status, double stat, int level, int cost)
    {
        void UpdateTextFields(TextMeshProUGUI levelText, TextMeshProUGUI costText, int level, int cost)
        {
            sb.Clear(); // StringBuilder ����
            sb.Append("Lv.").Append(level);
            levelText.text = sb.ToString();

            // ��� ������ (TextFormatter�� ����)
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
                Hp_Recovery.text = TextFormatter.FormatText(stat);
                break;
            case "Attack_Speed":
                UpdateTextFields(Attack_Speed_Level, Attack_Speed_Cost, level, cost);
                Attack_Speed.text = stat.ToString("N2");
                break;
            case "Critical":
                UpdateTextFields(Critical_Level, Critical_Cost, level, cost);
                Critical.text = string.Format("{0:N1}%", stat);  // String.Format ���
                break;
            case "Critical_Damage":
                UpdateTextFields(Critical_Damage_Level, Critical_Damage_Cost, level, cost);
                Critical_Damage.text = string.Format("{0}%", stat);
                break;
            case "DoubleAttack":
                UpdateTextFields(DoubleAttack_Level, DoubleAttack_Cost, level, cost);
                DoubleAttack.text = string.Format("{0:N1}%", stat);
                break;
            case "TripleAttack":
                UpdateTextFields(TripleAttack_Level, TripleAttack_Cost, level, cost);
                TripleAttack.text = string.Format("{0:N1}%", stat);
                break;
        }
    }

    public void PowerLevelTEXT(double power)
    {
        PowerLevel.text = $"������ : {TextFormatter.FormatText(power)}";
    }

    // ��ư ���� ������Ʈ �޼���
    public void UpdateButtonInteractivity(string status, bool isInteractable)
    {
        int index = GetIndexFromStatus(status);
        if (index >= 0 && index < buttons.Length)
        {
            buttons[index].interactable = isInteractable;
        }
    }

    //// ��� ���� ������Ʈ �޼���
    //public void Update_Cost(string status, int cost)
    //{
    //    int index = GetIndexFromStatus(status);
    //    if (index >= 0 && index < costs.Length)
    //    {
    //        costs[index].text = TextFormatter.FormatText(cost);
    //    }
    //}

    // ���¿� ���� �ε����� ��ȯ�ϴ� �޼���
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
            default: return -1;  // ���¿� �´� �ε����� ���� ���
        }
    }

    public void UnLock(string status)
    {
        switch(status)
        {
            case "DoubleAttack":
                LockDoubleAttack.SetActive(false);
                return;
            case "TripleAttack":
                LockTripleAttack.SetActive(false);
                return;
        }
    }
}
