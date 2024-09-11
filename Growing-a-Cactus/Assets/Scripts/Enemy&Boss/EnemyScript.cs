using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public Image HpBar;

    public double HP; // 적의 초기 HP 설정 befHP
    public double maxHP; // befHP
    public float speed = 1f; // 적의 이동 속도
    public double attackPower; // 적의 공격력 AttackDamage
    public int goldDropAmount; //DropGold
    public float stopDistance = 1f; // 플레이어와 접촉했을 때의 거리

    public ParticleSystem deathEffect; // 사망 이펙트

    public bool isAttacking = false;

    private EnemyManager enemyManager; // EnemyManager 참조
    private Transform playerTransform; // 플레이어의 Transform 참조
    private Animator animator; // Animator 컴포넌트 참조

    private void OnEnable()
    {
        Init();
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾기
        animator = GetComponent<Animator>(); // Animator 컴포넌트 찾기
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

    public void Init()
    {
        if (enemyManager != null)
        {
            HP = enemyManager.HpMax;
            maxHP = enemyManager.HpMax;
            attackPower = enemyManager.AttackDamage;
            goldDropAmount = enemyManager.DropGold;
            isAttacking = false;
        }
        UpdateHPBar();
    }

    public void TakeDamage(double damage)
    {
        HP -= damage;

        UpdateHPBar();

        if (HP <= 0f)
        {
            Die();
        }
    }

    public void UpdateHPBar()
    {
        HpBar.fillAmount = (float)(HP / maxHP);
    }

    public void Die()
    {
        // HP가 0 이하일 경우 오브젝트 파괴 및 골드 증가
        if (HP <= 0)
        {
            EffectManager.instance.MoveCoin(transform.position);

            Instantiate(deathEffect, transform.position, Quaternion.identity);

            PoolManager.instance.ReturnToEnemyPool(gameObject);

            GameManager.instance.IncreaseGold(goldDropAmount);

            enemyManager?.OnEnemyKilled();
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
                animator.SetTrigger("attack"); // 공격 트리거 설정
                player.TakeDamage(attackPower);
            }
            yield return new WaitForSeconds(1f); // 1초마다 공격
        }

        isAttacking = false;
    }

}
