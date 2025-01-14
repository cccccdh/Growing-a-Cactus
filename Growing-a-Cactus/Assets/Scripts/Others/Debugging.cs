using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debugging : MonoBehaviour
{
    [Header("스크립트 참조")]
    public GameManager gm;
    public GachaManager gachaManager;
    public QuestManager questManager;

    public TextMeshProUGUI debugtext;

    private bool isDebugMode = false;   

    // 디버깅용 키 모음
    void Update()
    {
        debugtext.gameObject.SetActive(isDebugMode);

        if (Input.GetKeyDown(KeyCode.F12))
        {
            isDebugMode = !isDebugMode;            
        }

        if (isDebugMode)
        {
            HandleDebugInputs();
        }
    }

    private void HandleDebugInputs()
    {       
        // 골드 증가
        if (Input.GetKeyDown(KeyCode.F1))
        {
            gm.gold += 5000000;
        }

        // 젬 증가
        if (Input.GetKeyDown(KeyCode.F2))
        {
            gm.gem += 5000000;
        }

        // 장비 잠금 해제
        if (Input.GetKeyDown(KeyCode.F3))
        {
            gachaManager.Unlock("장비");
        }

        // 펫 잠금 해제
        if (Input.GetKeyDown(KeyCode.F4))
        {
            gachaManager.Unlock("펫");
        }

        // 의상 잠금 해제
        if (Input.GetKeyDown(KeyCode.F5))
        {
            gachaManager.Unlock("의상");
        }

        // 퀘스트 스킵 버튼
        if (Input.GetKeyDown(KeyCode.Q))
        {
            questManager.SkipQuest();
        }
    }
}
