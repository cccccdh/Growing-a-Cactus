using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // 적 프리팹
    public Transform spawnPoint; // 적 생성 위치
    public int enemyCount = 0;

    private int stageNumber = 1;
    private int roundNumber = 1;

    private void Start()
    {
        SpawnEnemy(); // 처음에 적을 생성

    }

    public void SpawnEnemy()
    {
      
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();

        if (enemyScript != null)
        {
            enemyScript.maxHP += (roundNumber - 1) * 30; // 현재 라운드에 따라 maxHP 증가
            enemyScript.HP = enemyScript.maxHP; // 초기 HP 설정
            enemyScript.UpdateHPBar(); // HP 바 업데이트

            enemyScript.SetGoldDropAmount((roundNumber) * 500); // 드랍 골드 설정
        }
    }

    public void stageincreaseManager()
    {
        enemyCount++;        

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