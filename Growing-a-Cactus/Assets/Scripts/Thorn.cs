using UnityEditor.EditorTools;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    PoolManager poolManager;
    float damage;

    private void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetDirection(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * 5;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ������ ������ ������
            }

            Vector3 pos = collision.transform.position;
            poolManager.CreateDamageText(pos, damage);

            // Enqueue
            Destroy(gameObject); // ���� �ı�
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossScript enemy = collision.gameObject.GetComponent<BossScript>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ������ ������ ������
            }

            Vector3 pos = collision.transform.position;
            //PoolManager.Instance.DequeueWithText(pos, damage);
            poolManager.CreateDamageText(pos, damage);

            // Enqueue
            Destroy(gameObject); // ���� �ı�
        }
    }
}
