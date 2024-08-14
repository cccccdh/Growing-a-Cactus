using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image HpBar;
    public GameObject DiePage;
    public ParticleSystem attackEffect;
    public Transform thronPivot;
    public float CurrentHp;
    public float attackRange = 5f;
    public float HpR;
    public bool isOpenDie = false;

    public PlayerStatus status;
    private EnemyManager enemyManager;
    private PoolManager poolManager;
    private Transform target;
    private Animator animator;

    private bool isAttacking = false;
    private Vector3 originalPosition;
    private Vector3 shootpivot;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    void Start()
    {
        status.Init();
        CurrentHp = status.effectiveHP;
        HpR = status.Hp_Recovery;
        originalPosition = transform.position;
        enemyManager = FindObjectOfType<EnemyManager>();
        poolManager = PoolManager.Instance;
        animator = GetComponent<Animator>();
        StartCoroutine(HealthRegenCoroutine());
        shootpivot = thronPivot.position;
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= (int)damage;
        UpdateHPBar();
        if (CurrentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("Dead"); // 죽음 애니메이션 트리거

        CurrentHp = status.effectiveHP; // HP 초기화
        UpdateHPBar();
        transform.position = originalPosition; // 원래 위치로 되돌리기
        StartCoroutine(OpenDieWithDelay(0.5f)); // 0.5초 지연 후 OpenDie 호출
        enemyManager.ResetRound(); // EnemyManager에 라운드 리셋 요청
    }

    private IEnumerator OpenDieWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OpenDie();
    }

    public void OpenDie()
    {
        isOpenDie = !isOpenDie;
        if (isOpenDie)
        {
            DiePage.SetActive(true);
        }
        else
        {
            DiePage.SetActive(false);
        }
    }

    public void CloseDie()
    {
        DiePage.SetActive(false);
        isOpenDie = false;
    }

    public void UpdateHPBar()
    {
        HpBar.fillAmount = (float)CurrentHp / status.effectiveHP;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")) && !isAttacking)
        {
            target = collision.gameObject.transform;
            StartCoroutine(AttackCoroutine());
        }
    }

    public void SetHpR(float newHpR)
    {
        HpR = newHpR;
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            ShootThorn(direction, status.PowerLevel);
            animator.SetTrigger("Attack");
            yield return new WaitForSecondsRealtime(1 / status.Attack_Speed);

            if (target == null)
            {
                target = null;
                isAttacking = false;
                break;
            }
        }

        if (!isAttacking)
        {
            FindAndAttackNextEnemy();
        }
    }

    private void FindAndAttackNextEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                target = enemy.transform;
                isAttacking = true;
                StartCoroutine(AttackCoroutine());
                break;
            }
        }
    }

    private void ShootThorn(Vector2 direction, float damage)
    {
        bool isCritical = Random.Range(0, 100f) < status.Critical;
        poolManager.GetThorn(shootpivot, direction, damage, isCritical);

        HandleDoubleAndTripleAttack(direction, damage);
    }

    private void HandleDoubleAndTripleAttack(Vector2 direction, float damage)
    {
        if (Random.Range(0, 100f) < status.DoubleAttackChance)
        {
            Debug.Log("Double attack!");
            StartCoroutine(ShootThornWithDelay(direction, damage, 0.1f));
        }

        if (Random.Range(0, 100f) < status.TripleAttackChance)
        {
            Debug.Log("Triple attack!");
            StartCoroutine(ShootThornWithDelay(direction, damage, 0.1f, 2));
        }
    }

    private IEnumerator ShootThornWithDelay(Vector2 direction, float damage, float delay, int repeat = 1)
    {
        for (int i = 0; i < repeat; i++)
        {
            yield return new WaitForSeconds(delay);

            bool isCritical = Random.Range(0, 100f) < status.Critical;
            poolManager.GetThorn(transform.position, direction, damage, isCritical);
        }
    }

    private IEnumerator HealthRegenCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CurrentHp = Mathf.Min(CurrentHp + (int)HpR, status.effectiveHP);
            UpdateHPBar();
        }
    }

    public void MovePlayerWithDelay(float delay)
    {
        StartCoroutine(MovePlayerCoroutine(delay));
    }

    private IEnumerator MovePlayerCoroutine(float delay)
    {
        animator.SetBool("Walk", true); // 걷기 애니메이션 시작

        Vector3 targetPosition = originalPosition + Vector3.right * 1f;

        float elapsedTime = 0f;
        while (elapsedTime < 0.4f)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / 0.4f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        yield return new WaitForSeconds(delay - 0.4f);

        elapsedTime = 0f;
        while (elapsedTime < 0.2f)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;

        animator.SetBool("Walk", false); // 걷기 애니메이션 종료
    }
}
