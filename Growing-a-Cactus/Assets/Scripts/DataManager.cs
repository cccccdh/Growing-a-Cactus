using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyData
    {
        public int HP;
        public int maxHP;
        public float speed;
        public int attackPower;
        public int goldDropAmount;
        public Vector3 position; // ���� ��ġ�� ����
    }

    [System.Serializable]
    public class GameData
    {
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

        public List<EnemyData> enemies; // ���� �����͸� ������ ����Ʈ �߰�

        // GameManager ���� ������ �߰�
        public int gold;
        public int stageNumber;
        public int roundNumber;
    }

    private string saveFilePath;
    private PlayerStatus playerStatus;
    private UIManager uiManager;
    private GameManager gameManager; // GameManager ���� �߰�

    public GameObject enemyPrefab; // ���� �������� �����ϱ� ���� ���� �߰�

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        playerStatus = FindObjectOfType<PlayerStatus>();
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>(); // GameManager ã��
    }

    // ���� ������ ����
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
            enemies = new List<EnemyData>(),

            // GameManager ���� ������ ����
            gold = gameManager.Gold,
            stageNumber = gameManager.stageNumber,
            roundNumber = gameManager.roundNumber
        };

        // ��� ���� �����͸� ����
        foreach (var enemy in FindObjectsOfType<EnemyScript>())
        {
            data.enemies.Add(enemy.GetEnemyData());
        }

        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, jsonData);
        Debug.Log("Game Saved: " + saveFilePath);
    }

    // ���� ������ �ҷ�����
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

            // UI ������Ʈ
            uiManager.Update_Text("Attack", playerStatus.Attack, playerStatus.Attack_Level, playerStatus.Attack_Cost);
            uiManager.Update_Text("Hp", playerStatus.Hp, playerStatus.Hp_Level, playerStatus.Hp_Cost);
            uiManager.Update_Text("Hp_Recovery", playerStatus.Hp_Recovery, playerStatus.Hp_Recovery_Level, playerStatus.Hp_Recovery_Cost);
            uiManager.Update_Text("Attack_Speed", playerStatus.Attack_Speed, playerStatus.Attack_Speed_Level, playerStatus.Attack_Speed_Cost);
            uiManager.Update_Text("Critical", playerStatus.Critical, playerStatus.Critical_Level, playerStatus.Critical_Cost);
            uiManager.Update_Text("Critical_Damage", playerStatus.Critical_Damage, playerStatus.Critical_Damage_Level, playerStatus.Critical_Damage_Cost);

            // GameManager ������ ����
            if (gameManager != null)
            {
                gameManager.IncreaseGold(data.gold - gameManager.Gold);
                gameManager.stageNumber = data.stageNumber;
                gameManager.roundNumber = data.roundNumber;
                gameManager.UpdateStageText(); // stageText�� ������Ʈ
            }

            // ���� ������ �ҷ�����
            foreach (var enemyData in data.enemies)
            {
                // ���� �����ϰ� �����͸� ����
                GameObject enemyObject = Instantiate(enemyPrefab, enemyData.position, Quaternion.identity);
                EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();
                enemyScript.SetEnemyData(enemyData);
            }

            Debug.Log("Game Loaded: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("Save file not found");
        }
    }
}
