using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // UIManager와 PlayerController 참조
    private UIManager uiManager;
    private PlayerController playerController;

    // 플레이어 스탯 변수
    public int Attack;
    public int Attack_Level;
    public int Attack_Cost;

    public int Hp;
    public int Hp_Level;
    public int Hp_Cost;

    public float Hp_Recovery;
    public int Hp_Recovery_Level;
    public int Hp_Recovery_Cost;

    public float Attack_Speed;
    public int Attack_Speed_Level;
    public int Attack_Speed_Cost;

    public float Critical;
    public int Critical_Level;
    public int Critical_Cost;

    public float Critical_Damage;
    public int Critical_Damage_Level;
    public int Critical_Damage_Cost;

    public float DoubleAttackChance;
    public int DoubleAttack_Level;
    public int DoubleAttack_Cost;

    public float TripleAttackChance;
    public int TripleAttack_Level;
    public int TripleAttack_Cost;

    public float PowerLevel;

    // 버튼 상태 변수
    private bool isButtonDowning = false;
    private string currentStatus = null;
    private float holdTime = 0f;

    // 아이템 효과 변수
    private float totalEquipEffect = 0f;
    private float totalReactionEffect = 0f;
    private CSVReader.Item equippedItem;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public void Init()
    {
        Attack = 10;
        Attack_Level = 1;
        Attack_Cost = 10;

        Hp = 120;
        Hp_Level = 1;
        Hp_Cost = 5;

        Hp_Recovery = 7;
        Hp_Recovery_Level = 1;
        Hp_Recovery_Cost = 7;

        Attack_Speed = 1f;
        Attack_Speed_Level = 1;
        Attack_Speed_Cost = 22;

        Critical = 0;
        Critical_Level = 1;
        Critical_Cost = 15;

        Critical_Damage = 120;
        Critical_Damage_Level = 1;
        Critical_Damage_Cost = 6;

        DoubleAttackChance = 0f;
        DoubleAttack_Level = 1;
        DoubleAttack_Cost = 50;

        TripleAttackChance = 0f;
        TripleAttack_Level = 1;
        TripleAttack_Cost = 100;

        PowerLevel = Attack;
    }

    public void Increase(string status)
    {
        currentStatus = status;
        isButtonDowning = true;
        holdTime = 0f;
    }

    public void StopIncrease()
    {
        isButtonDowning = false;
        currentStatus = null;
        holdTime = 0f;
    }

    public void OnClickIncrease(string status)
    {
        PerformIncrease(status);
    }

    public void UpdateReactionEffects(List<CSVReader.Item> items)
    {
        totalReactionEffect = 0;
        foreach (var item in items)
        {
            if (item.Count > 0 || (item.Count == 0 && item.Level > 1))
            {
                totalReactionEffect += item.ReactionEffect;
                Debug.Log($"After Enhancement - Total Reaction Effect: {totalReactionEffect}");
            }
        }
        UpdatePowerLevel();
    }

    public void EquipItem(CSVReader.Item item)
    {
        if (equippedItem != null)
        {
            totalEquipEffect = 0;
        }

        equippedItem = item;

        if (equippedItem != null)
        {
            totalEquipEffect += equippedItem.EquipEffect;
        }

        UpdatePowerLevel();
    }

    public CSVReader.Item GetEquippedItem()
    {
        return equippedItem;
    }

    public void UpdatePowerLevel()
    {
        // 로그로 상태 확인
        Debug.Log($"Attack: {Attack}");
        Debug.Log($"totalReactionEffect: {totalReactionEffect}");
        Debug.Log($"totalEquipEffect: {totalEquipEffect}");

        float effectiveAttack = Attack * (1 + totalReactionEffect); // 보유 효과 적용
        effectiveAttack *= (1 + totalEquipEffect); // 장착 효과 적용

        PowerLevel = effectiveAttack;

        // 로그로 계산 결과 확인
        Debug.Log($"Effective Attack: {effectiveAttack}");
        Debug.Log($"PowerLevel: {PowerLevel}");

        uiManager.PowerLevelTEXT(PowerLevel);
    }

    private void Update()
    {
        if (isButtonDowning && currentStatus != null)
        {
            holdTime += Time.deltaTime;
            if (holdTime >= 0.5f)
            {
                PerformIncrease();
            }
        }
    }

    private void PerformIncrease()
    {
        if (currentStatus != null)
        {
            PerformIncrease(currentStatus);
        }
    }

    private void PerformIncrease(string status)
    {
        switch (status)
        {
            case "Attack":
                if (GameManager.instance.Gold >= Attack_Cost)
                {
                    GameManager.instance.DecreaseGold(Attack_Cost);
                    Attack += 10;
                    Attack_Level++;
                    if (Attack_Level % 50 == 0)
                    {
                        Attack_Cost += 15;
                    }
                    else if (Attack_Level % 100 == 0)
                    {
                        Attack_Cost += 30;
                    }
                    else
                    {
                        if (Attack_Level % 10 == 1 || Attack_Level % 10 == 4 || Attack_Level % 10 == 7)
                        {
                            Attack_Cost += 2;
                        }
                        else
                        {
                            Attack_Cost += 1;
                        }
                    }
                    UpdatePowerLevel();
                    uiManager.Update_Text("Attack", Attack, Attack_Level, Attack_Cost);
                }
                break;
            case "Hp":
                if (GameManager.instance.Gold >= Hp_Cost)
                {
                    GameManager.instance.DecreaseGold(Hp_Cost);
                    Hp += 10;
                    Hp_Level++;
                    if (Hp_Level % 50 == 0)
                    {
                        Hp_Cost += 15;
                    }
                    else if (Hp_Level % 100 == 0)
                    {
                        Hp_Cost += 30;
                    }
                    else
                    {
                        if (Hp_Level % 10 == 1 || Hp_Level % 10 == 4 || Hp_Level % 10 == 7)
                        {
                            Hp_Cost += 2;
                        }
                        else
                        {
                            Hp_Cost += 1;
                        }
                    }
                    uiManager.Update_Text("Hp", Hp, Hp_Level, Hp_Cost);
                }
                break;
            case "Hp_Recovery":
                if (GameManager.instance.Gold >= Hp_Recovery_Cost)
                {
                    GameManager.instance.DecreaseGold(Hp_Recovery_Cost);
                    Hp_Recovery += 0.7f;
                    Hp_Recovery_Level++;
                    if (Hp_Recovery_Level % 50 == 0)
                    {
                        Hp_Recovery_Cost += 15;
                    }
                    else if (Hp_Recovery_Level % 100 == 0)
                    {
                        Hp_Recovery_Cost += 30;
                    }
                    else
                    {
                        if (Hp_Recovery_Level % 10 == 1 || Hp_Recovery_Level % 10 == 4 || Hp_Recovery_Level % 10 == 7)
                        {
                            Hp_Recovery_Cost += 2;
                        }
                        else
                        {
                            Hp_Recovery_Cost += 1;
                        }
                    }
                    playerController.SetHpR(Hp_Recovery);
                    uiManager.Update_Text("Hp_Recovery", Hp_Recovery, Hp_Recovery_Level, Hp_Recovery_Cost);
                }
                break;
            case "Attack_Speed":
                if (Attack_Speed_Level >= 200)
                {
                    return; // 강화 불가능
                }

                if (GameManager.instance.Gold >= Attack_Speed_Cost)
                {
                    GameManager.instance.DecreaseGold(Attack_Speed_Cost);
                    Attack_Speed += 0.01f;
                    Attack_Speed_Level++;
                    if (Attack_Speed_Level % 25 == 0)
                    {
                        Attack_Speed_Cost += 15;
                    }
                    else
                    {
                        if (Attack_Speed_Level % 10 == 2 || Attack_Speed_Level % 10 == 4 || Attack_Speed_Level % 10 == 8)
                        {
                            Attack_Speed_Cost *= (int)1.06f;
                        }
                        else
                        {
                            Attack_Speed_Cost *= (int)1.05f;
                        }
                    }
                    uiManager.Update_Text("Attack_Speed", Attack_Speed, Attack_Speed_Level, Attack_Speed_Cost);
                }
                break;
            case "Critical":
                if (GameManager.instance.Gold >= Critical_Cost)
                {
                    GameManager.instance.DecreaseGold(Critical_Cost);
                    Critical += 0.1f;
                    Critical_Level++;
                    if (Critical_Level % 25 == 0)
                    {
                        Critical_Cost += 15;
                    }
                    else
                    {
                        if (Critical_Level % 10 == 2 || Critical_Level % 10 == 4 || Critical_Level % 10 == 8)
                        {
                            Critical_Cost += (int)1.06f;
                        }
                        else
                        {
                            Critical_Cost += (int)1.05f;
                        }
                    }
                    uiManager.Update_Text("Critical", Critical, Critical_Level, Critical_Cost);
                }
                break;
            case "Critical_Damage":
                if (GameManager.instance.Gold >= Critical_Damage_Cost)
                {
                    GameManager.instance.DecreaseGold(Critical_Damage_Cost);
                    Critical_Damage += 1;
                    Critical_Damage_Level++;
                    if (Critical_Damage_Level % 50 == 0)
                    {
                        Critical_Damage_Cost += 15;
                    }
                    else if (Critical_Damage_Level % 100 == 0)
                    {
                        Critical_Damage_Cost += 30;
                    }
                    else
                    {
                        if (Critical_Damage_Level % 10 == 1 || Critical_Damage_Level % 10 == 4 || Critical_Damage_Level % 10 == 7)
                        {
                            Critical_Damage_Cost += 2;
                        }
                        else
                        {
                            Critical_Damage_Cost += 1;
                        }
                    }
                    uiManager.Update_Text("Critical_Damage", Critical_Damage, Critical_Damage_Level, Critical_Damage_Cost);
                }
                break;
            case "DoubleAttack":
                if (Attack_Speed_Level < 200)
                {
                    return;
                }

                if (GameManager.instance.Gold >= DoubleAttack_Cost)
                {
                    GameManager.instance.DecreaseGold(DoubleAttack_Cost);
                    DoubleAttackChance += 0.1f;
                    DoubleAttack_Level++;
                    DoubleAttack_Cost += 10;
                    uiManager.Update_Text("DoubleAttack", DoubleAttackChance, DoubleAttack_Level, DoubleAttack_Cost);
                }
                break;
            case "TripleAttack":
                if (DoubleAttack_Level < 1000)
                {
                    return;
                }

                if (GameManager.instance.Gold >= TripleAttack_Cost)
                {
                    GameManager.instance.DecreaseGold(TripleAttack_Cost);
                    TripleAttackChance += 0.1f;
                    TripleAttack_Level++;
                    TripleAttack_Cost += 20;
                    uiManager.Update_Text("TripleAttack", TripleAttackChance, TripleAttack_Level, TripleAttack_Cost);
                }
                break;
        }
    }
}
