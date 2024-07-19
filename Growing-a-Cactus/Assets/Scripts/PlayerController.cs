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
            if (!isAttacking)
            {
                target = collision.gameObject.transform;
                StartCoroutine(AttackCoroutine());
            }
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
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
            float rand = Random.Range(0, 100f);
            if (rand < status.Critical)
            {
                Debug.Log("ġ��Ÿ �߻�");
                thorn.SetCriticalDamage(damage * (status.Critical_Damage / 100f));
            }
            else
            {
                thorn.SetDamage(damage);
            }
            thorn.SetDirection(direction);
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

    public void MovePlayerWithDelay(float delay)
    {
        StartCoroutine(MovePlayerCoroutine(delay));
    }

    private IEnumerator MovePlayerCoroutine(float delay)
    {
        Vector3 targetPosition = originalPosition + Vector3.right * 2.0f; // 2.0f ��ŭ ���������� �̵�

        // 0.7�� ���� ���������� �̵�
        float elapsedTime = 0f;
        while (elapsedTime < 0.7f)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / 0.7f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // ��Ȯ�� ������ ������ ��ġ ����

        // 0.3�� ���� ���� ��ġ�� ���ƿ���
        yield return new WaitForSeconds(delay - 0.7f); // ��ü �����̿��� 0.7�ʸ� �� �ð���ŭ ���
        elapsedTime = 0f;
        while (elapsedTime < 0.3f)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // ���� ��ġ�� ����
    }
}