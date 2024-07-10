using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // 적 프리팹
    public Transform[] spawnPoints; // 스폰 포인트 배열
    public int enemyCount = 0;

    private int stageNumber = 1;
    private int roundNumber = 1;

    private void Start()
    {
        SpawnEnemies(); // 처음에 적을 생성
    }

    public void SpawnEnemies()
    {
        // 모든 스폰 포인트에서 적을 생성
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                // 현재 라운드에 따라 적의 최대 HP 조정
                enemyScript.maxHP += (roundNumber - 1) * 30;
                enemyScript.HP = enemyScript.maxHP; // 초기 HP 설정
                enemyScript.UpdateHPBar(); // HP 바 업데이트

                // 현재 라운드에 따라 골드 드랍 설정
                enemyScript.SetGoldDropAmount((roundNumber) * 500);
            }
        }
    }

    public void stageincreaseManager()
    {
        enemyCount++;

        // 적의 수가 10마리씩 증가할 때마다 스테이지 증가
        if (enemyCount % 10 == 0 && enemyCount != 0)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.IncreaseStage();
            }
        }
    }

    public void SetStageInfo(int stage, int round)
    {
        stageNumber = stage;
        roundNumber = round;
    }
}