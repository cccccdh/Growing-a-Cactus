using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PlayerStatus status;
    private Transform target;
    private bool isAttacking = false;

    public GameObject thornPrefab;
    public float attackRange = 5f;
    public Image HpBar;
    public int CurrentHp;
    public float HpR;

    private Vector3 originalPosition;

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
        StartCoroutine(HealthRegenCoroutine());
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= (int)damage;
        UpdateHPBar();
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
        GameObject thornObj = Instantiate(thornPrefab, transform.position, Quaternion.identity);
        Thorn thorn = thornObj.GetComponent<Thorn>();
        if (thorn != null)
        {
            float rand = Random.Range(0, 100f);
            if (rand < status.Critical)
            {
                Debug.Log("Critical hit!");
                thorn.SetCriticalDamage(damage * (status.Critical_Damage / 100f));
            }
            else
            {
                thorn.SetDamage(damage);
            }
            thorn.SetDirection(direction);

            HandleDoubleAndTripleAttack(direction, damage);
        }
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

            GameObject thornObj = Instantiate(thornPrefab, transform.position, Quaternion.identity);
            Thorn thorn = thornObj.GetComponent<Thorn>();
            if (thorn != null)
            {
                thorn.SetDamage(damage);
                thorn.SetDirection(direction);
            }
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
