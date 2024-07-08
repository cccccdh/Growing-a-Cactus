using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyScript : MonoBehaviour
{
    public int HP = 10; // ���� �ʱ� HP ����
    public Image HpBar;

    public float speed = 1f; // ���� �̵� �ӵ�

    private float moveTime = 2f; // �̵� �ð�
    private float moveElapsedTime = 0f; // ��� �ð�
    private float hpDecreaseInterval = 0.3f; // HP ���� ���� 
    private float hpElapsedTime = 0f; // HP ��� �ð�

    private EnemyManager EnemyManager; // EnemyManager ����

    private void Start()
    {
        EnemyManager = FindObjectOfType<EnemyManager>(); // EnemyManager ã��
    }

    private void Update()
    {
        // �̵� ó�� 
        if (moveElapsedTime < moveTime)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            moveElapsedTime += Time.deltaTime;
        }

        // HP ���� ó�� ���ݹ޴°� �����Ǹ� �̺κ� �����
        hpElapsedTime += Time.deltaTime;
        if (hpElapsedTime >= hpDecreaseInterval)
        {
            HP -= 1;
            HpBar.fillAmount -= 0.1f;
            hpElapsedTime = 0f;
        }

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

            Destroy(gameObject); // �� ������Ʈ �ı�
        }
    }
}