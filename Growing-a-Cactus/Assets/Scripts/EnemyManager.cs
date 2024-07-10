using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public GameObject bossPrefab; // ���� ������
    public Transform[] spawnPoints; // ���� ����Ʈ �迭
    public Transform bossSpawnPoint; 

    private int enemiesKilled = 0; // ���� ���� ���� ����
    private int enemyCount = 0; // ���� �����ϴ� ���� ��

    private int stageNumber = 1;
    private int roundNumber = 1;

    private void Start()
    {
        SpawnEnemies(); // ó���� ���� ����
    }

    public void SpawnEnemies()
    {
        // ���� �����ϴ� ���� �� �ʱ�ȭ
        enemyCount = 0;

        // ��� ���� ����Ʈ���� ���� ����
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                // ���� ���忡 ���� ���� �ִ� HP ����
                enemyScript.maxHP = 30 + (roundNumber - 1) * 30;
                enemyScript.HP = enemyScript.maxHP; // �ʱ� HP ����
                enemyScript.UpdateHPBar(); // HP �� ������Ʈ

                // ���� ���忡 ���� ��� ��� ����
                enemyScript.SetGoldDropAmount(roundNumber * 500);
                enemyScript.SetEnemyManager(this); // EnemyManager ����
            }

            enemyCount++; // �� ī��Ʈ ����
        }
    }

    public void SpawnBoss()
    {
        
            
            GameObject bossObject = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            BossScript bossScript = bossObject.GetComponent<BossScript>();

            if (bossScript != null)
            {
                // ���� ���忡 ���� ���� �ִ� HP ����
                bossScript.maxHP = 100 + (roundNumber - 1) * 150;
                bossScript.HP = bossScript.maxHP; // �ʱ� HP ����
                bossScript.UpdateHPBar(); // HP �� ������Ʈ

                // ���� ���忡 ���� ��� ��� ����
                bossScript.SetGoldDropAmount(roundNumber * 5000);
                bossScript.SetEnemyManager(this); // EnemyManager ����
            }

     
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        enemyCount--;

        // ���� ��� �׾��� �� ���ο� ���� ��ȯ�ϰų� ������ ��ȯ
        if (enemyCount <= 0)
        {
            if (enemiesKilled % 12 == 0)
            {
                SpawnBoss();
            }
            else
            {
                SpawnEnemies();
            }
        }
    }

    public void SetStageInfo(int stage, int round)
    {
        stageNumber = stage;
        roundNumber = round;

        // ���� ���� �� �� ���� ������Ʈ
        UpdateEnemyLevels();
    }

    private void UpdateEnemyLevels()
    {
        // ���� ���忡 ���� ���� ���� ������Ʈ
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                // ���� ���忡 ���� ���� �ִ� HP ����
                enemyScript.maxHP = 30 + (roundNumber - 1) * 30;
                enemyScript.HP = enemyScript.maxHP; // �ʱ� HP ����
                enemyScript.UpdateHPBar(); // HP �� ������Ʈ

                // ���� ���忡 ���� ��� ��� ����
                enemyScript.SetGoldDropAmount(roundNumber * 500);
                enemyScript.SetEnemyManager(this); // EnemyManager ����
            }

            enemyCount++;
        }
    }
}