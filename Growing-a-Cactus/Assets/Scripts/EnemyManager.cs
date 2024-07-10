using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public Transform[] spawnPoints; // ���� ����Ʈ �迭
    public int enemyCount = 0;

    private int stageNumber = 1;
    private int roundNumber = 1;

    private void Start()
    {
        SpawnEnemies(); // ó���� ���� ����
    }

    public void SpawnEnemies()
    {
        // ��� ���� ����Ʈ���� ���� ����
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyScript enemyScript = enemyObject.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                // ���� ���忡 ���� ���� �ִ� HP ����
                enemyScript.maxHP += (roundNumber - 1) * 30;
                enemyScript.HP = enemyScript.maxHP; // �ʱ� HP ����
                enemyScript.UpdateHPBar(); // HP �� ������Ʈ

                // ���� ���忡 ���� ��� ��� ����
                enemyScript.SetGoldDropAmount((roundNumber) * 500);
            }
        }
    }

    public void stageincreaseManager()
    {
        enemyCount++;

        // ���� ���� 10������ ������ ������ �������� ����
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