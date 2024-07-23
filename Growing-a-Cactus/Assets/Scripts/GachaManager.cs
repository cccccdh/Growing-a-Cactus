using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    private List<CSVReader.Item> itemDictionary;
    public ItemManager itemManager;
    public GachaUIManager gachaUIManager;

    public void InitializeItems(List<CSVReader.Item> items)
    {
        itemDictionary = items;
    }

    // ��í�� �����ϴ� �޼���
    public void PerformGacha(int times)
    {
        var resultItemList = new List<CSVReader.Item>();
        for (int i = 0; i < times; i++)
        {
            float rand = Random.Range(0, 100f);
            float cumulative = 0f;
            foreach (var item in itemDictionary)
            {
                cumulative += item.Probability;
                if (rand < cumulative)
                {
                    resultItemList.Add(item);
                    itemManager.UpdateItemCount(item.Name); // ������ ���� ������Ʈ
                    Debug.Log($"�̱� {i + 1}: {item.Name} (Ÿ��: {item.Type}, ���: {item.Grade}), ���� : {item.Count}");
                    break;
                }
            }
        }
        itemManager.UpdateEquipImages(resultItemList); // ���â ������Ʈ
        gachaUIManager.UpdateGachaUI(resultItemList); // UI ������Ʈ
    }
}
