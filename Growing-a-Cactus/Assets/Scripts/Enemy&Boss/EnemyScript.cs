using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public Image HpBar;

    public int HP; // ���� �ʱ� HP ����
    public int maxHP;
    public float speed = 1f; // ���� �̵� �ӵ�
    public int attackPower; // ���� ���ݷ�
    public int goldDropAmount;
    public float stopDistance = 1f; // �÷��̾�� �������� ���� �Ÿ�

    public ParticleSystem deathEffect; // ��� ����Ʈ

    private bool isAttacking = false;

    private EnemyManager enemyManager; // EnemyManager ����
    private Transform playerTransform; // �÷��̾��� Transform ����
    private Animator animator; // Animator ������Ʈ ����
    private PoolManager poolManager;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ã��
        poolManager = PoolManager.Instance;

        if (enemyManager != null)
        {
            HP = enemyManager.HpMax;
            maxHP = enemyManager.HpMax;
            attackPower = enemyManager.AttackDamage;
            goldDropAmount = enemyManager.DropGold;
        }
        UpdateHPBar();
        animator = GetComponent<Animator>(); // Animator ������Ʈ ã��
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > stopDistance)
        {
            // �÷��̾��� x��ǥ �������θ� �̵� ó��
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0; // y�� ���� 0���� �����Ͽ� y�������� �̵��� ����
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
        // HP�� 0 ������ ��� ������Ʈ �ı� �� ��� ����
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
                animator.SetTrigger("attack"); // ���� Ʈ���� ����
                player.TakeDamage(attackPower);
            }
            yield return new WaitForSeconds(1f); // 1�ʸ��� ����
        }

        isAttacking = false;
    }

    // ���� ���¸� ��ȯ�ϴ� �޼��� �߰�
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

    // ���� ���¸� �����ϴ� �޼��� �߰�
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
