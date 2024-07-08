using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStatus status;
    private Transform target;
    private bool isAttacking = false;

    public GameObject thornPrefab;
    public float attackRange = 5f;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    void Start()
    {
        status.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {            
            status.Increase("Attack");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            status.Increase("HP");
        }
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
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            ShootThorn(direction, status.Attack);
            yield return new WaitForSeconds(status.Attack_Speed);

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
}
