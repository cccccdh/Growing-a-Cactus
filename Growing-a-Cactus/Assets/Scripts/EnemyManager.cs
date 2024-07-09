using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public Transform spawnPoint; // �� ���� ��ġ
    public int enemyCount = 0;

    private void Start()
    {
        SpawnEnemy(); // ó���� ���� ����
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