using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    private EnemyManager enemyManager; // EnemyManager 참조
    private Transform playerTransform; // 플레이어의 Transform 참조
    public Image HpBar;
    public TextMeshProUGUI damageTxTPrefab; // 데미지 텍스트 프리팹

    public int HP; // 적의 초기 HP 설정
    public int maxHP = 30;
    public float speed = 1f; // 적의 이동 속도
    public int attackPower = 10; // 적의 공격력
    private int goldDropAmount = 500;
    public float stopDistance = 1f; // 플레이어와 접촉했을 때의 거리

    private bool isAttacking = false;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾기
        HP = maxHP; // 초기 HP 설정
        UpdateHPBar();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > stopDistance)
        {
            // 플레이어의 x좌표 방향으로만 이동 처리
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0; // y축 값을 0으로 고정하여 y축으로의 이동을 방지
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else
        {
            if (!isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
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

            Destroy(gameObject); // 나중에 오브젝트 풀링으로 수정

            if (enemyManager != null)
            {
                enemyManager.OnEnemyKilled();
            }
        }
    }

    public void SetGoldDropAmount(int amount)
    {
        goldDropAmount = amount;
    }

    public void SetEnemyManager(EnemyManager manager)
    {
        enemyManager = manager;
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        while (Vector3.Distance(transform.position, playerTransform.position) <= stopDistance)
        {
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(attackPower);
            }
            yield return new WaitForSeconds(1f); // 2초마다 공격
        }

        isAttacking = false;
    }
}