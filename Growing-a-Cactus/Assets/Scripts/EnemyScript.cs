using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private EnemyManager EnemyManager; // EnemyManager ����
    private Transform playerTransform; // �÷��̾��� Transform ����

    public Image HpBar;
    public int HP; // ���� �ʱ� HP ����
    public int maxHP = 31;

    public int enemyCount = 0;
    public int stageCount = 1;

    public float speed = 1f; // ���� �̵� �ӵ�

    private void Start()
    {
        EnemyManager = FindObjectOfType<EnemyManager>(); // EnemyManager ã��
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ã��
        HP = maxHP; // �ʱ� HP ����
        UpdateHPBar();
    }

    private void Update()
    {
        // �÷��̾� �������� �̵� ó�� 
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        HP -= (int)damage;
        UpdateHPBar();

        Debug.Log("������ ����");

        if (HP <= 0f)
        {
            Die();
        }
    }

    private void UpdateHPBar()
    {
        HpBar.fillAmount = (float)HP / maxHP;
    }

    public void Die()
    {
        // HP�� 0 ������ ��� ������Ʈ �ı� �� ��� ����
        if (HP <= 0)
        {
            GoldScript goldScript = FindObjectOfType<GoldScript>();
            if (goldScript != null)
            {
                goldScript.IncreaseGold();
            }

            if (EnemyManager != null)
            {
                EnemyManager.SpawnEnemy(); // ���� �ı��� �� ���ο� �� ���� ��û
            }

            Destroy(gameObject); // ���߿� ������Ʈ Ǯ������ ����
            enemyCount++;
        }
        if (enemyCount % 10 == 0 && enemyCount != 0)
        {
            StageScript stageScript = FindObjectOfType<StageScript>();
            if (stageScript != null)
            {
                stageScript.IncreaseStage();
            }
        }
    }
}