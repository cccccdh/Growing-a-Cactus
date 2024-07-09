using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public Transform spawnPoint; // �� ���� ��ġ
    public int enemyCount = 0;

    private int stageNumber = 1;
    private int roundNumber = 1;

    private void Start()
    {
        SpawnEnemy(); // ó���� ���� ����

    }

    public void SpawnEnemy()
    {
      
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();

        if (enemyScript != null)
        {
            enemyScript.maxHP += (roundNumber - 1) * 30; // ���� ���忡 ���� maxHP ����
            enemyScript.HP = enemyScript.maxHP; // �ʱ� HP ����
            enemyScript.UpdateHPBar(); // HP �� ������Ʈ

            enemyScript.SetGoldDropAmount((roundNumber) * 500); // ��� ��� ����
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