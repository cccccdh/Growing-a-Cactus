using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Image WeaponImg;
    public Image EquipWeaponImg;
    public TextMeshProUGUI EquipWeaponText;
    public TextMeshProUGUI EquipWeaponLevelText;
    public TextMeshProUGUI WeaponNameText;
    public TextMeshProUGUI WeaponGradeText;
    public TextMeshProUGUI WeaponLevelText;
    public TextMeshProUGUI WeaponCountText;
    public TextMeshProUGUI ReactionEffectText;
    public TextMeshProUGUI EquipEffectText;
    public Image[] weaponImages;
    public TextMeshProUGUI[] weaponCountTexts;
    public TextMeshProUGUI[] weaponLevelTexts;

    public PlayerStatus playerstatus;

    private List<CSVReader.Item> items = new List<CSVReader.Item>();
    private string selectedItemName;
    private Color selectedItemColor;

    private void Awake()
    {
        playerstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    public void SetItems(List<CSVReader.Item> itemList)
    {
        items = itemList;
        playerstatus.UpdateReactionEffects(items);
    }

    // 아이템 개수를 업데이트하는 메서드
    public void UpdateItemCount(string itemName)
    {
        foreach(var item in items)
        {
            if(item.Name == itemName)
            {
                item.Count++;
                UpdateWeaponText(itemName);
                playerstatus.UpdateReactionEffects(items);
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

    // 아이템 강화개수를 가져오는 메서드
    public int GetItemRequiredCount(string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                return item.RequiredCount;
            }
        }
        return 0;
    }


    // 아이템 레벨을 가져오는 메서드
    public int GetItemLevel(string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                return item.Level;
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

    // 아이템에 따라 텍스트 업데이트
    public void UpdateWeaponText(string itemName)
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (weaponImages[i].name == itemName)
            {
                int count = GetItemCount(itemName);
                int requredcount = GetItemRequiredCount(itemName);
                int level = GetItemLevel(itemName);
                weaponCountTexts[i].text = $"{count}/{requredcount}";
                weaponLevelTexts[i].text = $"Lv.{level}";
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

        UpdateText();
    }

    // 선택 장비 업데이트
    public void UpdateText()
    {
        foreach (var item in items)
        {
            if (item.Name == selectedItemName)
            {
                WeaponGradeText.text = item.Grade;
                ReactionEffectText.text = $"공격력 + {TextFormatter.FormatText_F(item.ReactionEffect * 100)}%";
                EquipEffectText.text = $"공격력 + {TextFormatter.FormatText_F(item.EquipEffect * 100)}%";
                WeaponLevelText.text = $"Lv.{item.Level}";
                WeaponCountText.text = $"( {item.Count} / {item.RequiredCount} )";
                break;
            }
        }
    }

    // 장비 장착
    public void UpdateSelected()
    {
        // 장착 아이템 개수 가져오기
        int count = GetItemCount(selectedItemName);
        
        // 장착 아이템 찾기
        CSVReader.Item selectedItem = items.Find(item => item.Name == selectedItemName);

        if (count > 0 || selectedItem.Level > 1)
        {
            // 장착 아이템 색 바꾸기
            EquipWeaponImg.color = selectedItemColor;

            // 장착 아이템 이름 바꾸기
            EquipWeaponText.text = selectedItemName;

            // 장착 아이템 레벨 바꾸기
            EquipWeaponLevelText.text = $"Lv.{selectedItem.Level}";

            // 아이템 장착 효과 부여
            foreach (var item in items)
            {
                if (item.Name == selectedItemName)
                {
                    playerstatus.EquipItem(item);
                    break;
                }
            }
        }
    }

    // 장비 강화
    public void EnhanceItem()
    {
        foreach (var item in items)
        {
            if (item.Name == selectedItemName)
            {
                if (item.Count >= item.RequiredCount)
                {
                    item.Count -= item.RequiredCount;
                    item.Level++;
                    item.ReactionEffect += 0.0146f;
                    item.EquipEffect += item.EquipEffect / 5;
                    item.RequiredCount += 2;

                    // 보유효과 업데이트
                    playerstatus.UpdateReactionEffects(items);

                    // 장착효과 업데이트
                    if (playerstatus.GetEquippedItem() != null && playerstatus.GetEquippedItem().Name == item.Name)
                    {
                        playerstatus.EquipItem(item);
                    }

                    UpdateWeaponText(item.Name);
                    UpdateText();

                    if (EquipWeaponText.text == selectedItemName)
                    {
                        UpdateSelected();
                    }
                }
                break;
            }
        }
    }
}
