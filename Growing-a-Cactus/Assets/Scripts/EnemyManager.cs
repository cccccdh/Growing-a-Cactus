using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // 利 橇府普
    public Transform spawnPoint; // 利 积己 困摹
    public int enemyCount = 0;

    private void Start()
    {
        SpawnEnemy(); // 贸澜俊 利阑 积己
        stageincreaseManager();
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
  
    public void stageincreaseManager()
    {
        enemyCount++;

    if (enemyCount % 11 == 0 && enemyCount != 0)
        {
            StageScript stageScript = FindObjectOfType<StageScript>();
            if (stageScript != null)
            {
               stageScript.IncreaseStage();
            }
        }
    }
}