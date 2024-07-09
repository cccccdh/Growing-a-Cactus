using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageScript : MonoBehaviour
{
    public TextMeshProUGUI StageText;
    private int stageNumber = 1;  // "1-1"의 뒤의 숫자를 추적하기 위한 변수
    private int roundNumber = 1;

    void Start()
    {
        UpdateStageText();
    }

    void UpdateStageText()
    {
        StageText.text = stageNumber.ToString() + "-" + roundNumber.ToString() + "Stage";
    }

    public void IncreaseStage()
    {
        roundNumber++;
        UpdateStageText();

        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.SetStageInfo(stageNumber, roundNumber); // 현재 스테이지 정보 업데이트
        }
    }
}