using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour
{
    private EnemyManager enemyManager; // EnemyManager ����
    private Transform playerTransform; // �÷��̾��� Transform ����
    public Image HpBar;
    public TextMeshProUGUI damageTxTPrefab; // ������ �ؽ�Ʈ ������

    public int HP; // ���� �ʱ� HP ����
    public int maxHP = 100;
    public float speed = 0.6f; // ���� �̵� �ӵ�
    private int goldDropAmount = 1500;
    public int attackPower = 20; // ���� ���ݷ�
    public float stopDistance = 1.5f; // �÷��̾�� �������� ���� �Ÿ�

    private bool isAttacking = false;
    private Animator animator; // Animator ������Ʈ ����

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ã��
        HP = maxHP; // �ʱ� HP ����
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
            Destroy(gameObject); // ���߿� ������Ʈ Ǯ������ ����

            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.MovePlayerWithDelay(1f);
            }

            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.IncreaseGold(goldDropAmount);
                gm.IncreaseStage();
                gm.ResetWave(); // ���̺� ���� 0���� ����
                Debug.Log("���� ����");
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
            yield return new WaitForSeconds(1f); // 2�ʸ��� ����
        }

        isAttacking = false;
    }
}