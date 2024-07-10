using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public Transform[] spawnPoints; // ���� ����Ʈ �迭
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
                enemyScript.maxHP = 31 + (roundNumber - 1) * 30;
                enemyScript.HP = enemyScript.maxHP; // �ʱ� HP ����
                enemyScript.UpdateHPBar(); // HP �� ������Ʈ

                // ���� ���忡 ���� ��� ��� ����
                enemyScript.SetGoldDropAmount(roundNumber * 500);
                enemyScript.SetEnemyManager(this); // EnemyManager ����
            }

            enemyCount++; // �� ī��Ʈ ����
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        enemyCount--;

        // ���� ��� �׾��� �� ���ο� ���� ��ȯ
        if (enemyCount <= 0)
        {
            if (enemiesKilled % 12 == 0)
            {
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm != null)
                {
                    gm.IncreaseStage();
                }
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
                enemyScript.maxHP = 31 + (roundNumber - 1) * 30;
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