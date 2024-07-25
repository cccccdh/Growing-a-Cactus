using UnityEngine;
using System.Collections;

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

    private int hpCalcA;
    private int hpCalcB;

    private PlayerController playerController;
    private BackgroundScript backgroundScript;
    private QuestScript questScript; // �߰�: QuestScript ����
    private BossScript bossScript;
    private EnemyScript enemyScript;

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        backgroundScript = GameObject.FindObjectOfType<BackgroundScript>();
        questScript = GameObject.FindObjectOfType<QuestScript>(); // �߰�: QuestScript ã��
        SpawnEnemies(); // ó���� ���� ����
    }

    public void SpawnEnemies()
    {
        // ���� �����ϴ� ���� �� �ʱ�ȭ
        enemyCount = 0;

        UpdateEnemyLevels();
    }

    public void SpawnBoss()
    {
        GameObject bossObject = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        bossScript = bossObject.GetComponent<BossScript>();

        if (bossScript != null)
        {
            // ���� ���忡 ���� ������ �ִ� HP ����
            bossScript.maxHP = 100 + (roundNumber - 1) * 150;
            bossScript.HP = bossScript.maxHP; // �ʱ� HP ����
            bossScript.UpdateHPBar(); // HP �� ������Ʈ

            // ���� ���忡 ���� ��� ��� ����
            int goldDropAmount = 100 + ((roundNumber - 1) / 3) * 100;
            bossScript.SetGoldDropAmount(goldDropAmount);
            bossScript.SetEnemyManager(this); // EnemyManager ����
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        enemyCount--;

        if (questScript != null)
        {
            questScript.IncrementMonsterKillCount(); // �߰�: ���� óġ �� ������Ʈ
        }

        if (enemyCount <= 0)
        {
            GameManager.instance.IncreaseWave(25); // ���̺� ����
            if (playerController != null)
            {
                playerController.MovePlayerWithDelay(1f); // �÷��̾� �̵�
            }

            if (backgroundScript != null)
            {
                backgroundScript.StartMoveBg(); // ��� �̵� ����
            }

            if (enemiesKilled % 12 == 0)
            {
                StartCoroutine(SpawnBossWithDelay(1f)); // 1�� ������ �� ������ ��ȯ
            }
            else
            {
                StartCoroutine(SpawnEnemiesWithDelay(1f)); // 1�� ������ �� ���� ��ȯ
            }
        }
    }

    private IEnumerator SpawnEnemiesWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemies();
    }

    private IEnumerator SpawnBossWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBoss();
    }

    public void SetStageInfo(int stage, int round)
    {
        stageNumber = stage;
        roundNumber = round;

        // ���� ���� �� �� ���� ������Ʈ
        StartCoroutine(DelayedUpdateEnemyLevels(1f)); // 1�� �Ŀ� UpdateEnemyLevels �Լ� ȣ��
    }

    private IEnumerator DelayedUpdateEnemyLevels(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateEnemyLevels();
    }

    private void UpdateEnemyLevels()
    {
        // ���� ���忡 ���� ���� ���� ������Ʈ
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                // ���� ���忡 ���� ���� �ִ� HP ����
                enemyScript.maxHP = SetEnemyHP(enemyScript.maxHP);
                enemyScript.HP = enemyScript.maxHP; // �ʱ� HP ����
                enemyScript.UpdateHPBar(); // HP �� ������Ʈ

                // ���� ���忡 ���� ��� ��� ����
                int goldDropAmount = 10 + ((roundNumber - 1) / 3) * 10;
                enemyScript.SetGoldDropAmount(goldDropAmount);
                enemyScript.SetEnemyManager(this); // EnemyManager ����
            }
            enemyCount++;
        }
    }

    public int SetEnemyHP(int hp)
    {
        int A;
        if((roundNumber - 1) % 3 == 1)
        {
            hpCalcA = hp;
        }

        if (roundNumber % 10 == 1)
        {
            hpCalcB = 0;
        }
        else
        {
            if ((roundNumber - 1) % 3 == 0)
            {
                hpCalcB = 3;
            }
            else
            {
                hpCalcB = (roundNumber - 1) % 3;
            }
        }

        Debug.Log($"{hp} / {hpCalcA} / {hpCalcB}");
        A = hp + hpCalcA * hpCalcB;

        return A;
    }    
}