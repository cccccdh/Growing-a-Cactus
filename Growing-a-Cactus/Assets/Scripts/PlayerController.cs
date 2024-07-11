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
        StartCoroutine(HealthRegenCoroutine()); // ü�� ȸ�� �ڷ�ƾ ����
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
            Debug.Log("�� �߰�");
            if (!isAttacking)
            {
                target = collision.gameObject.transform;
                StartCoroutine(AttackCoroutine());
            }
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            Debug.Log("�� �߰�");
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

            // ���� �׾����� Ȯ��
            if (target == null)
            {
                target = null;
                isAttacking = false;
                break;
            }
        }

        // ���� ���� �ִ� �ٸ� ���� ã�� Ÿ����
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

    // ü�� ȸ�� �ڷ�ƾ
    private IEnumerator HealthRegenCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1�� �������� ȸ��
            CurrentHp = Mathf.Min(CurrentHp + (int)HpR, status.Hp); // �ִ� ü���� ���� �ʰ� ��
            UpdateHPBar();
        }
    }
}