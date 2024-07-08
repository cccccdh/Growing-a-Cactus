using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyScript : MonoBehaviour
{
    public int HP = 10; // 적의 초기 HP 설정
    public Image HpBar;

    public float speed = 1f; // 적의 이동 속도

    private float moveTime = 2f; // 이동 시간
    private float moveElapsedTime = 0f; // 경과 시간
    private float hpDecreaseInterval = 0.3f; // HP 감소 간격 
    private float hpElapsedTime = 0f; // HP 경과 시간

    private EnemyManager EnemyManager; // EnemyManager 참조

    private void Start()
    {
        EnemyManager = FindObjectOfType<EnemyManager>(); // EnemyManager 찾기
    }

    private void Update()
    {
        // 이동 처리 
        if (moveElapsedTime < moveTime)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            moveElapsedTime += Time.deltaTime;
        }

        // HP 감소 처리 공격받는거 구현되면 이부분 빼면댐
        hpElapsedTime += Time.deltaTime;
        if (hpElapsedTime >= hpDecreaseInterval)
        {
            HP -= 1;
            HpBar.fillAmount -= 0.1f;
            hpElapsedTime = 0f;
        }

        // HP가 0 이하일 경우 오브젝트 파괴 및 골드 증가
        if (HP <= 0)
        {
            GoldScript goldScript = FindObjectOfType<GoldScript>();
            if (goldScript != null)
            {
                goldScript.IncreaseGold();
            }

            if (EnemyManager != null)
            {
                EnemyManager.SpawnEnemy(); // 적이 파괴될 때 새로운 적 생성 요청
            }

            Destroy(gameObject); // 적 오브젝트 파괴
        }
    }
}