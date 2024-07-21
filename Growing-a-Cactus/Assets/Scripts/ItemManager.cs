using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    public Image[] weaponImages;
    public TextMeshProUGUI[] weaponCountTexts;

    // 아이템 개수를 업데이트하는 메서드
    public void UpdateItemCount(string itemName)
    {
        if (itemCounts.ContainsKey(itemName))
        {
            itemCounts[itemName]++;
        }
        else
        {
            itemCounts[itemName] = 1;
        }
        UpdateWeaponCountText(itemName);
    }

    // 아이템 개수를 가져오는 메서드
    public int GetItemCount(string itemName)
    {
        return itemCounts.ContainsKey(itemName) ? itemCounts[itemName] : 0;
    }

    // 장비창 무기 업데이트
    public void UpdateEquipImages(List<CSVReader.Item> resultItemList)
    {
        // 가챠 결과를 바탕으로 장비창 이미지를 업데이트
        foreach (var result in resultItemList)
        {
            foreach (var image in weaponImages)
            {
                if (image.name == result.Name)
                {
                    Color color = image.color;
                    color.a = 1f; // 이미지 표시
                    image.color = color;
                }
            }
        }
    }

    // 아이템 개수에 따라 텍스트 업데이트
    private void UpdateWeaponCountText(string itemName)
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (weaponImages[i].name == itemName)
            {
                int count = GetItemCount(itemName);
                weaponCountTexts[i].text = "(" + count.ToString() + "/" + "0)";
                break;
            }
        }
    }

}
