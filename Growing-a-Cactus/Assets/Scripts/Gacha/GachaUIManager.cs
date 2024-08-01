using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaUIManager : MonoBehaviour
{
    public TextMeshProUGUI[] gachaTexts;

    // �̱� ����� UI�� �ݿ�
    public void UpdateGachaUI(List<Item> resultItemList)
    {
        // ��íâ�� �ؽ�Ʈ�� �ʱ�ȭ
        foreach (var text in gachaTexts)
        {
            text.text = "";
        }

        // �̱� ����� ��íâ �ؽ�Ʈ�� �ݿ�
        for (int i = 0; i < gachaTexts.Length; i++)
        {
            if(i < resultItemList.Count )
            {
                var result = resultItemList[i];                
                gachaTexts[i].text = result.Name;
            }
        }
    }

    // �̱� ����� UI�� �ݿ�
    public void UpdateGachaUI(List<Pet> resultPetList)
    {
        // ��íâ�� �ؽ�Ʈ�� �ʱ�ȭ
        foreach (var text in gachaTexts)
        {
            text.text = "";
        }

        // �̱� ����� ��íâ �ؽ�Ʈ�� �ݿ�
        for (int i = 0; i < gachaTexts.Length; i++)
        {
            if (i < resultPetList.Count)
            {
                var result = resultPetList[i];
                gachaTexts[i].text = result.Name;
            }
        }
    }
}
