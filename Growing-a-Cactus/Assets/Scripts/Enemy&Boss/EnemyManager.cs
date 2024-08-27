using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    [Header("������")]
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    [Header("��ȯ ��ġ")]
    public Transform[] spawnPoints;
    public Transform bossSpawnPoint;

    [Header("�� ����")]
    private int enemiesKilled = 0;
    private int enemyCount = 0;

    [Header("��������")]
    public int stageNumber = 1;
    public int roundNumber = 1;

    private int roundCount = 1;

    [Header("�� ����")]
    public double hpCalcA;
    public double hpCalcB;
    public double HpMax;
    public double befHP = 0;
    public double AttackDamage;
    public double befAtt;
    public int DropGold;
    public int befGold;

    [Header("���� ����")]
    public double bossMaxHP;
    public double bossAttackPower;
    public int bossGoldDropAmount;

    [Header("��ũ��Ʈ ����")]
    public PlayerController playerController;
    public BackgroundScript backgroundScript;
    public QuestManager questmanager;

    private void Start()
    {
        InitializeStats();
        SpawnEnemies();
    }

    private void InitializeStats()
    {
        befHP = 30;
        befAtt = 0;
        befGold = 10;
        HpMax = SetEnemyHP(befHP);
        AttackDamage = setEnemyAtt(befAtt);
        DropGold = setGoldDrop(befGold);
    }

    public void SpawnEnemies()
    {
        enemyCount = 0;
        UpdateEnemyLevels();
    }

    public void SpawnBoss()
    {
        //GameObject bossObject = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        GameObject bossObject = PoolManager.Instance.GetBoss(bossSpawnPoint.position);
        BossScript bossScript = bossObject.GetComponent<BossScript>();

        if (bossScript != null)
        {
            bossMaxHP = befHP * 15;
            if(roundCount <= 10)
                bossAttackPower = befAtt * 3;
            else if(roundCount <= 30)
                bossAttackPower = befAtt * 20;
            else
                bossAttackPower = befAtt * 40;
            bossGoldDropAmount = befGold * 5;

            bossScript.Initialize(bossMaxHP, bossAttackPower, bossGoldDropAmount);
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        enemyCount--;

        QuestManager.instance.UpdateQuestProgress(1, "�� óġ");

        if (enemyCount <= 0)
        {
            GameManager.instance.IncreaseWave(25);
            playerController?.MovePlayerWithDelay(1f);
            backgroundScript?.OnKilled();

            if (enemiesKilled % 12 == 0)
            {
                StartCoroutine(SpawnBossWithDelay(1f));
            }
            else
            {
                StartCoroutine(SpawnEnemiesWithDelay(1f));
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
        StartCoroutine(DelayedUpdateEnemyLevels(1f));
    }

    private IEnumerator DelayedUpdateEnemyLevels(float delay)
    {
        yield return new WaitForSeconds(delay);

        roundCount++;
        HpMax = SetEnemyHP(befHP);
        AttackDamage = setEnemyAtt(befAtt);
        DropGold = setGoldDrop(befGold);
        UpdateEnemyLevels();
    }

    private void UpdateEnemyLevels()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = PoolManager.instance.GetEnemy(spawnPoint.position);
            EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                enemyScript.SetGoldDropAmount(DropGold);
                enemyScript.SetEnemyManager(this);
                enemyScript.UpdateHPBar();
            }

            enemyCount++;
        }
    }

    public int setGoldDrop(int gold)
    {
        if (gold < 100) 
            return befGold = gold + 10;
        else if (gold < 1000) 
            return befGold = (int)(gold + gold * 0.3f);
        else
            return befGold = (int)(gold + gold * 0.2f);
    }

    public double setEnemyAtt(double att)
    {
        int attcal = 1;
        if(roundCount > 10) // 10���尡 ������ �������� ���� ���ݷ��� 10�� �������� ����
            attcal = roundCount % 10 == 0 ? 0 : roundCount % 10;
        befAtt = att + 10 * attcal;
        return befAtt;
    }

    public double SetEnemyHP(double hp)
    {
        if ((roundCount - 1) % 3 == 1) hpCalcA = hp;
        hpCalcB = roundCount % 10 == 1 ? 0 : (roundCount - 1) % 3 == 0 ? 3 : (roundCount - 1) % 3;
        if(hp < 1000000) 
            befHP = hp + hpCalcA * hpCalcB;
        else if(hp < 1000000000) // hp�� 1b���� 1c �����϶����� hp�� �������� ����
            befHP = hp + hpCalcA / 2 * hpCalcB;
        else
            befHP = hp + hpCalcA / 3 * hpCalcB; // hp�� 1c �̻��϶����� hp�� �������� �� ���߰� ���� ����
        return befHP;
    }

    public void ResetRound()
    {
        StopAllCoroutines();
        DestroyAllEnemiesAndBosses();
        enemyCount = 0;
        enemiesKilled = 0;
        roundNumber = 1;

        GameManager.instance?.ResetWave();
        SpawnEnemies();
    }

    private void DestroyAllEnemiesAndBosses()
    {
        DestroyObjectsWithTag("Enemy");
        DestroyObjectsWithTag("Boss");
    }

    private void DestroyObjectsWithTag(string tag)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            Destroy(obj);
        }
    }
}
