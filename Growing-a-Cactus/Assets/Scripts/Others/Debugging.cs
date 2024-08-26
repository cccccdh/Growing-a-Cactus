using UnityEngine;

public class Debugging : MonoBehaviour
{
    public GameObject[] equipmentLockBtns;
    public GameObject[] petLockBtns;

    public GameManager gm;

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

        // ��� ����
        if (Input.GetKeyDown(KeyCode.F3))
        {
            gm.gold += 5000000;
        }

        // �� ����
        if (Input.GetKeyDown(KeyCode.F4))
        {
            gm.gem += 5000000;
        }
    }
}
