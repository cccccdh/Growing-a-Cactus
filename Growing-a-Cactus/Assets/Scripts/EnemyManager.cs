using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public Transform spawnPoint; // �� ���� ��ġ

    private void Start()
    {
        SpawnEnemy(); // ó���� ���� ����
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}