using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private EnemyManager EnemyManager; // EnemyManager 참조
    private Transform playerTransform; // 플레이어의 Transform 참조
    public Image HpBar;
    public TextMeshProUGUI damageTxTPrefab; // 데미지 텍스트 프리팹

    public int HP; // 적의 초기 HP 설정
    public int maxHP = 31;
    public float speed = 1f; // 적의 이동 속도
    private int goldDropAmount = 500;

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

        if (HP <= 0f)
        {
            Die();
        }
    }

    public void UpdateHPBar()
    {
        HpBar.fillAmount = (float)HP / maxHP;
    }

    public void Die()
    {
        // HP가 0 이하일 경우 오브젝트 파괴 및 골드 증가
        if (HP <= 0)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.IncreaseGold(goldDropAmount);
            }

            if (EnemyManager != null)
            {
                EnemyManager.SpawnEnemy(); // 적이 파괴될 때 새로운 적 생성 요청
                EnemyManager.stageincreaseManager();
            }

            Destroy(gameObject); // 나중에 오브젝트 풀링으로 수정
        }
    }

    public void SetGoldDropAmount(int amount)
    {
        goldDropAmount = amount;
    }
}