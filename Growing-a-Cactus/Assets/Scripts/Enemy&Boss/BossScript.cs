using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;


public class BossScript : MonoBehaviour
{
    // UI 및 이펙트 관련 변수
    public Image HpBar;
    public ParticleSystem deathEffect;

    // 보스의 상태 변수
    public double HP; // 현재 HP
    public double maxHP; // 최대 HP
    public double BossattackPower; // 공격력
    private int goldDropAmount; // 드랍하는 골드 양

    public float speed = 0.6f; // 이동 속도
    public float stopDistance = 1.5f; // 플레이어와의 접촉 거리

    private bool isAttacking = false; // 공격 중 여부
    private EnemyManager enemyManager; // EnemyManager 참조
    private QuestManager questManager; // QuestScript 참조
    private Transform playerTransform; // 플레이어의 Transform
    private Animator animator; // Animator 컴포넌트
    private BackgroundScript backgroundScript; // BackgroundScript 참조

    private void OnEnable()
    {
        Init();
    }

    // 스텟 초기화
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
        // 플레이어, Animator, QuestScript, BackgroundScript 초기화
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        questManager = QuestManager.instance;
        backgroundScript = FindObjectOfType<BackgroundScript>();
    }

    private void Update()
    {
        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 플레이어가 일정 거리 이상 떨어져 있으면 이동
        if (distanceToPlayer > stopDistance)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0; // y축 이동 방지
            transform.Translate(direction * speed * Time.deltaTime);
        }
        // 플레이어와 가까워지면 공격 시작
        else if (!isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    public void TakeDamage(double damage)
    {
        // 피해 처리 및 HP 바 업데이트
        HP -= damage;
        UpdateHPBar();

        // HP가 0 이하일 때 사망 처리
        if (HP <= 0)
        {
            Die();
        }
    }

    public void UpdateHPBar()
    {
        // HP 바 업데이트
        HpBar.fillAmount = (float)(HP / maxHP);
    }

    public void Die()
    {
        // 보스 사망 처리
        if (HP <= 0)
        {
            PlayerController player = FindObjectOfType<PlayerController>();

            player?.MovePlayerWithDelay(1f); // 플레이어 이동 지연

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
            Instantiate(deathEffect, transform.position, Quaternion.identity); // 사망 이펙트

            PoolManager.instance.ReturnToBossPool(gameObject);

            if (backgroundScript != null)
            {
                backgroundScript.OnKilled(); // 배경 스크립트에 보스 사망 알림
            }
        }

        if (questManager != null)
        {
            questManager.UpdateQuestProgress(1, "적 처치"); // 몬스터 처치 수 업데이트
            questManager.UpdateQuestProgress(1, "보스 처치"); // 몬스터 처치 수 업데이트
        }
    }



    public void SetEnemyManager(EnemyManager manager)
    {
        enemyManager = manager; // EnemyManager 참조 설정
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        while (Vector3.Distance(transform.position, playerTransform.position) <= stopDistance)
        {
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                animator.SetTrigger("attack"); // 공격 애니메이션 트리거 설정
                player.TakeDamage(BossattackPower); // 플레이어에게 피해
            }
            yield return new WaitForSeconds(1f); // 1초마다 공격
        }

        isAttacking = false;
    }

}
