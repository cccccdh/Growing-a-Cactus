using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private EnemyManager EnemyManager; // EnemyManager ����
    private Transform playerTransform; // �÷��̾��� Transform ����
    public Image HpBar;
    public TextMeshProUGUI damageTxTPrefab; // ������ �ؽ�Ʈ ������

    public int HP; // ���� �ʱ� HP ����
    public int maxHP = 31;
    public float speed = 1f; // ���� �̵� �ӵ�
    private int goldDropAmount = 500;

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

        if (HP <= 0f)
        {
            Die();
        }
    }

    public void UpdateHPBar()
    {
        HpBar.fillAmount = (float)HP / maxHP;
    }

    public void Die()
    {
        // HP�� 0 ������ ��� ������Ʈ �ı� �� ��� ����
        if (HP <= 0)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.IncreaseGold(goldDropAmount);
            }

            if (EnemyManager != null)
            {
                EnemyManager.SpawnEnemy(); // ���� �ı��� �� ���ο� �� ���� ��û
                EnemyManager.stageincreaseManager();
            }

            Destroy(gameObject); // ���߿� ������Ʈ Ǯ������ ����
        }
    }

    public void SetGoldDropAmount(int amount)
    {
        goldDropAmount = amount;
    }
}