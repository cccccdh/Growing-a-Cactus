using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageScript : MonoBehaviour
{
    public TextMeshProUGUI StageText;
    private int stageNumber = 1;  // "1-1"�� ���� ���ڸ� �����ϱ� ���� ����
    private int roundNumber = 1;

    void Start()
    {
        UpdateStageText();
    }

    void UpdateStageText()
    {
        StageText.text = roundNumber.ToString() + "-" + stageNumber.ToString();
    }

    public void IncreaseStage()
    {
        stageNumber++;
        UpdateStageText();
    }
}