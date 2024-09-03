using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    private string saveFilePath;
    public PlayerStatus playerStatus;
    public UIManager uiManager;
    public GameManager gameManager;
    public PlayerController playerController;
    public EnemyManager enemyManager;
    public EnemyScript enemyScript;
    public Item item;
    public ItemManager itemManager;
    public GachaManager gachaManager;
    public GachaUIManager gachaUIManager;

    [System.Serializable]
    public class GameData
    {
        public double Attack;
        public int Attack_Level;
        public int Attack_Cost;

        public double Hp;
        public int Hp_Level;
        public int Hp_Cost;

        public float Hp_Recovery;
        public int Hp_Recovery_Level;
        public int Hp_Recovery_Cost;

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

        // GamaManager
        public double gold;
        public int gem;
        public int stageNumber;
        public int roundNumber;

        // EnemyManager
        public double hpCalcA;
        public double hpCalcB;
        public double hpMax;
        public double befHP;
        public double AttackDamage;
        public double befAtt;
        public int DropGold;
        public int befGold;
        public double bossMaxHP;
        public double bossAttackPower;
        public int bossGoldDropAmount;
        public int emroundnumber;

        // itemManager
        public List<Item> weaponItems = new List<Item>();
        public List<Item> armorItems = new List<Item>();


        public PlayerStatus playerstatus;
    }

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    // 게임 데이터를 저장하는 함수
    public void SaveGame()
    {
        GameData data = new GameData
        {
            Attack = playerStatus.Attack,
            Attack_Level = playerStatus.Attack_Level,
            Attack_Cost = playerStatus.Attack_Cost,
            Hp = playerStatus.Hp,
            Hp_Level = playerStatus.Hp_Level,
            Hp_Cost = playerStatus.Hp_Cost,
            Hp_Recovery = playerStatus.Hp_Recovery,
            Hp_Recovery_Level = playerStatus.Hp_Recovery_Level,
            Hp_Recovery_Cost = playerStatus.Hp_Recovery_Cost,
            Attack_Speed = playerStatus.Attack_Speed,
            Attack_Speed_Level = playerStatus.Attack_Speed_Level,
            Attack_Speed_Cost = playerStatus.Attack_Speed_Cost,
            Critical = playerStatus.Critical,
            Critical_Level = playerStatus.Critical_Level,
            Critical_Cost = playerStatus.Critical_Cost,
            Critical_Damage = playerStatus.Critical_Damage,
            Critical_Damage_Level = playerStatus.Critical_Damage_Level,
            Critical_Damage_Cost = playerStatus.Critical_Damage_Cost,
            DoubleAttackChance = playerStatus.DoubleAttackChance,
            DoubleAttack_Level = playerStatus.DoubleAttack_Level,
            DoubleAttack_Cost = playerStatus.DoubleAttack_Cost,
            TripleAttackChance = playerStatus.TripleAttackChance,
            TripleAttack_Level = playerStatus.TripleAttack_Level,
            TripleAttack_Cost = playerStatus.TripleAttack_Cost,
            PowerLevel = playerStatus.PowerLevel,
            effectiveHP = playerStatus.effectiveHP,

            // EnemyManager
            hpCalcA = enemyManager.hpCalcA,
            hpCalcB = enemyManager.hpCalcB,
            hpMax = enemyManager.HpMax,
            befHP = enemyManager.befHP,
            AttackDamage = enemyManager.AttackDamage,
            befAtt = enemyManager.befAtt,
            DropGold = enemyManager.DropGold,
            befGold = enemyManager.befGold,
            bossGoldDropAmount = enemyManager.bossGoldDropAmount,
            bossAttackPower = enemyManager.bossAttackPower,
            bossMaxHP = enemyManager.bossMaxHP,
            emroundnumber = enemyManager.roundNumber,

            // ItemManager
            weaponItems = itemManager.weaponItems,
            armorItems = itemManager.armorItems,
            

              playerstatus = playerStatus,  // playerStatus는 현재 playerStatus 인스턴스를 참조
            // GameManager
            gold = gameManager.Gold,
            gem = gameManager.gem,
            stageNumber = gameManager.stageNumber,
            roundNumber = gameManager.roundNumber,
        };

        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, jsonData);
        Debug.Log("게임 저장됨: " + saveFilePath);
        //PrintItemData();

    }

    public void PrintItemData()
    {
        // 무기 데이터 출력
        foreach (var item in itemManager.weaponItems)
        {
            Debug.Log($"무기 데이터: 이름={item.Name}, 레벨={item.Level}, 개수={item.Count}, 등급={item.Grade}");
        }

        // 방어구 데이터 출력
        foreach (var item in itemManager.armorItems)
        {
            Debug.Log($"방어구 데이터: 이름={item.Name}, 레벨={item.Level}, 개수={item.Count}, 등급={item.Grade}");
        }
    }

    // 게임 데이터를 불러오는 함수
    public void LoadGame()
    {

        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(jsonData);

            playerStatus.Attack = data.Attack;
            playerStatus.Attack_Level = data.Attack_Level;
            playerStatus.Attack_Cost = data.Attack_Cost;
            playerStatus.Hp = data.Hp;
            playerStatus.Hp_Level = data.Hp_Level;
            playerStatus.Hp_Cost = data.Hp_Cost;
            playerStatus.Hp_Recovery = data.Hp_Recovery;
            playerStatus.Hp_Recovery_Level = data.Hp_Recovery_Level;
            playerStatus.Hp_Recovery_Cost = data.Hp_Recovery_Cost;
            playerStatus.Attack_Speed = data.Attack_Speed;
            playerStatus.Attack_Speed_Level = data.Attack_Speed_Level;
            playerStatus.Attack_Speed_Cost = data.Attack_Speed_Cost;
            playerStatus.Critical = data.Critical;
            playerStatus.Critical_Level = data.Critical_Level;
            playerStatus.Critical_Cost = data.Critical_Cost;
            playerStatus.Critical_Damage = data.Critical_Damage;
            playerStatus.Critical_Damage_Level = data.Critical_Damage_Level;
            playerStatus.Critical_Damage_Cost = data.Critical_Damage_Cost;
            playerStatus.DoubleAttackChance = data.DoubleAttackChance;
            playerStatus.DoubleAttack_Level = data.DoubleAttack_Level;
            playerStatus.DoubleAttack_Cost = data.DoubleAttack_Cost;
            playerStatus.TripleAttackChance = data.TripleAttackChance;
            playerStatus.TripleAttack_Level = data.TripleAttack_Level;
            playerStatus.TripleAttack_Cost = data.TripleAttack_Cost;
            playerStatus.PowerLevel = data.PowerLevel;
            playerStatus.effectiveHP = data.effectiveHP;

            enemyManager.hpCalcA = data.hpCalcA;
            enemyManager.hpCalcB = data.hpCalcB;
            enemyManager.HpMax = data.hpMax;
            enemyManager.befHP = data.befHP;
            enemyManager.AttackDamage = data.AttackDamage;
            enemyManager.befAtt = data.befAtt;
            enemyManager.DropGold = data.DropGold;
            enemyManager.befGold = data.befGold;
            enemyManager.bossAttackPower = data.bossAttackPower;
            enemyManager.bossGoldDropAmount = data.bossGoldDropAmount;
            enemyManager.bossMaxHP = data.bossMaxHP;
            enemyManager.roundNumber = data.emroundnumber;

            enemyScript.HP = data.befHP;
            enemyScript.maxHP = data.befHP;
            enemyScript.attackPower = data.AttackDamage;
            enemyScript.goldDropAmount = data.DropGold;

            playerController.HpR = data.Hp_Recovery;

            // uiManager
            uiManager.Update_Text("Attack", playerStatus.Attack, playerStatus.Attack_Level, playerStatus.Attack_Cost);
            uiManager.Update_Text("Hp", playerStatus.Hp, playerStatus.Hp_Level, playerStatus.Hp_Cost);
            uiManager.Update_Text("Hp_Recovery", playerStatus.Hp_Recovery, playerStatus.Hp_Recovery_Level, playerStatus.Hp_Recovery_Cost);
            uiManager.Update_Text("Attack_Speed", playerStatus.Attack_Speed, playerStatus.Attack_Speed_Level, (int)playerStatus.Attack_Speed_Cost);
            uiManager.Update_Text("Critical", playerStatus.Critical, playerStatus.Critical_Level, playerStatus.Critical_Cost);
            uiManager.Update_Text("Critical_Damage", playerStatus.Critical_Damage, playerStatus.Critical_Damage_Level, playerStatus.Critical_Damage_Cost);
            uiManager.Update_Text("DoubleAttack", playerStatus.DoubleAttackChance, playerStatus.DoubleAttack_Level, playerStatus.DoubleAttack_Cost);
            uiManager.Update_Text("TripleAttack", playerStatus.TripleAttackChance, playerStatus.TripleAttack_Level, playerStatus.TripleAttack_Cost);

            // ItemManager
            itemManager.weaponItems = data.weaponItems;
            itemManager.armorItems = data.armorItems;
            itemManager.UpdateUI();

            itemManager.playerstatus = data.playerstatus;

            // 보유 중인 아이템에 대해 텍스트 업데이트
            foreach (var item in itemManager.weaponItems)
            {
                if (item.Count > 0) // 소지한 아이템만 업데이트
                {
                    itemManager.UpdateItemText(item.Name, itemManager.weaponItems);
                }
            }

            foreach (var item in itemManager.armorItems)
            {
                if (item.Count > 0) // 소지한 아이템만 업데이트
                {
                    itemManager.UpdateItemText(item.Name, itemManager.armorItems);
                }
            }

            List<Item> ownedItems = new List<Item>();
            foreach (var item in itemManager.weaponItems)
            {
                if (item.Count > 0) // Count가 0보다 큰 아이템만 추가
                {
                    ownedItems.Add(item);
                }
            }
            foreach (var item in itemManager.armorItems)
            {
                if (item.Count > 0) // Count가 0보다 큰 아이템만 추가
                {
                    ownedItems.Add(item);
                }
            }
            itemManager.UpdateItemImages(ownedItems);  // 소지 중인 아이템만 전달

            // PrintItemData();

            // GameManager
            if (gameManager != null)
            {
                gameManager.IncreaseGold(data.gold - gameManager.Gold);
                gameManager.gem = data.gem;
                gameManager.UpdateGemText();
                gameManager.stageNumber = data.stageNumber;
                gameManager.roundNumber = data.roundNumber;
                gameManager.UpdateStageText();
            }
            Debug.Log("게임 불러오기 완료");

        }
        else
        {
            Debug.LogWarning("저장된 게임 파일이 없습니다.");
        }
    }
    
}
