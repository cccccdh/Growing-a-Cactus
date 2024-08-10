using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // 적 프리팹
    public GameObject bossPrefab; // 보스 프리팹
    public Transform[] spawnPoints; // 스폰 포인트 배열
    public Transform bossSpawnPoint; // 보스 스폰 포인트

    private int enemiesKilled = 0; // 죽인 적의 수를 추적
    private int enemyCount = 0; // 현재 존재하는 적의 수

    private int stageNumber = 1;
    public int roundNumber = 1;

    public int hpCalcA;
    public int hpCalcB;
    public int HpMax;
    private int befHP = 0;

    // 추가된 필드
    public int AttackDamage;
    public int DropGold;

    private PlayerController playerController;
    private BackgroundScript backgroundScript;

    private QuestScript questScript;
    private BossScript bossScript;
    private EnemyScript enemyScript;

    private void Start()
    {
        befHP = 30;
        HpMax = 30;
        AttackDamage = 10; // 적의 기본 공격력 초기화
        DropGold = 10; // 적이 드랍하는 골드 초기화

        playerController = FindObjectOfType<PlayerController>();
        backgroundScript = FindObjectOfType<BackgroundScript>();
        questScript = FindObjectOfType<QuestScript>();
        SpawnEnemies(); // 처음에 적을 생성
    }

    // 적을 스폰하는 함수
    public void SpawnEnemies()
    {
        enemyCount = 0; // 현재 존재하는 적의 수 초기화
        UpdateEnemyLevels(); // 적 레벨 업데이트
    }

    // 보스를 스폰하는 함수
    public void SpawnBoss()
    {
        GameObject bossObject = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        bossScript = bossObject.GetComponent<BossScript>();

        if (bossScript != null)
        {
            bossScript.maxHP = 100 + (roundNumber - 1) * 150; // 보스의 최대 HP 설정
            bossScript.HP = bossScript.maxHP; // 보스의 HP 설정
            bossScript.UpdateHPBar(); // HP 바 업데이트

            int goldDropAmount = 100 + ((roundNumber - 1) / 3) * 100; // 골드 드랍 설정
            bossScript.SetGoldDropAmount(goldDropAmount);
            bossScript.SetEnemyManager(this); // EnemyManager 설정
        }
    }

    // 적이 죽었을 때 호출되는 함수
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        enemyCount--;

        if (questScript != null)
        {
            questScript.IncrementMonsterKillCount(); // 몬스터 처치 수 업데이트
        }

        if (enemyCount <= 0)
        {
            GameManager.instance.IncreaseWave(25); // 웨이브 증가
            if (playerController != null)
            {
                playerController.MovePlayerWithDelay(1f); // 플레이어 이동
            }

            // BackgroundScript에서 모든 적이 죽었을 때 처리
            if (backgroundScript != null)
            {
                backgroundScript.OnKilled(); // 모든 적이 죽었을 때 처리
            }

            if (enemiesKilled % 12 == 0)
            {
                StartCoroutine(SpawnBossWithDelay(1f)); // 1초 딜레이 후 보스를 소환
            }
            else
            {
                StartCoroutine(SpawnEnemiesWithDelay(1f)); // 1초 딜레이 후 적을 소환
            }
        }
    }

    // 적을 딜레이 후 스폰하는 코루틴
    private IEnumerator SpawnEnemiesWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemies();
    }

    // 보스를 딜레이 후 스폰하는 코루틴
    private IEnumerator SpawnBossWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBoss();
    }

    // 스테이지 정보를 설정하는 함수
    public void SetStageInfo(int stage, int round)
    {
        stageNumber = stage;
        roundNumber = round;
        StartCoroutine(DelayedUpdateEnemyLevels(1f)); // 1초 후에 UpdateEnemyLevels 함수 호출
    }

    // 적 레벨을 딜레이 후 업데이트하는 코루틴
    private IEnumerator DelayedUpdateEnemyLevels(float delay)
    {
        yield return new WaitForSeconds(delay);

        HpMax = SetEnemyHP(befHP);
        UpdateEnemyLevels();
    }

    // 적의 레벨을 업데이트하는 함수
    private void UpdateEnemyLevels()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                enemyScript.UpdateHPBar(); // HP 바 업데이트

                int goldDropAmount = 10 + ((roundNumber - 1) / 3) * 10; // 골드 드랍 설정
                enemyScript.SetGoldDropAmount(goldDropAmount);
                enemyScript.SetEnemyManager(this); // EnemyManager 설정
            }
            enemyCount++;
        }
    }

    // 적의 HP를 설정하는 함수
    public int SetEnemyHP(int hp)
    {
        if ((roundNumber - 1) % 3 == 1)
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

        befHP = hp + hpCalcA * hpCalcB;
        return befHP;
    }

    // 라운드를 초기화하는 함수
    public void ResetRound()
    {
        StopAllCoroutines(); // 모든 코루틴 중지

        // 태그가 Enemy인 모든 오브젝트 제거
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // 태그가 Boss인 모든 오브젝트 제거
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject boss in bosses)
        {
            Destroy(boss);
        }

        enemyCount = 0; // 적 수 초기화
        enemiesKilled = 0;
        roundNumber = 1; // 첫 웨이브로 설정

        // GameManager 인스턴스의 ResetWave 호출
        if (GameManager.instance != null)
        {
            GameManager.instance.ResetWave();
        }

        SpawnEnemies(); // 적 다시 스폰
    }
}
