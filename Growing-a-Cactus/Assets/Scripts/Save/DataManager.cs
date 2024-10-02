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

        public string EquipWeaponImgsprite;
        public string EquipArmorImgsprite;

        public string EquipWeaponEquipEffectText;
        public string EquipArmorEquipEffectText;


        public ItemTextData itemtextData;

        //petManager
        public PetTextData pettextData;
        public List<Pet> pets = new List<Pet>();

        public string GradeText;
        public string LevelText;
        public string CountText;
        public string NameText;

        public string PetImgSprite;

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
       // LoadGame();
    }

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
            EquipWeaponImgR = itemManager.EquipWeaponImgBG.color.r,
            EquipWeaponImgG = itemManager.EquipWeaponImgBG.color.g,
            EquipWeaponImgB = itemManager.EquipWeaponImgBG.color.b,
            EquipWeaponImgA = itemManager.EquipWeaponImgBG.color.a,
            EquipArmorImgR = itemManager.EquipArmorImgBG.color.r,
            EquipArmorImgG = itemManager.EquipArmorImgBG.color.g,
            EquipArmorImgB = itemManager.EquipArmorImgBG.color.b,
            EquipArmorImgA = itemManager.EquipArmorImgBG.color.a,

            EquipWeaponImgsprite = itemManager.EquipWeaponImg.sprite.name,
            EquipArmorImgsprite = itemManager.EquipArmorImg.sprite.name,

            EquipWeaponEquipEffectText = itemManager.EquipWeaponEquipEffectText.text,
            EquipArmorEquipEffectText = itemManager.EquipArmorEquipEffectText.text,

            // PetManager
            pets = petManager.pets,
            NameText = petManager.NameText.text,
            GradeText = petManager.GradeText.text,
            LevelText = petManager.LevelText.text,
            CountText = petManager.CountText.text,
            isPetActive = petManager.Pet.gameObject.activeSelf,
            PetImgSprite = playerStatus.equippedPet.Name,

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
        Debug.Log(data.PetImgSprite);

    }

    // 게임 데이터를 저장하는 함수
    //public void OnApplicationQuit()
    //{
      
        
    //}

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
            UpdateStatusTexts();

            // ItemManager
            itemManager.weaponItems = data.weaponItems;
            itemManager.armorItems = data.armorItems;

            itemManager.EquipWeaponText.text = data.EquipWeaponText;
            itemManager.EquipWeaponLevelText.text = data.EquipWeaponLevelText;
            itemManager.EquipArmorText.text = data.EquipArmorText;
            itemManager.EquipArmorLevelText.text = data.EquipArmorLevelText;
            itemManager.EquipWeaponImgBG.color = new Color(
            data.EquipWeaponImgR,
            data.EquipWeaponImgG,
            data.EquipWeaponImgB,
            data.EquipWeaponImgA
            );
            itemManager.EquipArmorImgBG.color = new Color(
            data.EquipArmorImgR,
            data.EquipArmorImgG,
            data.EquipArmorImgB,
            data.EquipArmorImgA
            );

            List<Item> LoadItems = new List<Item>();
            foreach (var item in itemManager.weaponItems)
            {
                if (item.Count > 0 || item.Level >= 2) 
                {
                    LoadItems.Add(item);
                }
            }
            foreach (var item in itemManager.armorItems)
            {
                if (item.Count > 0 || item.Level >= 2) 
                {
                    LoadItems.Add(item);
                }
            }
            itemManager.UpdateItemImages(LoadItems);  // 소지 중인 아이템만 전달
            itemManager.SetTextData(data.itemtextData);
            itemManager.EquipWeaponImg.sprite = itemManager.ChangeSpriteWithWeapon(data.EquipWeaponImgsprite);
            itemManager.EquipArmorImg.sprite = itemManager.ChangeSpriteWithArmor(data.EquipArmorImgsprite);
            itemManager.EquipWeaponEquipEffectText.text = data.EquipWeaponEquipEffectText;
            itemManager.EquipArmorEquipEffectText.text = data.EquipArmorEquipEffectText;
            Debug.Log(data.EquipWeaponImgsprite);




            // PetManager
            petManager.pets = data.pets;
            petManager.GradeText.text = data.GradeText;
            petManager.LevelText.text = data.LevelText;
            petManager.CountText.text = data.CountText;
            petManager.NameText.text = data.NameText;

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
                petManager.Pet.gameObject.SetActive(true);  // 활성화
                petManager.Pet.GetComponent<SpriteRenderer>().sprite = petManager.ChangeSpriteWithPet(data.PetImgSprite);
            }
            Debug.Log(data.PetImgSprite);
            petManager.UpdatePetImages(ownedpets);

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
                //Debug.Log(gachaManager.UnLockEquipment);
                gachaManager.Unlock("장비");
            }
            gachaManager.UnLockPet = data.UnLockPet;

            if (gachaManager.UnLockPet == true)
            {
                gachaManager.Unlock("펫");
                //Debug.Log(gachaManager.UnLockPet);

            }
            gachaManager.UnLockClothes = data.UnLockClothes;

            // 스탯 창 초기화
            statusUIManager.Initialize_Texts();

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

        // GameManager
        gameManager.Initialize();
        gameManager.StopDecreaseWaveAndReset();

        // PlayerController
        playerController.Initialize();

        // PlayerStatus
        playerStatus.Initialize();

        // enemyManager
        enemyManager.InitializeStats();
        enemyManager.ResetRound();

        // enemyScript
        enemyScript.Initialize();

        // uiManager
        UpdateStatusTexts();

        // statusUIManager
        statusUIManager.Initialize_Texts();

        // ItemManager
        itemManager.Initialize();
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

        foreach (var item in resetItems)
        {
            itemManager.UpdateItemText(item.Name, resetItems);
        }

        // PetManager
        petManager.Initialize();
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

        //Debug.Log("게임 불러오기 완료");
    }

    private void UpdateStatusTexts()
    {
        uiManager.Update_Text("Attack", playerStatus.Attack, playerStatus.Attack_Level, playerStatus.Attack_Cost);
        uiManager.Update_Text("Hp", playerStatus.Hp, playerStatus.Hp_Level, playerStatus.Hp_Cost);
        uiManager.Update_Text("Hp_Recovery", playerStatus.Hp_Recovery, playerStatus.Hp_Recovery_Level, playerStatus.Hp_Recovery_Cost);
        uiManager.Update_Text("Attack_Speed", playerStatus.Attack_Speed, playerStatus.Attack_Speed_Level, (int)playerStatus.Attack_Speed_Cost);
        uiManager.Update_Text("Critical", playerStatus.Critical, playerStatus.Critical_Level, playerStatus.Critical_Cost);
        uiManager.Update_Text("Critical_Damage", playerStatus.Critical_Damage, playerStatus.Critical_Damage_Level, playerStatus.Critical_Damage_Cost);
        uiManager.Update_Text("DoubleAttack", playerStatus.DoubleAttackChance, playerStatus.DoubleAttack_Level, playerStatus.DoubleAttack_Cost);
        uiManager.Update_Text("TripleAttack", playerStatus.TripleAttackChance, playerStatus.TripleAttack_Level, playerStatus.TripleAttack_Cost);
    }
}
