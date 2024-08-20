using UnityEngine;

public class Debugging : MonoBehaviour
{
    public GameObject[] equipmentLockBtns;
    public GameObject[] petLockBtns;

    // ������ Ű ����
    void Update()
    {
        // ��� ��� ����
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (var btn in equipmentLockBtns)
            {
                btn.SetActive(false);
            }
        }

        // �� ��� ����
        if (Input.GetKeyDown(KeyCode.F2))
        {
            foreach (var btn in petLockBtns)
            {
                btn.SetActive(false);
            }
        }
    }
}
