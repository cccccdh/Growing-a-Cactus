using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject thornPrefab;

    public GameObject DiePage;
    public float attackRange = 5f;

    public Image HpBar;
    public int CurrentHp;
    public float HpR;


    private PlayerStatus status;
    private EnemyManager enemyManager;

    private PoolManager poolManager;

    private Transform target;
    private bool isAttacking = false;
    private Vector3 originalPosition;

    public bool isOpenDie = false;


    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    void Start()
    {
        status.Init();
        CurrentHp = status.Hp;
        HpR = status.Hp_Recovery;
        originalPosition = transform.position;
        enemyManager = FindObjectOfType<EnemyManager>();
        poolManager = PoolManager.Instance;
        StartCoroutine(HealthRegenCoroutine());
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
        CurrentHp = status.Hp; // HP 초기화
        UpdateHPBar();
        transform.position = originalPosition; // 원래 위치로 되돌리기
        OpenDie();
        enemyManager.ResetRound(); // EnemyManager에 라운드 리셋 요청
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
        HpBar.fillAmount = (float)CurrentHp / status.Hp;
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
        poolManager.GetThorn(transform.position, direction, damage, isCritical);

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
            CurrentHp = Mathf.Min(CurrentHp + (int)HpR, status.Hp);
            UpdateHPBar();
        }
    }

    public void MovePlayerWithDelay(float delay)
    {
        StartCoroutine(MovePlayerCoroutine(delay));
    }

    private IEnumerator MovePlayerCoroutine(float delay)
    {
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
    }
}
