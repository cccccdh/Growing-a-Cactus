using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    private Dictionary<string, CSVReader.Item> itemDictionary;
    public ItemManager itemManager;
    public GachaUIManager gachaUIManager;

    // 아이템 딕셔너리를 초기화하는 메서드
    public void InitializeItems(Dictionary<string, CSVReader.Item> items)
    {
        itemDictionary = items;
    }

    // 가챠를 수행하는 메서드
    public void PerformGacha(int times)
    {
        var resultItemList = new List<CSVReader.Item>();
        for (int i = 0; i < times; i++)
        {
            float rand = Random.Range(0, 100f);
            float cumulative = 0f;
            foreach (var item in itemDictionary.Values)
            {
                cumulative += item.Probability;
                if (rand < cumulative)
                {
                    resultItemList.Add(item);
                    itemManager.UpdateItemCount(item.Name); // 아이템 개수 업데이트
                    //Debug.Log($"뽑기 {i + 1}: {item.Name} (타입: {item.Type}, 등급: {item.Grade})");
                    break;
                }
            }
        }
        itemManager.UpdateEquipImages(resultItemList); // 장비창 업데이트
        gachaUIManager.UpdateGachaUI(resultItemList); // UI 업데이트
    }
}
