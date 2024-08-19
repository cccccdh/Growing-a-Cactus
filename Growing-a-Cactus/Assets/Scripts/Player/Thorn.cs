using UnityEngine;

public class Thorn : MonoBehaviour
{
    public ParticleSystem attackEffect;
    private PoolManager poolManager;
    private double damage;
    private bool isCritical;

    private void Start()
    {
        poolManager = PoolManager.Instance;
    }

    public void SetDamage(double damage)
    {
        this.damage = damage;
        isCritical = false;
    }

    public void SetCriticalDamage(double damage)
    {
        this.damage = damage;
        isCritical = true;
    }

    public void SetDirection(Vector2 direction)
    {
        // 가시의 속도 설정
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * 5; // 방향 벡터를 정규화하고 속도를 곱함

        // 가시의 회전 설정
        UpdateRotation(direction);
    }

    private void UpdateRotation(Vector2 direction)
    {
        // 방향 벡터를 기준으로 회전 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            HandleDamage(collision.gameObject, damage, isCritical);
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            poolManager.ReturnToThornPool(gameObject);
        }
    }

    private void HandleDamage(GameObject target, double damage, bool isCritical)
    {
        if (target.CompareTag("Enemy"))
        {
            EnemyScript enemy = target.GetComponent<EnemyScript>();
            enemy?.TakeDamage(damage);
        }
        else if (target.CompareTag("Boss"))
        {
            BossScript boss = target.GetComponent<BossScript>();
            boss?.TakeDamage(damage);
        }

        Instantiate(attackEffect, target.transform.position, Quaternion.identity);

        // 데미지 텍스트를 생성
        Vector3 pos = target.transform.position;
        poolManager.CreateDamageText(pos, damage, isCritical);

        // 가시 비활성화
        poolManager.ReturnToThornPool(gameObject);
    }
}