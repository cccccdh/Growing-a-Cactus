using UnityEngine;

public class Thorn : MonoBehaviour
{
    private PoolManager poolManager;
    private float damage;
    private bool isCritical;

    private void Start()
    {
        poolManager = PoolManager.Instance;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
        isCritical = false;
    }

    public void SetCriticalDamage(float damage)
    {
        this.damage = damage;
        isCritical = true;
    }

    public void SetDirection(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * 5;
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

    private void HandleDamage(GameObject target, float damage, bool isCritical)
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

        // 데미지 텍스트를 생성
        Vector3 pos = target.transform.position;
        poolManager.CreateDamageText(pos, damage, isCritical);

        // 가시 비활성화
        poolManager.ReturnToThornPool(gameObject);
    }
}