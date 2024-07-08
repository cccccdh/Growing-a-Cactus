using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int HP = 10; // ���� �ʱ� HP ����

    private void Update()
    {
        if (HP <= 0)
        {
            // HP�� 0 ���ϰ� �Ǹ� GoldScript�� �ִ� GoldText�� ���� 1 ������Ű�� ���� GoldScript�� ã�ƾ� ��
            GoldScript goldScript = FindObjectOfType<GoldScript>();
            if (goldScript != null)
            {
                goldScript.IncreaseGold();
            }

            Destroy(gameObject); // �� ������Ʈ �ı�
        }
    }
}