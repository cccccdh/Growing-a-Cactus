using UnityEngine;

public class Debugging : MonoBehaviour
{
    [Header("��ũ��Ʈ ����")]
    public GameManager gm;
    public GachaManager gachaManager;

    // ������ Ű ����
    void Update()
    {
        // ��� ��� ����
        if (Input.GetKeyDown(KeyCode.F1))
        {
            gachaManager.Unlock("���");
        }

        // �� ��� ����
        if (Input.GetKeyDown(KeyCode.F2))
        {
            gachaManager.Unlock("��");
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
