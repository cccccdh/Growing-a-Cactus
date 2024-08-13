using UnityEngine;
using System.Collections;
using System;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public GameObject bossPrefab; // ���� ������
    public Transform[] spawnPoints; // ���� ����Ʈ �迭
    public Transform bossSpawnPoint; // ���� ���� ����Ʈ

    private int enemiesKilled = 0; // ���� ���� ���� ����
    private int enemyCount = 0; // ���� �����ϴ� ���� ��

    private int stageNumber = 1;
    public int roundNumber = 1;

    private int roundCount = 1;
    public int hpCalcA;
    public int hpCalcB;
    public int HpMax;
    private int befHP = 0;

    public int AttackDamage;
    private int befAtt;

    public int DropGold;
    private int befGold;

    private PlayerController playerController;
    private BackgroundScript backgroundScript;

    private QuestScript questScript;
    private BossScript bossScript;
    private EnemyScript enemyScript;

    private void Start()
    {
        
        befHP = 30;
        befAtt = 0;
        befGold = 10;
        HpMax = SetEnemyHP(befHP);
        AttackDamage = setEnemyAtt(befAtt);
        DropGold = setGoldDrop(befGold);
        AttackDamage = 10; // ���� �⺻ ���ݷ� �ʱ�ȭ

        playerController = FindObjectOfType<PlayerController>();
        backgroundScript = FindObjectOfType<BackgroundScript>();
        questScript = FindObjectOfType<QuestScript>();
        SpawnEnemies(); // ó���� ���� ����
    }

    // ���� �����ϴ� �Լ�
    public void SpawnEnemies()
    {
        enemyCount = 0; // ���� �����ϴ� ���� �� �ʱ�ȭ
        UpdateEnemyLevels(); // �� ���� ������Ʈ
    }

    // ������ �����ϴ� �Լ�
    public void SpawnBoss()
    {
        GameObject bossObject = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        bossScript = bossObject.GetComponent<BossScript>();

        if (bossScript != null)
        {
            bossScript.maxHP = 100 + (roundNumber - 1) * 150; // ������ �ִ� HP ����
            bossScript.HP = bossScript.maxHP; // ������ HP ����
            bossScript.UpdateHPBar(); // HP �� ������Ʈ

            int goldDropAmount = 100 + ((roundNumber - 1) / 3) * 100; // ��� ��� ����
            bossScript.SetGoldDropAmount(goldDropAmount);
            bossScript.SetEnemyManager(this); // EnemyManager ����
        }
    }

    // ���� �׾��� �� ȣ��Ǵ� �Լ�
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        enemyCount--;

        if (questScript != null)
        {
            questScript.IncrementMonsterKillCount(); // ���� óġ �� ������Ʈ
        }

        if (enemyCount <= 0)
        {
            GameManager.instance.IncreaseWave(25); // ���̺� ����
            if (playerController != null)
            {
                playerController.MovePlayerWithDelay(1f); // �÷��̾� �̵�
            }

            // BackgroundScript���� ��� ���� �׾��� �� ó��
            if (backgroundScript != null)
            {
                backgroundScript.OnKilled(); // ��� ���� �׾��� �� ó��
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

    // ���� ������ �� �����ϴ� �ڷ�ƾ
    private IEnumerator SpawnEnemiesWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemies();
    }

    // ������ ������ �� �����ϴ� �ڷ�ƾ
    private IEnumerator SpawnBossWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBoss();
    }

    // �������� ������ �����ϴ� �Լ�
    public void SetStageInfo(int stage, int round)
    {
        stageNumber = stage;
        roundNumber = round;
        StartCoroutine(DelayedUpdateEnemyLevels(1f)); // 1�� �Ŀ� UpdateEnemyLevels �Լ� ȣ��
    }

    // �� ������ ������ �� ������Ʈ�ϴ� �ڷ�ƾ
    private IEnumerator DelayedUpdateEnemyLevels(float delay)
    {
        yield return new WaitForSeconds(delay);

        roundCount++;
        HpMax = SetEnemyHP(befHP); 
        AttackDamage = setEnemyAtt(befAtt);
        DropGold = setGoldDrop(befGold);
        UpdateEnemyLevels();
    }

    // ���� ������ ������Ʈ�ϴ� �Լ�
    private void UpdateEnemyLevels()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                enemyScript.UpdateHPBar(); // HP �� ������Ʈ

                int goldDropAmount = 10 + ((roundNumber - 1) / 3) * 10; // ��� ��� ����
                enemyScript.SetGoldDropAmount(goldDropAmount);
                enemyScript.SetEnemyManager(this); // EnemyManager ����
            }
            enemyCount++;
        }
    }
    public int setGoldDrop(int gold)
    {
        if (gold < 100) befGold = gold + 10;
        else if (gold < 1000)
            befGold = (int)((float)gold + ((float)gold * 0.3));
        else if (gold < 5000)
            befGold = (int)((float)gold + ((float)gold * 0.2));
        else
            befGold = (int)((float)gold + ((float)gold * 0.1));
        return befGold;
    }

    //���ݷ� ���� �Լ�
    public int setEnemyAtt(int att)
    {
        int attcal;
        if (roundCount % 10 == 0)
        {
            attcal = 0;
        }
        else attcal = roundCount % 10;

        befAtt = att + 10 * attcal;

        return befAtt;
    }

    // ���� HP�� �����ϴ� �Լ�
    public int SetEnemyHP(int hp)
    {
        if ((roundCount - 1) % 3 == 1)
        {
            hpCalcA = hp;
        }

        if (roundCount % 10 == 1)
        {
            hpCalcB = 0;
        }
        else
        {
            if ((roundCount - 1) % 3 == 0)
            {
                hpCalcB = 3;
            }
            else
            {
                hpCalcB = (roundCount - 1) % 3;
            }
        }

        befHP = hp + hpCalcA * hpCalcB;
        return befHP;
    }

    // ���带 �ʱ�ȭ�ϴ� �Լ�
    public void ResetRound()
    {
        StopAllCoroutines(); // ��� �ڷ�ƾ ����

        // �±װ� Enemy�� ��� ������Ʈ ����
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // �±װ� Boss�� ��� ������Ʈ ����
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject boss in bosses)
        {
            Destroy(boss);
        }

        enemyCount = 0; // �� �� �ʱ�ȭ
        enemiesKilled = 0;
        roundNumber = 1; // ù ���̺�� ����

        // GameManager �ν��Ͻ��� ResetWave ȣ��
        if (GameManager.instance != null)
        {
            GameManager.instance.ResetWave();
        }

        SpawnEnemies(); // �� �ٽ� ����
    }
}
