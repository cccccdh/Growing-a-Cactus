using UnityEngine;

public class Debugging : MonoBehaviour
{
    public GameObject[] equipmentLockBtns;
    public GameObject[] petLockBtns;

    public GameManager gm;

    // 디버깅용 키 모음
    void Update()
    {
        // 장비 잠금 해제
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (var btn in equipmentLockBtns)
            {
                btn.SetActive(false);
            }
        }

        // 펫 잠금 해제
        if (Input.GetKeyDown(KeyCode.F2))
        {
            foreach (var btn in petLockBtns)
            {
                btn.SetActive(false);
            }
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
    }
}
