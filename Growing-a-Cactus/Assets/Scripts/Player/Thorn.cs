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
        // ������ �ӵ� ����
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * 5; // ���� ���͸� ����ȭ�ϰ� �ӵ��� ����

        // ������ ȸ�� ����
        UpdateRotation(direction);
    }

    private void UpdateRotation(Vector2 direction)
    {
        // ���� ���͸� �������� ȸ�� ���
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

        // ������ �ؽ�Ʈ�� ����
        Vector3 pos = target.transform.position;
        poolManager.CreateDamageText(pos, damage, isCritical);

        // ���� ��Ȱ��ȭ
        poolManager.ReturnToThornPool(gameObject);
    }
}