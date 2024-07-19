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
            float rand = Random.Range(0, 100f);
            if (rand < status.Critical)
            {
                Debug.Log("치명타 발생");
                thorn.SetCriticalDamage(damage * (status.Critical_Damage / 100f));
            }
            else
            {
                thorn.SetDamage(damage);
            }
            thorn.SetDirection(direction);
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

    public void MovePlayerWithDelay(float delay)
    {
        StartCoroutine(MovePlayerCoroutine(delay));
    }

    private IEnumerator MovePlayerCoroutine(float delay)
    {
        Vector3 targetPosition = originalPosition + Vector3.right * 2.0f; // 2.0f 만큼 오른쪽으로 이동

        // 0.7초 동안 오른쪽으로 이동
        float elapsedTime = 0f;
        while (elapsedTime < 0.7f)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / 0.7f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // 정확히 오른쪽 끝으로 위치 고정

        // 0.3초 동안 원래 위치로 돌아오기
        yield return new WaitForSeconds(delay - 0.7f); // 전체 딜레이에서 0.7초를 뺀 시간만큼 대기
        elapsedTime = 0f;
        while (elapsedTime < 0.3f)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // 원래 위치로 복귀
    }
}