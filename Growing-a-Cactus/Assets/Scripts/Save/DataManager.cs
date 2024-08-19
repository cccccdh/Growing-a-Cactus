using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DataManager : MonoBehaviour
{

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

        public double PowerLevel;
        public double effectiveHP;

        public double gold;
        public int gem;
        public int stageNumber;
        public int roundNumber;
        public int killedMonsters;

        // EnemyMnager 쪽임 여기
        public int hpCalcA;
        public int hpCalcB;
        public int hpMax;
        public int befHP;
        public int AttackDamage;
        public int befAtt;
        public int DropGold;
        public int befGold;
        public int bossMaxHP;
        public int bossAttackPower;
        public int bossGoldDropAmount;
        public int emroundnumber;

        //item쪽
        //public string name;
        //public string Type;
        //public string Grade;
        //public float Probability;
        //public float RetentionEffect;
        //public float EquipEffect;

        //public int Level;
        //public int Count;
        //public int RequiredCount;

        // itemManager쪽
        // 무기
        public Image WeaponImg;
        public Image EquipWeaponImg;
        public TextMeshProUGUI EquipWeaponText;
        public TextMeshProUGUI EquipWeaponLevelText;
        public TextMeshProUGUI WeaponNameText;
        public TextMeshProUGUI WeaponGradeText;
        public TextMeshProUGUI WeaponLevelText;
        public TextMeshProUGUI WeaponCountText;
        public TextMeshProUGUI WeaponRetentionEffect;
        public TextMeshProUGUI WeaponEquipEffectText;
        public Image[] weaponImages;
        public TextMeshProUGUI[] weaponCountTexts;
        public TextMeshProUGUI[] weaponLevelTexts;
        // 방어구
        public Image ArmorImg;
        public Image EquipArmorImg;
        public TextMeshProUGUI EquipArmorText;
        public TextMeshProUGUI EquipArmorLevelText;
        public TextMeshProUGUI ArmorNameText;
        public TextMeshProUGUI ArmorGradeText;
        public TextMeshProUGUI ArmorLevelText;
        public TextMeshProUGUI ArmorCountText;
        public TextMeshProUGUI ArmorRetentionEffect;
        public TextMeshProUGUI ArmorEquipEffectText;
        public Image[] armorImages;
        public TextMeshProUGUI[] armorCountTexts;
        public TextMeshProUGUI[] armorLevelTexts;

        public List<Item> weaponItems = new List<Item>();
        public List<Item> armorItems = new List<Item>();

        public PlayerStatus playerstatus;

        public string selectedItemName;
        public Color selectedItemColor;


    }

    private string saveFilePath;
    private PlayerStatus playerStatus;
    private UIManager uiManager;
    private GameManager gameManager;
    private QuestScript questScript;
    private PlayerController playerController; // PlayerController 참조 추가
    private EnemyManager enemyManager;
    private EnemyScript enemyScript;
    private Item item;
    private ItemManager itemManager;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        playerStatus = FindObjectOfType<PlayerStatus>();
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        questScript = FindObjectOfType<QuestScript>();
        playerController = FindObjectOfType<PlayerController>();
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyScript = FindObjectOfType<EnemyScript>();
        //item = FindObjectOfType<Item>();
        itemManager = FindObjectOfType<ItemManager>();
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
            Attack_Speed_Cost = (int)playerStatus.Attack_Speed_Cost,
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
            //EnemyManager
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

            //Item
            //name = item.name,
            //Type = item.Type,
            //Grade = item.Grade,
            //Probability = item.Probability,
            //RetentionEffect = item.RetentionEffect,
            //EquipEffect = item.EquipEffect,
            //Level = item.Level,
            //Count = item.Count,
            //RequiredCount = item.RequiredCount,


            //ItemManager
            WeaponImg = itemManager.WeaponImg,
            EquipWeaponImg = itemManager.EquipWeaponImg,
            EquipWeaponText = itemManager.EquipWeaponText,
            EquipWeaponLevelText = itemManager.EquipWeaponLevelText,
            WeaponNameText = itemManager.WeaponNameText,
            WeaponGradeText = itemManager.WeaponGradeText,
            WeaponLevelText = itemManager.WeaponLevelText,
            WeaponCountText = itemManager.WeaponCountText,
            WeaponRetentionEffect = itemManager.WeaponRetentionEffect,
            WeaponEquipEffectText = itemManager.WeaponEquipEffectText,
            weaponImages = itemManager.weaponImages,
            weaponCountTexts = itemManager.weaponCountTexts,
            weaponLevelTexts = itemManager.weaponLevelTexts,

            ArmorImg = itemManager.ArmorImg,
            EquipArmorImg = itemManager.EquipArmorImg,
            EquipArmorText = itemManager.EquipArmorText,
            EquipArmorLevelText = itemManager.EquipArmorLevelText,
            ArmorNameText = itemManager.ArmorNameText,
            ArmorGradeText = itemManager.ArmorGradeText,
            ArmorLevelText = itemManager.ArmorLevelText,
            ArmorCountText = itemManager.ArmorCountText,
            ArmorRetentionEffect = itemManager.ArmorRetentionEffect,
            ArmorEquipEffectText = itemManager.ArmorEquipEffectText,
            armorImages = itemManager.armorImages,
            armorCountTexts = itemManager.armorCountTexts,
            armorLevelTexts = itemManager.armorLevelTexts,

            weaponItems = itemManager.weaponItems,
            armorItems = itemManager.armorItems,

            playerstatus = itemManager.playerstatus,
            selectedItemName = itemManager.selectedItemName,
            selectedItemColor = itemManager.selectedItemColor,


            gold = gameManager.Gold,
            gem = gameManager.gem,
            stageNumber = gameManager.stageNumber,
            roundNumber = gameManager.roundNumber,
            killedMonsters = questScript.killedMonsters

        };



        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, jsonData);
        Debug.Log("게임 저장됨: " + saveFilePath);
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

         
            //item.name = data.name;
            //item.Type = data.Type;
            //item.Grade = data.Grade;
            //item.Probability = data.Probability;
            //item.RetentionEffect = data.RetentionEffect;
            //item.EquipEffect = data.EquipEffect;
            //item.Level = data.Level;
            //item.Count = data.Count;
            //item.RequiredCount = data.RequiredCount;

            itemManager.WeaponImg = data.WeaponImg;
            itemManager.EquipWeaponImg = data.EquipWeaponImg;
            itemManager.EquipWeaponText = data.EquipWeaponText;
            itemManager.EquipWeaponLevelText = data.EquipWeaponLevelText;
            itemManager.WeaponNameText = data.WeaponNameText;
            itemManager.WeaponGradeText = data.WeaponGradeText;
            itemManager.WeaponLevelText = data.WeaponLevelText;
            itemManager.WeaponCountText = data.WeaponCountText;
            itemManager.WeaponRetentionEffect = data.WeaponRetentionEffect;
            itemManager.WeaponEquipEffectText = data.WeaponEquipEffectText;
            itemManager.weaponImages = data.weaponImages;
            itemManager.weaponCountTexts = data.weaponCountTexts;
            itemManager.weaponLevelTexts = data.weaponLevelTexts;

            itemManager.ArmorImg = data.ArmorImg;
            itemManager.EquipArmorImg = data.EquipArmorImg;
            itemManager.EquipArmorText = data.EquipArmorText;
            itemManager.EquipArmorLevelText = data.EquipArmorLevelText;
            itemManager.ArmorNameText = data.ArmorNameText;
            itemManager.ArmorGradeText = data.ArmorGradeText;
            itemManager.ArmorLevelText = data.ArmorLevelText;
            itemManager.ArmorCountText = data.ArmorCountText;
            itemManager.ArmorRetentionEffect = data.ArmorRetentionEffect;
            itemManager.armorImages = data.armorImages;
            itemManager.armorCountTexts = data.armorCountTexts;
            itemManager.armorLevelTexts = data.armorLevelTexts;

            itemManager.weaponItems = data.weaponItems;
            itemManager.armorItems = data.armorItems;

            itemManager.playerstatus = data.playerstatus;

            itemManager.selectedItemName = data.selectedItemName;
            itemManager.selectedItemColor = data.selectedItemColor;


            uiManager.Update_Text("Attack", playerStatus.Attack, playerStatus.Attack_Level, playerStatus.Attack_Cost);
            uiManager.Update_Text("Hp", playerStatus.Hp, playerStatus.Hp_Level, playerStatus.Hp_Cost);
            uiManager.Update_Text("Hp_Recovery", playerStatus.Hp_Recovery, playerStatus.Hp_Recovery_Level, playerStatus.Hp_Recovery_Cost);
            uiManager.Update_Text("Attack_Speed", playerStatus.Attack_Speed, playerStatus.Attack_Speed_Level, (int)playerStatus.Attack_Speed_Cost);
            uiManager.Update_Text("Critical", playerStatus.Critical, playerStatus.Critical_Level, playerStatus.Critical_Cost);
            uiManager.Update_Text("Critical_Damage", playerStatus.Critical_Damage, playerStatus.Critical_Damage_Level, playerStatus.Critical_Damage_Cost);
            uiManager.Update_Text("DoubleAttack", playerStatus.DoubleAttackChance, playerStatus.DoubleAttack_Level, playerStatus.DoubleAttack_Cost);
            uiManager.Update_Text("TripleAttack", playerStatus.TripleAttackChance, playerStatus.TripleAttack_Level, playerStatus.TripleAttack_Cost);

            

            if (gameManager != null)
            {
                gameManager.IncreaseGold(data.gold - gameManager.Gold);
                gameManager.gem = data.gem;
                gameManager.UpdateGemText();
                gameManager.stageNumber = data.stageNumber;
                gameManager.roundNumber = data.roundNumber;
                gameManager.UpdateStageText();
            }

            if (questScript != null)
            {
                questScript.killedMonsters = data.killedMonsters;
                questScript.UpdateQuestText();
            }

            Debug.Log("게임 로드됨: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("저장 파일을 찾을 수 없음");
        }
    }
}
