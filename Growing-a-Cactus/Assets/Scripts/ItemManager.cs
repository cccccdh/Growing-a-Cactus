using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Image WeaponImg;
    public Image EquipWeaponImg;
    public TextMeshProUGUI EquipWeaponText;
    public TextMeshProUGUI WeaponNameText;
    public TextMeshProUGUI WeaponGradeText;
    public TextMeshProUGUI ReactionEffectText;
    public TextMeshProUGUI EquipEffectText;
    public Image[] weaponImages;
    public TextMeshProUGUI[] weaponCountTexts;

    public PlayerStatus playerstatus;

    private List<CSVReader.Item> items = new List<CSVReader.Item>();
    private string selectedItemName;
    private Color selectedItemColor;

    private void Awake()
    {
        playerstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        if (playerstatus == null)
        {
            Debug.LogError("PlayerStatus component not found on the GameObject. Make sure the PlayerStatus component is attached.");
        }
    }

    public void SetItems(List<CSVReader.Item> itemList)
    {
        items = itemList;
        Update_PowerLevel();
    }

    // 아이템 개수를 업데이트하는 메서드
    public void UpdateItemCount(string itemName)
    {
        foreach(var item in items)
        {
            if(item.Name == itemName)
            {
                item.Count++;
                UpdateWeaponCountText(itemName);
                Update_PowerLevel();
                break;
            }
        }        
    }

    // 아이템 개수를 가져오는 메서드
    public int GetItemCount(string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                return item.Count;
            }
        }
        return 0;
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
                    color.a = 1f; 
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

    // 착용 이미지, 장비, 등급 업데이트
    public void UpdateWeapon(Image image)
    {
        selectedItemName = image.name;

        selectedItemColor = image.color;
        selectedItemColor.a = 1f;
        WeaponImg.color = selectedItemColor;

        WeaponNameText.text = selectedItemName;

        foreach (var item in items)
        {
            if (item.Name == selectedItemName)
            {
                WeaponGradeText.text = item.Grade;
                ReactionEffectText.text = $"공격력 + {item.ReactionEffect * 100}%";
                EquipEffectText.text = $"공격력 + {item.EquipEffect * 100}%";
                break;
            }
        }
    }

    // 아이템 장착
    public void UpdateEquip()
    {
        // 장착 아이템 개수 가져오기
        int count = GetItemCount(selectedItemName);

        if(count > 0)
        {
            // 장착 아이템 색 바꾸기
            EquipWeaponImg.color = selectedItemColor;

            // 장착 아이템 이름 바꾸기
            EquipWeaponText.text = selectedItemName;

            Update_PowerLevel();
        }
    }

    // 전투력 업데이트 메서드
    private void Update_PowerLevel()
    {
        if (playerstatus == null)
        {
            Debug.LogError("PlayerStatus component is not assigned.");
            return;
        }

        if (items == null)
        {
            Debug.LogError("Items list is not initialized.");
            return;
        }

        playerstatus.UpdatePowerLevel(items);
    }
}
