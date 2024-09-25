using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string saveFilePath;
    public PlayerStatus playerStatus;
    public UIManager uiManager;
    public GameManager gameManager;
    public PlayerController playerController;
    public EnemyManager enemyManager;
    public EnemyScript enemyScript;
    public ItemManager itemManager;
    public PetManager petManager;
    public QuestUI questUI;
    public QuestManager questManager;
    public QuestCSVReader questCSVReader;
    public GachaManager gachaManager;
    public ItemCSVReader itemCSVReader;
    public StatusUIManager statusUIManager;

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

        public float weaponTotalEquipEffect;
        public float weaponTotalRetentionEffect;
        public float armorTotalEquipEffect;
        public float armorTotalRetentionEffect;
        public float petTotalEquipEffect;
        public float petTotalRetentionEffect;

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

        public string EquipWeaponText;
        public string EquipWeaponLevelText;
        public string EquipArmorText;
        public string EquipArmorLevelText;

        public float EquipWeaponImgR;
        public float EquipWeaponImgG;
        public float EquipWeaponImgB;
        public float EquipWeaponImgA;

        public float EquipArmorImgR;
        public float EquipArmorImgG;
        public float EquipArmorImgB;
        public float EquipArmorImgA;

        public ItemTextData itemtextData;
        public PetTextData pettextData;

        //petManager
        public List<Pet> pets = new List<Pet>();

        public string GradeText;
        public string LevelText;
        public string CountText;
        public string PetName;


        public bool isPetActive;

        // QuestUI
        public string questNameText;
        public string questProgressText;
        public string questRewardText;

        // Quest
        public int Id;
        public string Title;
        public string Description;
        public int Goal;
        public int Reward;
        public int GoalCount;
        public bool IsActive;
        public List<Quest> quests = new List<Quest>();

        // QuestCSVReader
        public List<Quest> questList = new List<Quest>();

        // uiManager
        public string PowerLevelText;

        // gachaManager
        public bool UnLockEquipment;
        public bool UnLockPet;
        public bool UnLockClothes;

        //statusuiManager

        public string stat_Attack;
        public string stat_Hp;
        public string stat_HpRecovery;
        public string stat_AttackSpeed;
        public string stat_Critical;
        public string stat_CriticalDamage;
        public string stat_DoubleAttackChance;
        public string stat_TripleAttackChance;

    }

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        //LoadGame();
    }

    // 게임 데이터를 저장하는 함수
    public void SaveGame()//OnApplicationQuit()
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

            weaponTotalEquipEffect = playerStatus.weaponTotalEquipEffect,
            weaponTotalRetentionEffect = playerStatus.weaponTotalRetentionEffect,
            armorTotalEquipEffect = playerStatus.armorTotalEquipEffect,
            armorTotalRetentionEffect = playerStatus.armorTotalRetentionEffect,
            petTotalEquipEffect = playerStatus.petTotalEquipEffect,
            petTotalRetentionEffect = playerStatus.petTotalRetentionEffect,

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
            EquipWeaponText = itemManager.EquipWeaponText.text,
            EquipWeaponLevelText = itemManager.EquipWeaponLevelText.text,
            EquipArmorText = itemManager.EquipArmorText.text,
            EquipArmorLevelText = itemManager.EquipArmorLevelText.text,
            EquipWeaponImgR = itemManager.EquipWeaponImg.color.r,
            EquipWeaponImgG = itemManager.EquipWeaponImg.color.g,
            EquipWeaponImgB = itemManager.EquipWeaponImg.color.b,
            EquipWeaponImgA = itemManager.EquipWeaponImg.color.a,
            EquipArmorImgR = itemManager.EquipArmorImg.color.r,
            EquipArmorImgG = itemManager.EquipArmorImg.color.g,
            EquipArmorImgB = itemManager.EquipArmorImg.color.b,
            EquipArmorImgA = itemManager.EquipArmorImg.color.a,


            // PetManager
            pets = petManager.pets,
            GradeText = petManager.GradeText.text,
            LevelText = petManager.LevelText.text,
            CountText = petManager.CountText.text,
            isPetActive = petManager.Pet.activeSelf,
            PetName = petManager.PetName.text,

            // QuestUI
            questNameText = questUI.questNameText.text,
            questProgressText = questUI.questProgressText.text,
            questRewardText = questUI.questRewardText.text,

            //uiManager
            PowerLevelText = uiManager.PowerLevel.text,

            // QuestManager
            quests = questManager.quests,

            // gachaManager
            UnLockEquipment = gachaManager.UnLockEquipment,
            UnLockPet = gachaManager.UnLockPet,
            UnLockClothes = gachaManager.UnLockClothes,

            // statusUIManager
            stat_Attack = statusUIManager.stat_Attack.text,
            stat_Hp = statusUIManager.stat_Hp.text,
            stat_HpRecovery = statusUIManager.stat_HpRecovery.text,
            stat_AttackSpeed = statusUIManager.stat_AttackSpeed.text,
            stat_Critical = statusUIManager.stat_Critical.text,
            stat_CriticalDamage = statusUIManager.stat_CriticalDamage.text,
            stat_DoubleAttackChance = statusUIManager.stat_DoubleAttackChance.text,
            stat_TripleAttackChance = statusUIManager.stat_TripleAttackChance.text,

            // GameManager
            gold = gameManager.Gold,
            gem = gameManager.gem,
            stageNumber = gameManager.stageNumber,
            roundNumber = gameManager.roundNumber,
            itemtextData = itemManager.GetItemTextData(),
            pettextData = petManager.GetPetTextData()

        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("게임 저장 완료");
    }

    // 게임 데이터를 불러오는 함수
    public void LoadGame()
    {

        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(jsonData);

  

            //playerStatus
            playerStatus.Attack = data.Attack;
            playerStatus.Attack_Level = data.Attack_Level;
            playerStatus.Attack_Cost = data.Attack_Cost;
            playerStatus.Hp = data.effectiveHP;
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
            playerStatus.weaponTotalEquipEffect = data.weaponTotalEquipEffect;
            playerStatus.weaponTotalRetentionEffect = data.weaponTotalRetentionEffect;
            playerStatus.armorTotalEquipEffect = data.armorTotalEquipEffect;
            playerStatus.armorTotalRetentionEffect = data.armorTotalRetentionEffect;
            playerStatus.petTotalEquipEffect = data.petTotalEquipEffect;
            playerStatus.petTotalRetentionEffect = data.petTotalRetentionEffect;

            //playerController
            playerController.CurrentHp = data.effectiveHP;

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

            itemManager.EquipWeaponText.text = data.EquipWeaponText;
            itemManager.EquipWeaponLevelText.text = data.EquipWeaponLevelText;
            itemManager.EquipArmorText.text = data.EquipArmorText;
            itemManager.EquipArmorLevelText.text = data.EquipArmorLevelText;
            itemManager.EquipWeaponImg.color = new Color(
            data.EquipWeaponImgR,
            data.EquipWeaponImgG,
            data.EquipWeaponImgB,
            data.EquipWeaponImgA
            );
            itemManager.EquipArmorImg.color = new Color(
            data.EquipArmorImgR,
            data.EquipArmorImgG,
            data.EquipArmorImgB,
            data.EquipArmorImgA
            );

            List<Item> ResetItems = new List<Item>();
            foreach (var item in itemManager.weaponItems)
            {
                if (item.Count > 0 || item.Level >= 2) 
                {
                    ResetItems.Add(item);
                }
            }
            foreach (var item in itemManager.armorItems)
            {
                if (item.Count > 0 || item.Level >= 2) 
                {
                    ResetItems.Add(item);
                }
            }
            itemManager.UpdateItemImages(ResetItems);  // 소지 중인 아이템만 전달

            // PetManager
            petManager.pets = data.pets;
            petManager.GradeText.text = data.GradeText;
            petManager.LevelText.text = data.LevelText;
            petManager.CountText.text = data.CountText;
            petManager.PetName.text = data.PetName;

            List<Pet> ownedpets = new List<Pet>();
            foreach (var pet in petManager.pets)
            {
                if (pet.Count > 0 || pet.Level >= 2) // Count가 0보다 큰 아이템만 추가
                {
                    ownedpets.Add(pet);
                }
            }

            if (data.isPetActive)
            {
                petManager.Pet.SetActive(true);  // 활성화
            }
            petManager.UpdateOwnedPetImages(ownedpets);

            petManager.SetTextData(data.pettextData); // TextData 불러오기


            // QuestUI
            questUI.questNameText.text = data.questNameText;
            questUI.questProgressText.text = data.questProgressText;
            questUI.questRewardText.text = data.questRewardText;

            // QuestManager
            questManager.quests = data.quests;

            // GameManager
            gameManager.IncreaseGold(data.gold - gameManager.Gold);
            gameManager.gem = data.gem;
            gameManager.UpdateGemText();
            gameManager.stageNumber = data.stageNumber;
            gameManager.roundNumber = data.roundNumber;
            gameManager.UpdateStageText();

            //gachaManager
            gachaManager.UnLockEquipment = data.UnLockEquipment;
            if (gachaManager.UnLockEquipment == true)
            {
                Debug.Log(gachaManager.UnLockEquipment);
                gachaManager.LoadUnlock("장비");
            }
            gachaManager.UnLockPet = data.UnLockPet;

            if (gachaManager.UnLockPet == true)
            {
                gachaManager.LoadUnlock("펫");
                Debug.Log(gachaManager.UnLockPet);

            }
            gachaManager.UnLockClothes = data.UnLockClothes;

            // 스탯 창 초기화
            statusUIManager.Init_Texts();

            uiManager.PowerLevel.text = data.PowerLevelText;

            Debug.Log("게임 불러오기 완료");
        }
    }
    public void ResetGame()
    {

        if (File.Exists(saveFilePath))
        {
            File.WriteAllText(saveFilePath, "");  // 파일의 내용을 빈 문자열로 덮어쓰기
            Debug.Log("저장 파일의 내용을 초기화했습니다.");

        }

        //playerController
        playerController.CurrentHp = 120;

        playerStatus.Init();

        enemyManager.InitializeStats();
        enemyManager.ResetRound();

        enemyScript.HP = 30;
        enemyScript.maxHP = 30;
        enemyScript.attackPower = 10;
        enemyScript.goldDropAmount = 20;

        playerController.HpR = 10;

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

        itemManager.EquipWeaponText.text = "장착 무기";
        itemManager.EquipWeaponLevelText.text = "";
        itemManager.EquipArmorText.text = "장착 방어구";
        itemManager.EquipArmorLevelText.text = "";
        itemManager.EquipWeaponImg.color = new Color(255, 255, 255, 255);
        itemManager.EquipArmorImg.color = new Color(255, 255, 255, 255);

        // statusUIManager
        statusUIManager.Init_Texts();

        List<Item> resetItems = new List<Item>();
        foreach (var item in itemManager.weaponItems)
        {
            if (item.Count > 0 || item.Level >= 2)
            {
                item.Count = 0;
                item.Level = 1;
                item.RequiredCount = 2;

                resetItems.Add(item);
            }
        }
        foreach (var item in itemManager.armorItems)
        {
            if (item.Count > 0 || item.Level >= 2)
            {
                item.Count = 0;
                item.Level = 1;
                item.RequiredCount = 2;

                resetItems.Add(item);
            }
        }
        itemManager.ResetItemImages(resetItems);  // 소지 중인 아이템만 전달

        foreach (var item in resetItems)
        {
            itemManager.UpdateItemText(item.Name, resetItems);
        }

        // PetManager

        petManager.GradeText.text = "일반";
        petManager.LevelText.text = "Lv.1";
        petManager.CountText.text = "( 0 / 0 )";
        petManager.PetName.text = "토끼";


        List<Pet> resetpets = new List<Pet>();
        foreach (var pet in petManager.pets)
        {
            if (pet.Count > 0 || pet.Level >= 2)
            {
                pet.Count = 0;
                pet.Level = 1;
                pet.RequiredCount = 2;
                resetpets.Add(pet);
            }
        }

        petManager.Pet.SetActive(false);  // 활성화
        petManager.UpdateResetPetImages(resetpets);
        foreach (var pet in resetpets)
        {
            petManager.UpdatePetText(pet.Name);
        }



        // QuestUI

        questCSVReader.questList.Clear(); // 리스트를 명확히 비워줍니다.
        questCSVReader.LoadQuests(); // CSV에서 퀘스트 데이터를 다시 로드

        // QuestManager에 새로운 퀘스트 리스트 설정
        questManager.SetQuest(questCSVReader.questList);

        // UI 초기화
        Quest initialQuest = questManager.quests[0]; // 첫 번째 퀘스트를 가져옵니다.
        questUI.UpdateQuestUI(initialQuest); // 첫 번째 퀘스트 정보를 UI에 반영

        // GameManager
        gameManager.DecreaseGold(gameManager.gold);
        gameManager.gem = 0;
        gameManager.UpdateGemText();
        gameManager.stageNumber = 1;
        gameManager.roundNumber = 1;
        gameManager.UpdateStageText();

        //Debug.Log("게임 불러오기 완료");
        uiManager.PowerLevel.text = $"전투력 : {TextFormatter.FormatText(10)}";
    }
}
