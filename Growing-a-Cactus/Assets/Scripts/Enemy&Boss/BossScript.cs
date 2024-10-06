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
    public double HP; // ���� HP
    public double maxHP; // �ִ� HP
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
        Init();
    }

    // ���� �ʱ�ȭ
    public void Init()
    {
        if (enemyManager != null)
        {
            HP = enemyManager.bossMaxHP;
            maxHP = enemyManager.bossMaxHP;
            BossattackPower = enemyManager.bossAttackPower;
            goldDropAmount = enemyManager.bossGoldDropAmount;
            isAttacking = false;
        }
        UpdateHPBar();
    }

    private void Start()
    {
        // �÷��̾�, Animator, QuestScript, BackgroundScript �ʱ�ȭ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        questManager = QuestManager.instance;
        backgroundScript = FindObjectOfType<BackgroundScript>();
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
        HP -= damage;
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

            EffectManager.instance.MoveCoin(transform.position);

            if (GameManager.instance != null)
            {
                GameManager.instance.IncreaseGold(goldDropAmount);
                GameManager.instance.IncreaseStage();
                GameManager.instance.ResetWave();
            }
            if (GameManager.instance != null)
            {
                GameManager.instance.StopDecreaseWaveAndReset();
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
            questManager.UpdateQuestProgress(1, "���� óġ"); // ���� óġ �� ������Ʈ
        }
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

}
