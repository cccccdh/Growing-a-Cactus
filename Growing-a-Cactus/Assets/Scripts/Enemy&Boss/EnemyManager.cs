using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    // Prefabs
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    // Spawn Points
    public Transform[] spawnPoints;
    public Transform bossSpawnPoint;

    // Enemy stats
    private int enemiesKilled = 0;
    private int enemyCount = 0;

    public int stageNumber = 1;
    public int roundNumber = 1;

    private int roundCount = 1;
    public int hpCalcA;
    public int hpCalcB;
    public int HpMax;
    public int befHP = 0;
    public int AttackDamage;
    public int befAtt;
    public int DropGold;
    public int befGold;

    // Boss 스탯
    public int bossMaxHP;
    public int bossAttackPower;
    public int bossGoldDropAmount;

    // 참조
    private PlayerController playerController;
    private BackgroundScript backgroundScript;
    private QuestScript questScript;

    private void Start()
    {
        InitializeStats();
        FindReferences();
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

    private void FindReferences()
    {
        playerController = FindObjectOfType<PlayerController>();
        backgroundScript = FindObjectOfType<BackgroundScript>();
        questScript = FindObjectOfType<QuestScript>();
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
            bossMaxHP = 100 + (roundNumber - 1) * 150;
            bossAttackPower = 20 + (roundNumber - 1) * 5;
            bossGoldDropAmount = 100 + ((roundNumber - 1) / 3) * 100;

            bossScript.Initialize(bossMaxHP, bossAttackPower, bossGoldDropAmount);
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        enemyCount--;

        questScript?.IncrementMonsterKillCount();

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
                int goldDropAmount = 10 + ((roundNumber - 1) / 3) * 10;
                enemyScript.SetGoldDropAmount(goldDropAmount);
                enemyScript.SetEnemyManager(this);
                enemyScript.UpdateHPBar();
            }

            enemyCount++;
        }
    }

    public int setGoldDrop(int gold)
    {
        if (gold < 100) return gold + 10;
        if (gold < 1000) return (int)(gold + gold * 0.3f);
        return (int)(gold + gold * 0.2f);
    }

    public int setEnemyAtt(int att)
    {
        int attcal = 1;
        if(roundCount > 10) // 10라운드가 지나기 전까지는 적의 공격력이 10씩 오르도록 설정
            attcal = roundCount % 10 == 0 ? 0 : roundCount % 10;
        befAtt = att + 10 * attcal;
        return befAtt;
    }

    public int SetEnemyHP(int hp)
    {
        if ((roundCount - 1) % 3 == 1) hpCalcA = hp;
        hpCalcB = roundCount % 10 == 1 ? 0 : (roundCount - 1) % 3 == 0 ? 3 : (roundCount - 1) % 3;
        if(hp < 1000000) 
            befHP = hp + hpCalcA * hpCalcB;
        else if(hp < 1000000000) // hp가 1b에서 1c 사이일때부터 hp의 증가폭을 낮춤
            befHP = hp + hpCalcA / 2 * hpCalcB;
        else
            befHP = hp + hpCalcA / 3 * hpCalcB; // hp가 1c 이상일때부터 hp의 증가폭을 더 낮추고 이후 고정
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
