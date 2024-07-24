using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    UIManager UImanager;

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

    public float PowerLevel;

    bool IsButtonDowning = false;
    string currentStatus = null;
    float holdTime = 0f;

    private void Awake()
    {
        UImanager = FindObjectOfType<UIManager>();
    }

    public void Init()
    {
        Attack = 10; Attack_Level = 1; Attack_Cost = 10;
        Hp = 120; Hp_Level = 1; Hp_Cost = 5;
        Hp_Recovery = 7; Hp_Recovery_Level = 1; Hp_Recovery_Cost = 7;
        Attack_Speed = 1f; Attack_Speed_Level = 1; Attack_Speed_Cost = 22;
        Critical = 0; Critical_Level = 1; Critical_Cost = 15;
        Critical_Damage = 120; Critical_Damage_Level = 1; Critical_Damage_Cost = 6;
        PowerLevel = Attack;
    }

    public void Increase(string status)
    {
        currentStatus = status;
        IsButtonDowning = true;
        holdTime = 0f;
        if (holdTime >= 0.5f)
        {
            PerformIncrease();
        }
    }

    public void StopIncrease()
    {
        IsButtonDowning = false;
        currentStatus = null;
        holdTime = 0f;
    }

    public void OnClickIncrease(string status)
    {
        PerformIncrease(status);
    }

    //public void UpdatePowerLevel(List<CSVReader.Item> items)
    //{
    //    float totalAttackPower = Attack;

    //    foreach (var item in items)
    //    {          
    //        if(item.Count > 0)
    //        {
    //            totalAttackPower += Attack * item.ReactionEffect;
    //        }
    //    }

    //    PowerLevel = totalAttackPower;
    //    UImanager.PowerLevelTEXT(PowerLevel);
    //}

    private void Update()
    {
        if (IsButtonDowning && currentStatus != null)
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
                    UImanager.Update_Text("Attack", Attack, Attack_Level, Attack_Cost);
                }
                break;
            case "Hp":
                if (GameManager.instance.Gold >= Hp_Cost)
                {
                    GameManager.instance.DecreaseGold(Hp_Cost);
                    Hp += 10;
                    Hp_Level++;
                    Hp_Cost += 10;
                    UImanager.Update_Text("Hp", Hp, Hp_Level, Hp_Cost);
                }
                break;
            case "Hp_Recovery":
                if (GameManager.instance.Gold >= Hp_Recovery_Cost)
                {
                    GameManager.instance.DecreaseGold(Hp_Recovery_Cost);
                    Hp_Recovery += 0.7f;
                    Hp_Recovery_Level++;
                    Hp_Recovery_Cost += 10;
                    UImanager.Update_Text("Hp_Recovery", Hp_Recovery, Hp_Recovery_Level, Hp_Recovery_Level);
                }
                break;
            case "Attack_Speed":
                if (GameManager.instance.Gold >= Attack_Speed_Cost)
                {
                    GameManager.instance.DecreaseGold(Attack_Speed_Cost);
                    Attack_Speed += 0.01f;
                    Attack_Speed_Level++;
                    Attack_Speed_Cost += 8;
                    UImanager.Update_Text("Attack_Speed", Attack_Speed, Attack_Speed_Level, Attack_Speed_Cost);
                }
                break;
            case "Critical":
                if (GameManager.instance.Gold >= Critical_Cost)
                {
                    GameManager.instance.DecreaseGold(Critical_Cost);
                    Critical += 0.1f;
                    Critical_Level++;
                    Critical_Cost += 6;
                    UImanager.Update_Text("Critical", Critical, Critical_Level, Critical_Cost);
                }
                break;
            case "Critical_Damage":
                if (GameManager.instance.Gold >= Critical_Damage_Cost)
                {
                    GameManager.instance.DecreaseGold(Critical_Damage_Cost);
                    Critical_Damage += 1;
                    Critical_Damage_Level++;
                    Critical_Damage_Cost += 5;
                    UImanager.Update_Text("Critical_Damage", Critical_Damage, Critical_Damage_Level, Critical_Damage_Cost);
                }
                break;
        }
    }
}