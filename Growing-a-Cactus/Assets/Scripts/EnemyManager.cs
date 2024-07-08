using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // 利 橇府普
    public Transform spawnPoint; // 利 积己 困摹


    

    private void Start()
    {
        SpawnEnemy(); // 贸澜俊 利阑 积己
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}