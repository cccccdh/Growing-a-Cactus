using System.Collections.Generic;
using UnityEngine;

public class EnhancementManager : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public ItemManager itemManager;

    private List<CSVReader.Item> items;


    void Start()
    {
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        itemManager = FindObjectOfType<ItemManager>();
    }

    public bool EnhanceItem(CSVReader.Item item)
    {
        if (item.Count >= item.RequiredCount)
        {
            item.Count -= item.RequiredCount;
            item.Level++;
            item.ReactionEffect += 0.0146f;
            item.EquipEffect += item.EquipEffect / 5;
            item.RequiredCount += 2;            
            return true;
        }
        return false;
    }

    public void EnhanceItemByName(string itemName)
    {
        items = itemManager.GetItems();

        foreach (var item in items)
        {
            if(item.Name == itemName)
            {
                if (EnhanceItem(item))
                {
                    itemManager.SetItems(items);
                    itemManager.UpdateWeaponCountText(item.Name);
                    itemManager.UpdateText();
                }
                break;
            }
        }
    }
}
