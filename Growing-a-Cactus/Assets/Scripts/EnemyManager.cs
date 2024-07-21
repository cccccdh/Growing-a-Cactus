using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // 적 프리팹
    public GameObject bossPrefab; // 보스 프리팹
    public Transform[] spawnPoints; // 스폰 포인트 배열
    public Transform bossSpawnPoint;

    private int enemiesKilled = 0; // 죽인 적의 수를 추적
    private int enemyCount = 0; // 현재 존재하는 적의 수

    private int stageNumber = 1;
    private int roundNumber = 1;

    private PlayerController playerController;
    private BackgroundScript backgroundScript; // BackgroundScript 참조 추가

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        backgroundScript = GameObject.FindObjectOfType<BackgroundScript>(); // BackgroundScript 찾기
        SpawnEnemies(); // 처음에 적을 생성
    }

    public void SpawnEnemies()
    {
        // 현재 존재하는 적의 수 초기화
        enemyCount = 0;

        // 모든 스폰 포인트에서 적을 생성
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                // 현재 라운드에 따라 적의 최대 HP 조정
                enemyScript.maxHP = 30 + (roundNumber - 1) * 30;
                enemyScript.HP = enemyScript.maxHP; // 초기 HP 설정
                enemyScript.UpdateHPBar(); // HP 바 업데이트

                // 현재 라운드에 따라 골드 드랍 설정
                enemyScript.SetGoldDropAmount(roundNumber * 500);
                enemyScript.SetEnemyManager(this); // EnemyManager 설정
            }

            enemyCount++; // 적 카운트 증가
        }
    }

    public void SpawnBoss()
    {
        GameObject bossObject = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        BossScript bossScript = bossObject.GetComponent<BossScript>();

        if (bossScript != null)
        {
            // 현재 라운드에 따라 보스의 최대 HP 조정
            bossScript.maxHP = 100 + (roundNumber - 1) * 150;
            bossScript.HP = bossScript.maxHP; // 초기 HP 설정
            bossScript.UpdateHPBar(); // HP 바 업데이트

            // 현재 라운드에 따라 골드 드랍 설정
            bossScript.SetGoldDropAmount(roundNumber * 5000);
            bossScript.SetEnemyManager(this); // EnemyManager 설정
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        enemyCount--;

        if (enemyCount <= 0)
        {
            if (enemiesKilled % 12 == 0)
            {
                GameManager.instance.IncreaseWave(25); // 웨이브 증가
                if (playerController != null)
                    playerController.MovePlayerWithDelay(1f); // 플레이어 이동

                if (backgroundScript != null)
                    backgroundScript.MoveBackgroundForDuration(1f); // 배경 1초 이동

                StartCoroutine(SpawnBossWithDelay(1f)); // 1초 딜레이 후 보스를 소환
            }
            else
            {
                GameManager.instance.IncreaseWave(25); // 웨이브 증가
                if (playerController != null)
                    playerController.MovePlayerWithDelay(1f); // 플레이어 이동

                if (backgroundScript != null)
                    backgroundScript.MoveBackgroundForDuration(1f); // 배경 1초 이동

                StartCoroutine(SpawnEnemiesWithDelay(1f)); // 1초 딜레이 후 적을 소환
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

        // 라운드 변경 시 적 레벨 업데이트
        StartCoroutine(DelayedUpdateEnemyLevels(1f)); // 1초 후에 UpdateEnemyLevels 함수 호출
    }

    private IEnumerator DelayedUpdateEnemyLevels(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateEnemyLevels();
    }

    private void UpdateEnemyLevels()
    {
        // 현재 라운드에 따라 적의 레벨 업데이트
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                // 현재 라운드에 따라 적의 최대 HP 조정
                enemyScript.maxHP = 30 + (roundNumber - 1) * 30;
                enemyScript.HP = enemyScript.maxHP; // 초기 HP 설정
                enemyScript.UpdateHPBar(); // HP 바 업데이트

                // 현재 라운드에 따라 골드 드랍 설정
                enemyScript.SetGoldDropAmount(roundNumber * 500);
                enemyScript.SetEnemyManager(this); // EnemyManager 설정
            }

            enemyCount++;
        }
    }
}