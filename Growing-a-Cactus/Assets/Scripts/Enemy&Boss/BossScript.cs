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
    private double HP; // ���� HP
    private double maxHP; // �ִ� HP
    public double BossattackPower; // ���ݷ�
    private int goldDropAmount; // ����ϴ� ��� ��

    public float speed = 0.6f; // �̵� �ӵ�
    public float stopDistance = 1.5f; // �÷��̾���� ���� �Ÿ�

    private bool isAttacking = false; // ���� �� ����
    private EnemyManager enemyManager; // EnemyManager ����
    private QuestManager questManager; // QuestScript ����
    private Transform playerTransform; // �÷��̾��� Transform
    private Animator animator; // Animator ������Ʈ
    private BackgroundScript backgroundScript; // BackgroundScript ����

    private void OnEnable()
    {
        HP = maxHP;
        UpdateHPBar();
    }

    private void Start()
    {
        // �÷��̾�, Animator, QuestScript, BackgroundScript �ʱ�ȭ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        questManager = QuestManager.instance;
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

    public void TakeDamage(double damage)
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
        HpBar.fillAmount = (float)(HP / maxHP);
    }

    public void Die()
    {
        // ���� ��� ó��
        if (HP <= 0)
        {
            PlayerController player = FindObjectOfType<PlayerController>();

            player?.MovePlayerWithDelay(1f); // �÷��̾� �̵� ����

            if (GameManager.instance != null)
            {
                GameManager.instance.IncreaseGold(goldDropAmount);
                GameManager.instance.IncreaseStage();
                GameManager.instance.ResetWave();
            }


            Instantiate(deathEffect, transform.position, Quaternion.identity); // ��� ����Ʈ

            PoolManager.instance.ReturnToBossPool(gameObject);

            if (backgroundScript != null)
            {
                backgroundScript.OnKilled(); // ��� ��ũ��Ʈ�� ���� ��� �˸�
            }
        }

        if (questManager != null)
        {
            questManager.UpdateQuestProgress(1, "�� óġ"); // ���� óġ �� ������Ʈ
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
                player.TakeDamage(BossattackPower); // �÷��̾�� ����
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
    public void Initialize(double maxHP, double attackPower, int goldDropAmount)
    {
        this.maxHP = maxHP;
        this.BossattackPower = attackPower;
        this.goldDropAmount = goldDropAmount;
        this.HP = maxHP; // �ʱ� HP ����
        UpdateHPBar(); // HP �� �ʱ�ȭ
    }
}
