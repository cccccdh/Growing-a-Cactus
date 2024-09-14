using UnityEngine;

public class Debugging : MonoBehaviour
{
    [Header("스크립트 참조")]
    public GameManager gm;
    public GachaManager gachaManager;
    public QuestManager questManager;

    // 디버깅용 키 모음
    void Update()
    {
        // 장비 잠금 해제
        if (Input.GetKeyDown(KeyCode.F1))
        {
            gachaManager.Unlock("장비");
        }

        // 펫 잠금 해제
        if (Input.GetKeyDown(KeyCode.F2))
        {
            gachaManager.Unlock("펫");
        }

        // 골드 증가
        if (Input.GetKeyDown(KeyCode.F3))
        {
            gm.gold += 5000000;
        }

        // 젬 증가
        if (Input.GetKeyDown(KeyCode.F4))
        {
            gm.gem += 5000000;
        }

        // 의상 잠금 해제
        if (Input.GetKeyDown(KeyCode.F5))
        {
            gachaManager.Unlock("의상");
        }

        // 퀘스트 스킵 버튼
        if (Input.GetKeyDown(KeyCode.F6))
        {
            questManager.SkipQuest();
        }
       
    }
}
