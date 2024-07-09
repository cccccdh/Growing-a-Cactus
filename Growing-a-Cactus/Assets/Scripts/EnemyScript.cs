using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private EnemyManager EnemyManager; // EnemyManager 참조
    private Transform playerTransform; // 플레이어의 Transform 참조

    public Image HpBar;
    public int HP; // 적의 초기 HP 설정
    public int maxHP = 31;

    public int enemyCount = 0;
    public int stageCount = 1;

    public float speed = 1f; // 적의 이동 속도

    private void Start()
    {
        EnemyManager = FindObjectOfType<EnemyManager>(); // EnemyManager 찾기
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾기
        HP = maxHP; // 초기 HP 설정
        UpdateHPBar();
    }

    private void Update()
    {
        // 플레이어 방향으로 이동 처리 
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        HP -= (int)damage;
        UpdateHPBar();

        Debug.Log("데미지 입힘");

        if (HP <= 0f)
        {
            Die();
        }
    }

    private void UpdateHPBar()
    {
        HpBar.fillAmount = (float)HP / maxHP;
    }

    public void Die()
    {
        // HP가 0 이하일 경우 오브젝트 파괴 및 골드 증가
        if (HP <= 0)
        {
            GoldScript goldScript = FindObjectOfType<GoldScript>();
            if (goldScript != null)
            {
                goldScript.IncreaseGold();
            }

            if (EnemyManager != null)
            {
                EnemyManager.SpawnEnemy(); // 적이 파괴될 때 새로운 적 생성 요청
            }

            Destroy(gameObject); // 나중에 오브젝트 풀링으로 수정
            enemyCount++;
        }
        if (enemyCount % 10 == 0 && enemyCount != 0)
        {
            StageScript stageScript = FindObjectOfType<StageScript>();
            if (stageScript != null)
            {
                stageScript.IncreaseStage();
            }
        }
    }
}