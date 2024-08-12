using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public Image HpBar;

    public int HP; // 적의 초기 HP 설정
    public int maxHP;
    public float speed = 1f; // 적의 이동 속도
    public int attackPower; // 적의 공격력
    public int goldDropAmount;
    public float stopDistance = 1f; // 플레이어와 접촉했을 때의 거리

    public ParticleSystem deathEffect; // 사망 이펙트

    private bool isAttacking = false;

    private EnemyManager enemyManager; // EnemyManager 참조
    private Transform playerTransform; // 플레이어의 Transform 참조
    private Animator animator; // Animator 컴포넌트 참조
    private PoolManager poolManager;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾기
        poolManager = PoolManager.Instance;

        if (enemyManager != null)
        {
            HP = enemyManager.HpMax;
            maxHP = enemyManager.HpMax;
            attackPower = enemyManager.AttackDamage;
            goldDropAmount = enemyManager.DropGold;
        }
        UpdateHPBar();
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
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);

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
                animator.SetTrigger("attack"); // 공격 트리거 설정
                player.TakeDamage(attackPower);
            }
            yield return new WaitForSeconds(1f); // 1초마다 공격
        }

        isAttacking = false;
    }

    // 적의 상태를 반환하는 메서드 추가
    public DataManager.EnemyData GetEnemyData()
    {
        return new DataManager.EnemyData
        {
            HP = this.HP,
            maxHP = this.maxHP,
            speed = this.speed,
            attackPower = this.attackPower,
            goldDropAmount = this.goldDropAmount,
        };
    }

    // 적의 상태를 설정하는 메서드 추가
    public void SetEnemyData(DataManager.EnemyData data)
    {
        this.HP = data.HP;
        this.maxHP = data.maxHP;
        this.speed = data.speed;
        this.attackPower = data.attackPower;
        this.goldDropAmount = data.goldDropAmount;

        UpdateHPBar();
    }
}
