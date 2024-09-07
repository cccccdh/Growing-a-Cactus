using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image HpBar;
    public GameObject DiePage;
    public ParticleSystem SpeedLine;
    public Transform thronPivot;

    public double CurrentHp;
    public float attackRange = 5f;
    public float HpR;
    public bool isOpenDie = false;

    public PlayerStatus status;
    public EnemyManager enemyManager;
    private Transform target;
    private Animator animator;

    private bool isAttacking = false;
    private Vector3 originalPosition;
    private Vector3 shootpivot;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
        SpeedLine.Stop();
    }

    void Start()
    {        
        CurrentHp = status.effectiveHP;
        HpR = status.Hp_Recovery;
        originalPosition = transform.position;        
        shootpivot = thronPivot.position;
        StartCoroutine(HealthRegenCoroutine());
    }

    public void TakeDamage(double damage)
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
        animator.SetTrigger("Dead"); // ���� �ִϸ��̼� Ʈ����

        CurrentHp = status.effectiveHP; // HP �ʱ�ȭ
        UpdateHPBar();
        transform.position = originalPosition; // ���� ��ġ�� �ǵ�����
        StartCoroutine(OpenDieWithDelay(0.7f)); // 0.7�� ���� �� OpenDie ȣ��
        enemyManager.ResetRound(); // EnemyManager�� ���� ���� ��û
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
            StartCoroutine(FadeOutAfterDelay(10f)); // 10�� �� ���̵� �ƿ� ����
        }
        else
        {
            DiePage.SetActive(false);
        }
    }

    // ���� �ð� �� ������� �ڷ�ƾ
    IEnumerator FadeOutAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð�(10��) ���� ���

        CanvasGroup canvasGroup = DiePage.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = DiePage.AddComponent<CanvasGroup>(); // CanvasGroup�� ������ �߰�
        }

        float fadeDuration = 1.5f; // ���̵� �ƿ� ���� �ð� (1.5��)
        float fadeSpeed = 1f / fadeDuration;

        for (float t = 0; t < 1; t += Time.deltaTime * fadeSpeed)
        {
            canvasGroup.alpha = 1 - t; // ���� ���� ���������� ����
            yield return null;
        }

        canvasGroup.alpha = 0; // ������ �����ϰ� ����
        DiePage.SetActive(false); // ������ ��Ȱ��ȭ

        canvasGroup.alpha = 1; // �ʱ�ȭ
    }

    public void CloseDie()
    {
        DiePage.SetActive(false);
        isOpenDie = false;
    }

    // �÷��̾� ü�� �� ������Ʈ
    public void UpdateHPBar()
    {
        HpBar.fillAmount = (float)(CurrentHp / status.effectiveHP);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")) && !isAttacking)
        {
            target = collision.transform;
            StartCoroutine(AttackCoroutine());
        }
    }

    // ���� ü�� ����
    public void SetHp(float cHp)
    {
        CurrentHp += cHp;
    }

    // ü�� ��� ����
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

            if (target == null || !target.gameObject.activeInHierarchy)
            {
                target = null;
                isAttacking = false;
                FindAndAttackNextEnemy();
                break;
            }
        }
    }

    private void FindAndAttackNextEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy") && enemy.gameObject.activeInHierarchy)
            {
                target = enemy.transform;
                isAttacking = true;
                StartCoroutine(AttackCoroutine());
                break;
            }
        }
    }

    private void ShootThorn(Vector2 direction, double damage)
    {
        bool isCritical = Random.Range(0, 100f) < status.Critical;
        PoolManager.instance.GetThorn(shootpivot, direction, damage, isCritical);

        HandleDoubleAndTripleAttack(direction, damage);
    }

    private void HandleDoubleAndTripleAttack(Vector2 direction, double damage)
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

    private IEnumerator ShootThornWithDelay(Vector2 direction, double damage, float delay, int repeat = 1)
    {
        for (int i = 0; i < repeat; i++)
        {
            yield return new WaitForSeconds(delay);

            bool isCritical = Random.Range(0, 100f) < status.Critical;
            PoolManager.instance.GetThorn(transform.position, direction, damage, isCritical);
        }
    }

    private IEnumerator HealthRegenCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CurrentHp = Mathf.Min((float)CurrentHp + (int)HpR, (float)status.effectiveHP);
            UpdateHPBar();
        }
    }

    public void MovePlayerWithDelay(float delay)
    {
        StartCoroutine(MovePlayerCoroutine(delay));
    }

    private IEnumerator MovePlayerCoroutine(float delay)
    {
        animator.SetBool("Walk", true); // �ȱ� �ִϸ��̼� ����
        SpeedLine.Play(); // �̵� ����Ʈ ����

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
        SpeedLine.Stop(); // �̵� ����Ʈ ����

        animator.SetBool("Walk", false); // �ȱ� �ִϸ��̼� ����
    }
}
