using UnityEngine;

public class Debugging : MonoBehaviour
{
    public GameObject[] equipmentLockBtns;
    public GameObject[] petLockBtns;

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
    }
}
