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

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    void Start()
    {
        status.Init();
        CurrentHp = status.Hp;
        HpR = status.Hp_Recovery;
        StartCoroutine(HealthRegenCoroutine()); // 체력 회복 코루틴 시작
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= (int)damage;
        UpdateHPBar();
        /*
        if (HP <= 0f)
        {
            Die();
        }
        */
    }

    public void UpdateHPBar()
    {
        HpBar.fillAmount = (float)CurrentHp / status.Hp;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("적 발견");
            if (!isAttacking)
            {
                target = collision.gameObject.transform;
                StartCoroutine(AttackCoroutine());
            }
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            Debug.Log("적 발견");
            if (!isAttacking)
            {
                target = collision.gameObject.transform;
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            ShootThorn(direction, status.Attack);
            yield return new WaitForSecondsRealtime(1 / status.Attack_Speed);

            // 적이 죽었는지 확인
            if (target == null)
            {
                target = null;
                isAttacking = false;
                break;
            }
        }

        // 범위 내에 있는 다른 적을 찾아 타겟팅
        if (!isAttacking)
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
    }

    private void ShootThorn(Vector2 direction, float damage)
    {
        GameObject thornobj = Instantiate(thornPrefab, transform.position, Quaternion.identity);
        Thorn thorn = thornobj.GetComponent<Thorn>();
        if (thorn != null)
        {
            thorn.SetDamage(damage);
            thorn.SetDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                TakeDamage(enemy.attackPower);
            }
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossScript Boss = collision.gameObject.GetComponent<BossScript>();
            if (Boss != null)
            {
                TakeDamage(Boss.attackPower);
            }
        }
    }

    // 체력 회복 코루틴
    private IEnumerator HealthRegenCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초 간격으로 회복
            CurrentHp = Mathf.Min(CurrentHp + (int)HpR, status.Hp); // 최대 체력을 넘지 않게 함
            UpdateHPBar();
        }
    }
}