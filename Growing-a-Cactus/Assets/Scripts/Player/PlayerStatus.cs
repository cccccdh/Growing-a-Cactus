using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // UIManager와 PlayerController 참조
    public UIManager uiManager;
    public StatusUIManager statusUIManager;
    public PlayerController playerController;

    // 플레이어 스탯 변수
    public double Attack;
    public int Attack_Level;
    public int Attack_Cost;
    public int Increase_Attack;

    public double Hp;
    public int Hp_Level;
    public int Hp_Cost;
    public int Increase_HP;

    public float Hp_Recovery;
    public int Hp_Recovery_Level;
    public int Hp_Recovery_Cost;
    public float Increase_Hp_Recovery;

    public float Attack_Speed;
    public int Attack_Speed_Level;
    public float Attack_Speed_Cost;

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

    public double PowerLevel;
    public double effectiveHP;

    // 버튼 상태 변수
    private bool isButtonDowning = false;
    private string currentStatus = null;
    private float holdTime = 0f;

    // 아이템 효과 변수
    public float weaponTotalEquipEffect;
    public float weaponTotalRetentionEffect = 0f;
    public float armorTotalEquipEffect = 0f;
    public float armorTotalRetentionEffect = 0f;
    public float petTotalEquipEffect = 0f;
    public float petTotalRetentionEffect = 0f;
    private float clothesTotalRetentionEffect = 0f;

    public Item equippedWeapon;
    public Item equippedArmor;
    public Pet equippedPet;



    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        Attack = 10;
        Attack_Level = 1;
        Attack_Cost = 10;
        Increase_Attack = 10;

        Hp = 120;
        Hp_Level = 1;
        Hp_Cost = 5;
        Increase_HP = 10;

        Hp_Recovery = 10;
        Hp_Recovery_Level = 1;
        Hp_Recovery_Cost = 7;
        Increase_Hp_Recovery = 3;

        Attack_Speed = 1f;
        Attack_Speed_Level = 1;
        Attack_Speed_Cost = 22;

        Critical = 0.1f;
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
        effectiveHP = Hp;

        weaponTotalEquipEffect = 0f;
        weaponTotalEquipEffect = 0f;
        weaponTotalRetentionEffect = 0f;
        armorTotalEquipEffect = 0f;
        armorTotalRetentionEffect = 0f;
        petTotalEquipEffect = 0f;
        petTotalRetentionEffect = 0f;
        clothesTotalRetentionEffect = 0f;

        uiManager.PowerLevelTEXT(PowerLevel);
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

    // 무기 보유효과 -> 공격력 증가
    public void UpdateWeaponRetentionEffects(List<Item> items)
    {
        weaponTotalRetentionEffect = 0;
        foreach (var item in items)
        {
            if (item.Count > 0 || (item.Count == 0 && item.Level > 1))
            {
                weaponTotalRetentionEffect += item.RetentionEffect;
                //Debug.Log($"전체 보유효과 : {weaponTotalRetentionEffect}");
            }
        }
        UpdatePowerLevel();
    }

    // 방어구 보유효과 -> 체력 증가
    public void UpdateArmorRetentionEffects(List<Item> items)
    {
        armorTotalRetentionEffect = 0;
        foreach (var item in items)
        {
            if (item.Count > 0 || (item.Count == 0 && item.Level > 1))
            {
                armorTotalRetentionEffect += item.RetentionEffect;
                //Debug.Log($"전체 보유효과 : {armorTotalRetentionEffect}");
            }
        }
        UpdateHP();
    }
    
    // 펫 보유효과 -> 공격력 증가
    public void UpdatePetRetentionEffects(List<Pet> pets)
    {
        petTotalRetentionEffect = 0;
        foreach (var pet in pets)
        {
            if (pet.Count > 0 || (pet.Count == 0 && pet.Level > 1))
            {
                petTotalRetentionEffect += pet.RetentionEffect;
                //Debug.Log($"전체 펫 보유효과 : {petTotalRetentionEffect}");
            }
        }
        UpdatePowerLevel();
    }

    // 의상 보유효과 -> 공격력 증가
    public void UpdateClothesRetentionEffects(List<Clothes> clothes)
    {
        clothesTotalRetentionEffect = 0;
        foreach (var cloth in clothes)
        {
            if (cloth.Count > 0 || (cloth.Count == 0 && cloth.Level > 1))
            {
                clothesTotalRetentionEffect += cloth.RetentionEffect;
                //Debug.Log($"전체 의상 보유효과 : {clothesTotalRetentionEffect}");
            }
        }
        UpdatePowerLevel();
    }

    // 무기 장착 효과 -> 공격력 증가
    public void EquipWeapon(Item item)
    {
        if (equippedWeapon != null)
        {
            weaponTotalEquipEffect = 0;
        }

        equippedWeapon = item;

        weaponTotalEquipEffect += equippedWeapon.EquipEffect;

        UpdatePowerLevel();
    }

    // 방어구 장착효과 -> 체력 증가
    public void EquipArmor(Item item)
    {
        if (equippedArmor != null)
        {
            armorTotalEquipEffect = 0;
        }

        equippedArmor = item;

        armorTotalEquipEffect += equippedArmor.EquipEffect;

        UpdateHP();
    }

    // 펫 장착효과 -> 공격력 증가
    public void EquipPet(Pet pet)
    {
        if (equippedPet != null)
        {
            petTotalEquipEffect = 0;
        }

        equippedPet = pet;

        petTotalEquipEffect += equippedPet.EquipEffect;

        UpdatePowerLevel();
    }

    public Item GetEquippedWeapon() => equippedWeapon;

    public Item GetEquippedArmor() => equippedArmor;

    public Pet GetEquippedPet() => equippedPet;

    public void UpdatePowerLevel()
    {
        // 로그로 상태 확인
        //Debug.Log($"공격력 : {Attack}");
        //Debug.Log($"총 무기 보유효과 : {weaponTotalRetentionEffect}");
        //Debug.Log($"총 무기 장착효과 : {weaponTotalEquipEffect}");
        //Debug.Log($"총 펫 보유효과 : {petTotalRetentionEffect}");
        //Debug.Log($"총 펫 장착효과 : {petTotalEquipEffect}");

        double effect = Attack * (1 + weaponTotalRetentionEffect + petTotalRetentionEffect + clothesTotalRetentionEffect); // 보유 효과 적용
        effect *= (1 + weaponTotalEquipEffect + petTotalEquipEffect); // 장착 효과 적용

        PowerLevel = effect;

        // 로그로 계산 결과 확인
        //Debug.Log($"전투력 : {PowerLevel}");
        //Debug.Log($"==================================================");

        // UI 갱신
        uiManager.PowerLevelTEXT(PowerLevel);
    }

    public void UpdateHP()
    {
        // 로그로 상태 확인
        //Debug.Log($"체력 : {Hp}");
        //Debug.Log($"총 보유효과 : {armorTotalRetentionEffect}");
        //Debug.Log($"총 장착효과 : {armorTotalEquipEffect}");

        double effect = Hp * (1 + armorTotalRetentionEffect); // 보유 효과 적용
        effect *= (1 + armorTotalEquipEffect); // 장착 효과 적용

        effectiveHP = effect;

        // 로그로 계산 결과 확인
        //Debug.Log($"체력 : {effectiveHP}");

        // UI 갱신
        uiManager.Update_Text("Hp", effectiveHP, Hp_Level, Hp_Cost);
        statusUIManager.UpdateStatText("Hp");
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

        UpdateButtonInteractivity();
    }

    // 보유 골드에 따라 버튼 상태 확인 함수
    public void UpdateButtonInteractivity()
    {
        UpdateButtonState("Attack", GameManager.instance.Gold >= Attack_Cost);
        UpdateButtonState("Hp", GameManager.instance.Gold >= Hp_Cost);
        UpdateButtonState("Hp_Recovery", GameManager.instance.Gold >= Hp_Recovery_Cost);
        UpdateButtonState("Attack_Speed", GameManager.instance.Gold >= Attack_Speed_Cost);
        UpdateButtonState("Critical", GameManager.instance.Gold >= Critical_Cost);
        UpdateButtonState("Critical_Damage", GameManager.instance.Gold >= Critical_Damage_Cost);
        UpdateButtonState("DoubleAttack", GameManager.instance.Gold >= DoubleAttack_Cost);
        UpdateButtonState("TripleAttack", GameManager.instance.Gold >= TripleAttack_Cost);
    }

    // 버튼 상태 변경
    private void UpdateButtonState(string status, bool isInteractable)
    {
        uiManager.UpdateButtonInteractivity(status, isInteractable);
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
                if (GameManager.instance.Gold >= Attack_Cost) EnhanceAttack();
                    break;
            case "Hp":
                if (GameManager.instance.Gold >= Hp_Cost) EnhanceHp();
                    break;
            case "Hp_Recovery":
                if (GameManager.instance.Gold >= Hp_Recovery_Cost) EnhanceHpRecovery();
                    break;
            case "Attack_Speed":                
                if (GameManager.instance.Gold >= Attack_Speed_Cost) EnhanceAttackSpeed();
                    break;
            case "Critical":
                if (GameManager.instance.Gold >= Critical_Cost) EnhanceCritical();
                    break;
            case "Critical_Damage":
                if (GameManager.instance.Gold >= Critical_Damage_Cost) EnhanceCriticalDamage();
                    break;
            case "DoubleAttack":          
                if (GameManager.instance.Gold >= DoubleAttack_Cost) EnhanceDoubleAttack();
                    break;
            case "TripleAttack":
                if (GameManager.instance.Gold >= TripleAttack_Cost) EnhanceTripleAttack();
                    break;
        }
    }   

    // 공격력 강화
    private void EnhanceAttack()
    {
        GameManager.instance.DecreaseGold(Attack_Cost);
        Attack += Increase_Attack;
        Attack_Level++;
        if (Attack_Level % 5 == 0) Increase_Attack++;
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

        // 퀘스트 반영
        QuestManager.instance.UpdateQuestProgress(0, "공격력 강화");

        // UI 반영
        uiManager.Update_Text("Attack", Attack, Attack_Level, Attack_Cost);
        statusUIManager.UpdateStatText("Attack");
    }

    // 체력 강화
    private void EnhanceHp()
    {
        GameManager.instance.DecreaseGold(Hp_Cost);
        Hp += Increase_HP;
        Hp_Level++;
        if (Hp_Level % 5 == 0) Increase_HP++;
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

        UpdateHP();
        playerController.SetHp(Increase_HP);
        
        // 퀘스트 반영
        QuestManager.instance.UpdateQuestProgress(0, "체력 강화");

        // UI 갱신
        uiManager.Update_Text("Hp", effectiveHP, Hp_Level, Hp_Cost);
        statusUIManager.UpdateStatText("Hp");
    }

    // 체력재생 강화
    private void EnhanceHpRecovery()
    {
        GameManager.instance.DecreaseGold(Hp_Recovery_Cost);
        Hp_Recovery += Increase_Hp_Recovery;
        if (Hp_Recovery_Level % 6 == 0) Increase_Hp_Recovery += 2;
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
        
        QuestManager.instance.UpdateQuestProgress(0, "체력재생 강화");

        uiManager.Update_Text("Hp_Recovery", Hp_Recovery, Hp_Recovery_Level, Hp_Recovery_Cost);
        statusUIManager.UpdateStatText("Hp_Recovery");
    }

    // 공격속도 강화
    private void EnhanceAttackSpeed()
    {
        if (Attack_Speed_Level >= 200) 
            return;

        GameManager.instance.DecreaseGold((int)Attack_Speed_Cost);
        Attack_Speed += 0.01f;
        Attack_Speed_Level++;
        if (Attack_Speed_Level >= 15)
        {
            if (Attack_Speed_Level % 25 == 0)
            {
                Attack_Speed_Cost += 15;
            }
            else
            {
                if (Attack_Speed_Level % 10 == 2 || Attack_Speed_Level % 10 == 4 || Attack_Speed_Level % 10 == 8)
                {
                    Attack_Speed_Cost *= 1.06f;
                }
                else
                {
                    Attack_Speed_Cost *= 1.05f;
                }
            }

            if (Attack_Speed_Level == 200)
                Attack_Speed = 3;
        }
        else
        {
            switch (Attack_Speed_Level)
            {
                case 1:
                    Attack_Speed_Cost *= 1.3f;
                    break;
                case 2:
                    Attack_Speed_Cost *= 1.3f;
                    break;
                case 3:
                    Attack_Speed_Cost *= 1.3f;
                    break;
                case 4:
                    Attack_Speed_Cost *= 1.2f;
                    break;
                case 5:
                    Attack_Speed_Cost *= 1.15f;
                    break;
                case 6:
                    Attack_Speed_Cost *= 1.15f;
                    break;
                case 7:
                    Attack_Speed_Cost *= 1.1f;
                    break;
                case 8:
                    Attack_Speed_Cost *= 1.1f;
                    break;
                case 9:
                    Attack_Speed_Cost *= 1.08f;
                    break;
                case 10:
                    Attack_Speed_Cost *= 1.08f;
                    break;
                case 11:
                    Attack_Speed_Cost *= 1.06f;
                    break;
                case 12:
                    Attack_Speed_Cost *= 1.06f;
                    break;
                case 13:
                    Attack_Speed_Cost *= 1.06f;
                    break;
                case 14:
                    Attack_Speed_Cost *= 1.06f;
                    break;
            }
        }

        if (Attack_Speed_Level == 200)
            uiManager.UnLock("DoubleAttack");

        QuestManager.instance.UpdateQuestProgress(0, "공격속도 강화");

        uiManager.Update_Text("Attack_Speed", Attack_Speed, Attack_Speed_Level, (int)Attack_Speed_Cost);
        statusUIManager.UpdateStatText("Attack_Speed");
    }

    // 치명타확률 강화
    private void EnhanceCritical()
    {
        if (Critical_Level >= 1000)
            return;

        GameManager.instance.DecreaseGold(Critical_Cost);
        Critical += 0.1f;
        Critical_Level++;
        double cost = 0;
            if (Critical_Level % 10 == 2 || Critical_Level % 10 == 4 || Critical_Level % 10 == 8)
            {
                cost = (double)Critical_Cost * 1.03f;
            }
            else
            {
                cost = (double)Critical_Cost * 1.04f;
            }
            Critical_Cost = (int)Math.Ceiling(cost);
        
        QuestManager.instance.UpdateQuestProgress(0, "치명타확률 강화");

        uiManager.Update_Text("Critical", Critical, Critical_Level, Critical_Cost);
        statusUIManager.UpdateStatText("Critical");
    }

    // 치명타데미지 강화
    private void EnhanceCriticalDamage()
    {
        GameManager.instance.DecreaseGold(Critical_Damage_Cost);
        Critical_Damage += 0.3f;
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
        
        // UI 갱신
        uiManager.Update_Text("Critical_Damage", Critical_Damage, Critical_Damage_Level, Critical_Damage_Cost);
        statusUIManager.UpdateStatText("Critical_Damage");
    }

    // 더블가시 강화
    private void EnhanceDoubleAttack()
    {
        if (Attack_Speed_Level < 200)
            return;

        if (DoubleAttack_Level >= 1000)
            return;

        GameManager.instance.DecreaseGold(DoubleAttack_Cost);
        DoubleAttackChance += 0.1f;
        DoubleAttack_Level++;
        DoubleAttack_Cost += 10; 

        if (DoubleAttack_Level == 1000)
            uiManager.UnLock("TripleAttack");

        // UI 갱신
        uiManager.Update_Text("DoubleAttack", DoubleAttackChance, DoubleAttack_Level, DoubleAttack_Cost);
        statusUIManager.UpdateStatText("DoubleAttack");
    }

    // 트리플 가시 강화
    private void EnhanceTripleAttack()
    {
        if (DoubleAttack_Level < 1000)
            return;

        if (TripleAttack_Level >= 1000)
            return;

        GameManager.instance.DecreaseGold(TripleAttack_Cost);
        TripleAttackChance += 0.1f;
        TripleAttack_Level++;
        TripleAttack_Cost += 20;
        
        // UI 갱신
        uiManager.Update_Text("TripleAttack", TripleAttackChance, TripleAttack_Level, TripleAttack_Cost);
        statusUIManager.UpdateStatText("TripleAttack");
    }
}
