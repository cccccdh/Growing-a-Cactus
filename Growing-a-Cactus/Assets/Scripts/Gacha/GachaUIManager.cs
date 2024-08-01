using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaUIManager : MonoBehaviour
{
    public TextMeshProUGUI[] gachaTexts;

    // 뽑기 결과를 UI에 반영
    public void UpdateGachaUI(List<Item> resultItemList)
    {
        // 가챠창의 텍스트를 초기화
        foreach (var text in gachaTexts)
        {
            text.text = "";
        }

        // 뽑기 결과를 가챠창 텍스트에 반영
        for (int i = 0; i < gachaTexts.Length; i++)
        {
            if(i < resultItemList.Count )
            {
                var result = resultItemList[i];                
                gachaTexts[i].text = result.Name;
            }
        }
    }

    // 뽑기 결과를 UI에 반영
    public void UpdateGachaUI(List<Pet> resultPetList)
    {
        // 가챠창의 텍스트를 초기화
        foreach (var text in gachaTexts)
        {
            text.text = "";
        }

        // 뽑기 결과를 가챠창 텍스트에 반영
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
