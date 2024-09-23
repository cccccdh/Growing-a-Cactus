using TMPro;
using UnityEngine;

public class StatusUIManager : MonoBehaviour
{
    [Header("플레이어 스텟 텍스트")]
    public TextMeshProUGUI stat_Attack;
    public TextMeshProUGUI stat_Hp;
    public TextMeshProUGUI stat_HpRecovery;
    public TextMeshProUGUI stat_AttackSpeed;
    public TextMeshProUGUI stat_Critical;
    public TextMeshProUGUI stat_CriticalDamage;
    public TextMeshProUGUI stat_DoubleAttackChance;
    public TextMeshProUGUI stat_TripleAttackChance;

    [Header("스크립트 참조")]
    public PlayerStatus status;

    private void Start()
    {
        Init_Texts();
    }

    public void Init_Texts()
    {
        stat_Attack.text = $"{status.Attack}";
        stat_Hp.text = $"{status.effectiveHP}";
        stat_HpRecovery.text = $"{status.Hp_Recovery}";
        stat_AttackSpeed.text = $"{status.Attack_Speed}";
        stat_Critical.text = $"{status.Critical}%";
        stat_CriticalDamage.text = $"{status.Critical_Damage}%";
        stat_DoubleAttackChance.text = $"{status.DoubleAttackChance}%";
        stat_TripleAttackChance.text = $"{status.TripleAttackChance}%";
    }

    public void UpdateStatText(string statType)
    {
        switch (statType)
        {
            case "Attack":
                stat_Attack.text = TextFormatter.FormatText(status.Attack);
                break;
            case "Hp":
                stat_Hp.text = TextFormatter.FormatText(status.effectiveHP);
                break;
            case "Hp_Recovery":
                stat_HpRecovery.text = $"{status.Hp_Recovery}";
                break;
            case "Attack_Speed":
                stat_AttackSpeed.text = $"{status.Attack_Speed:F2}";
                break;
            case "Critical":
                stat_Critical.text = $"{status.Critical:F1}%";
                break;
            case "Critical_Damage":
                stat_CriticalDamage.text = $"{status.Critical_Damage}%";
                break;
            case "DoubleAttackChance":
                stat_DoubleAttackChance.text = $"{status.DoubleAttackChance:F1}%";
                break;
            case "TripleAttackChance":
                stat_TripleAttackChance.text = $"{status.TripleAttackChance:F1}%";
                break;
            default:
                Debug.LogWarning("Invalid stat type");
                break;
        }
    }
}
