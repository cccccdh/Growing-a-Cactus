using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    private EnemyManager enemyManager; // EnemyManager ����
    private Transform playerTransform; // �÷��̾��� Transform ����
    public Image HpBar;
    public TextMeshProUGUI damageTxTPrefab; // ������ �ؽ�Ʈ ������

    public int HP; // ���� �ʱ� HP ����
    public int maxHP;
    public float speed = 1f; // ���� �̵� �ӵ�
    public int attackPower = 10; // ���� ���ݷ�
    private int goldDropAmount = 10;
    public float stopDistance = 1f; // �÷��̾�� �������� ���� �Ÿ�

    private bool isAttacking = false;

    private Animator animator; // Animator ������Ʈ ����

    private void Start()
    {
        HP = enemyManager.HpMax;
        maxHP = enemyManager.HpMax;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ã��
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

            Destroy(gameObject); // ���߿� ������Ʈ Ǯ������ ����

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