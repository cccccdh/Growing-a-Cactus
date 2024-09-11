using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public Image HpBar;

    public double HP; // ���� �ʱ� HP ���� befHP
    public double maxHP; // befHP
    public float speed = 1f; // ���� �̵� �ӵ�
    public double attackPower; // ���� ���ݷ� AttackDamage
    public int goldDropAmount; //DropGold
    public float stopDistance = 1f; // �÷��̾�� �������� ���� �Ÿ�

    public ParticleSystem deathEffect; // ��� ����Ʈ

    public bool isAttacking = false;

    private EnemyManager enemyManager; // EnemyManager ����
    private Transform playerTransform; // �÷��̾��� Transform ����
    private Animator animator; // Animator ������Ʈ ����

    private void OnEnable()
    {
        Init();
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ã��
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
        // HP�� 0 ������ ��� ������Ʈ �ı� �� ��� ����
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
                animator.SetTrigger("attack"); // ���� Ʈ���� ����
                player.TakeDamage(attackPower);
            }
            yield return new WaitForSeconds(1f); // 1�ʸ��� ����
        }

        isAttacking = false;
    }

}
