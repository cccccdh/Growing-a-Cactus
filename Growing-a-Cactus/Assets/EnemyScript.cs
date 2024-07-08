using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int HP = 10; // 적의 초기 HP 설정

    private void Update()
    {
        if (HP <= 0)
        {
            // HP가 0 이하가 되면 GoldScript에 있는 GoldText에 값을 1 증가시키기 위해 GoldScript를 찾아야 함
            GoldScript goldScript = FindObjectOfType<GoldScript>();
            if (goldScript != null)
            {
                goldScript.IncreaseGold();
            }

            Destroy(gameObject); // 적 오브젝트 파괴
        }
    }
}