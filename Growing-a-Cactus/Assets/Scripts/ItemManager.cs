using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    public Image[] weaponImages;
    public TextMeshProUGUI[] weaponCountTexts;

    // ������ ������ ������Ʈ�ϴ� �޼���
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

    // ������ ������ �������� �޼���
    public int GetItemCount(string itemName)
    {
        return itemCounts.ContainsKey(itemName) ? itemCounts[itemName] : 0;
    }

    // ���â ���� ������Ʈ
    public void UpdateEquipImages(List<CSVReader.Item> resultItemList)
    {
        // ��í ����� �������� ���â �̹����� ������Ʈ
        foreach (var result in resultItemList)
        {
            foreach (var image in weaponImages)
            {
                if (image.name == result.Name)
                {
                    Color color = image.color;
                    color.a = 1f; // �̹��� ǥ��
                    image.color = color;
                }
            }
        }
    }

    // ������ ������ ���� �ؽ�Ʈ ������Ʈ
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
