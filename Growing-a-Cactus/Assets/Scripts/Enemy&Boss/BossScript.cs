using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour
{
    // UI �� ����Ʈ ���� ����
    public Image HpBar;
    public ParticleSystem deathEffect;

    // ������ ���� ����
    private int HP; // ���� HP
    private int maxHP; // �ִ� HP
    private int attackPower; // ���ݷ�
    private int goldDropAmount; // ����ϴ� ��� ��

    public float speed = 0.6f; // �̵� �ӵ�
    public float stopDistance = 1.5f; // �÷��̾���� ���� �Ÿ�

    private bool isAttacking = false; // ���� �� ����
    private EnemyManager enemyManager; // EnemyManager ����
    private QuestScript questScript; // QuestScript ����
    private Transform playerTransform; // �÷��̾��� Transform
    private Animator animator; // Animator ������Ʈ
    private BackgroundScript backgroundScript; // BackgroundScript ����

    private void Start()
    {
        // �÷��̾�, Animator, QuestScript, BackgroundScript �ʱ�ȭ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        questScript = GameObject.FindObjectOfType<QuestScript>();
        backgroundScript = FindObjectOfType<BackgroundScript>();
        StartCoroutine(BossTimer(10f)); // ���� Ÿ�̸� ����
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // �÷��̾ ���� �Ÿ� �̻� ������ ������ �̵�
        if (distanceToPlayer > stopDistance)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0; // y�� �̵� ����
            transform.Translate(direction * speed * Time.deltaTime);
        }
        // �÷��̾�� ��������� ���� ����
        else if (!isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    public void TakeDamage(float damage)
    {
        // ���� ó�� �� HP �� ������Ʈ
        HP -= (int)damage;
        UpdateHPBar();

        // HP�� 0 ������ �� ��� ó��
        if (HP <= 0)
        {
            Die();
        }
    }

    public void UpdateHPBar()
    {
        // HP �� ������Ʈ
        HpBar.fillAmount = (float)HP / maxHP;
    }

    public void Die()
    {
        // ���� ��� ó��
        if (HP <= 0)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.MovePlayerWithDelay(1f); // �÷��̾� �̵� ����
            }

            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.IncreaseGold(goldDropAmount); // ��� ����
                gm.IncreaseStage(); // �������� ����
                gm.ResetWave(); // ���̺� �ʱ�ȭ
                Debug.Log("���� ����");
            }

            Instantiate(deathEffect, transform.position, Quaternion.identity); // ��� ����Ʈ

            Destroy(gameObject); // ���� ������Ʈ ����

            if (backgroundScript != null)
            {
                backgroundScript.OnKilled(); // ��� ��ũ��Ʈ�� ���� ��� �˸�
            }
        }

        if (questScript != null)
        {
            questScript.IncrementMonsterKillCount(); // ���� óġ �� ������Ʈ
        }
    }

    public void SetGoldDropAmount(int amount)
    {
        goldDropAmount = amount; // ��� ��� �� ����
    }

    public void SetEnemyManager(EnemyManager manager)
    {
        enemyManager = manager; // EnemyManager ���� ����
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        while (Vector3.Distance(transform.position, playerTransform.position) <= stopDistance)
        {
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                animator.SetTrigger("attack"); // ���� �ִϸ��̼� Ʈ���� ����
                player.TakeDamage(attackPower); // �÷��̾�� ����
            }
            yield return new WaitForSeconds(1f); // 1�ʸ��� ����
        }

        isAttacking = false;
    }

    private IEnumerator BossTimer(float timeLimit)
    {
        yield return new WaitForSeconds(timeLimit);

        if (HP > 0) // ������ ���� ����ִٸ�
        {
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die(); // �÷��̾� ���
            }
        }
    }

    // EnemyManager�κ��� �ʱ�ȭ ���� �����޴� �޼���
    public void Initialize(int maxHP, int attackPower, int goldDropAmount)
    {
        this.maxHP = maxHP;
        this.attackPower = attackPower;
        this.goldDropAmount = goldDropAmount;
        this.HP = maxHP; // �ʱ� HP ����
        UpdateHPBar(); // HP �� �ʱ�ȭ
    }
}
