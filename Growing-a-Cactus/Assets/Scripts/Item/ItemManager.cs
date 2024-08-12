using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    // 무기
    public Image WeaponImg;
    public Image EquipWeaponImg;
    public TextMeshProUGUI EquipWeaponText;
    public TextMeshProUGUI EquipWeaponLevelText;
    public TextMeshProUGUI WeaponNameText;
    public TextMeshProUGUI WeaponGradeText;
    public TextMeshProUGUI WeaponLevelText;
    public TextMeshProUGUI WeaponCountText;
    public TextMeshProUGUI WeaponRetentionEffect;
    public TextMeshProUGUI WeaponEquipEffectText;
    public Image[] weaponImages;
    public TextMeshProUGUI[] weaponCountTexts;
    public TextMeshProUGUI[] weaponLevelTexts;

    // 방어구
    public Image ArmorImg;
    public Image EquipArmorImg;
    public TextMeshProUGUI EquipArmorText;
    public TextMeshProUGUI EquipArmorLevelText;
    public TextMeshProUGUI ArmorNameText;
    public TextMeshProUGUI ArmorGradeText;
    public TextMeshProUGUI ArmorLevelText;
    public TextMeshProUGUI ArmorCountText;
    public TextMeshProUGUI ArmorRetentionEffect;
    public TextMeshProUGUI ArmorEquipEffectText;
    public Image[] armorImages;
    public TextMeshProUGUI[] armorCountTexts;
    public TextMeshProUGUI[] armorLevelTexts;

    private List<Item> weaponItems = new List<Item>();
    private List<Item> armorItems = new List<Item>();

    public PlayerStatus playerstatus;

    private string selectedItemName;
    private Color selectedItemColor;

    private void Awake()
    {
        playerstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    public void SetItems(List<Item> itemList)
    {
        weaponItems.Clear();
        armorItems.Clear();

        foreach (var item in itemList)
        {
            if (item.Type == "무기")
            {
                weaponItems.Add(item);
            }
            else if (item.Type == "방어구")
            {
                armorItems.Add(item);
            }
        }
        playerstatus.UpdateWeaponRetentionEffects(weaponItems);
        playerstatus.UpdateArmorRetentionEffects(armorItems);
    }

    // 아이템 개수를 업데이트하는 메서드
    public void UpdateItemCount(string itemName)
    {
        if (weaponItems.Exists(item => item.Name == itemName))
        {
            UpdateItemCountInList(weaponItems, itemName);
        }
        else if (armorItems.Exists(item => item.Name == itemName))
        {
            UpdateItemCountInList(armorItems, itemName);
        }
    }

    private void UpdateItemCountInList(List<Item> items, string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                item.Count++;
                UpdateItemText(itemName, items);
                playerstatus.UpdateWeaponRetentionEffects(weaponItems);
                playerstatus.UpdateArmorRetentionEffects(armorItems);
                break;
            }
        }
    }

    // 아이템 개수를 가져오는 메서드
    public int GetItemCount(string itemName)
    {
        int count = GetItemCountInList(weaponItems, itemName);
        if (count == 0)
        {
            count = GetItemCountInList(armorItems, itemName);
        }
        return count;
    }

    private int GetItemCountInList(List<Item> items, string itemName)
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
        int requiredCount = GetItemRequiredCountInList(weaponItems, itemName);
        if (requiredCount == 0)
        {
            requiredCount = GetItemRequiredCountInList(armorItems, itemName);
        }
        return requiredCount;
    }

    private int GetItemRequiredCountInList(List<Item> items, string itemName)
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
        int level = GetItemLevelInList(weaponItems, itemName);
        if (level == 0)
        {
            level = GetItemLevelInList(armorItems, itemName);
        }
        return level;
    }

    private int GetItemLevelInList(List<Item> items, string itemName)
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

    // 장비창 업데이트
    public void UpdateItemImages(List<Item> resultItemList)
    {
        UpdateItemImagesInList(resultItemList, weaponImages);
        UpdateItemImagesInList(resultItemList, armorImages);
    }

    private void UpdateItemImagesInList(List<Item> resultItemList, Image[] images)
    {
        foreach (var result in resultItemList)
        {
            foreach (var image in images)
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
    public void UpdateItemText(string itemName, List<Item> items)
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (weaponImages[i].name == itemName)
            {
                int count = GetItemCount(itemName);
                int requiredcount = GetItemRequiredCount(itemName);
                int level = GetItemLevel(itemName);
                weaponCountTexts[i].text = $"({count}/{requiredcount})";
                weaponLevelTexts[i].text = $"Lv.{level}";
                break;
            }
        }
        for (int i = 0; i < armorImages.Length; i++)
        {
            if (armorImages[i].name == itemName)
            {
                int count = GetItemCount(itemName);
                int requiredcount = GetItemRequiredCount(itemName);
                int level = GetItemLevel(itemName);
                armorCountTexts[i].text = $"({count}/{requiredcount})";
                armorLevelTexts[i].text = $"Lv.{level}";
                break;
            }
        }
    }

    // 착용 이미지, 장비, 등급 업데이트
    public void SelectedItem(Image image)
    {
        selectedItemName = image.name;

        selectedItemColor = image.color;
        selectedItemColor.a = 1f;

        // 선택 아이템 가져오기
        Item selectedItem = weaponItems.Find(item => item.Name == selectedItemName)
                        ?? armorItems.Find(item => item.Name == selectedItemName);

        if (selectedItem == null) return; 

        if (selectedItem.Type == "무기")
        {
            WeaponImg.color = selectedItemColor;
            WeaponNameText.text = selectedItemName;
            UpdateWeaponInfo(selectedItem);
        }
        else if (selectedItem.Type == "방어구")
        {
            ArmorImg.color = selectedItemColor;
            ArmorNameText.text = selectedItemName;
            UpdateArmorInfo(selectedItem);
        }
    }

    // 무기 정보 업데이트
    private void UpdateWeaponInfo(Item item)
    {
        WeaponGradeText.text = item.Grade;
        WeaponRetentionEffect.text = $"공격력 + {TextFormatter.FormatText(item.RetentionEffect * 100)}%";
        WeaponEquipEffectText.text = $"공격력 + {TextFormatter.FormatText(item.EquipEffect * 100)}%";
        WeaponLevelText.text = $"Lv.{item.Level}";
        WeaponCountText.text = $"( {item.Count} / {item.RequiredCount} )";
    }

    // 방어구 정보 업데이트
    private void UpdateArmorInfo(Item item)
    {
        ArmorGradeText.text = item.Grade;
        ArmorRetentionEffect.text = $"체력 + {TextFormatter.FormatText(item.RetentionEffect * 100)}%";
        ArmorEquipEffectText.text = $"체력 + {TextFormatter.FormatText(item.EquipEffect * 100)}%";
        ArmorLevelText.text = $"Lv.{item.Level}";
        ArmorCountText.text = $"( {item.Count} / {item.RequiredCount} )";
    }

    // 장비 장착
    public void EquipItem()
    {
        // 장착 아이템 개수 가져오기
        int count = GetItemCount(selectedItemName);

        // 장착 아이템 찾기
        Item selectedItem = weaponItems.Find(item => item.Name == selectedItemName) 
            ?? armorItems.Find(item => item.Name == selectedItemName);

        if (selectedItem != null && count > 0 || selectedItem.Level > 1)
        {
            if (selectedItem.Type == "무기")
            {
                EquipWeaponImg.color = selectedItemColor;
                EquipWeaponText.text = selectedItemName;
                EquipWeaponLevelText.text = $"Lv.{selectedItem.Level}";

                // 아이템 장착 효과 부여
                playerstatus.EquipWeapon(selectedItem);
            }
            else if (selectedItem.Type == "방어구")
            {
                EquipArmorImg.color = selectedItemColor;
                EquipArmorText.text = selectedItemName;
                EquipArmorLevelText.text = $"Lv.{selectedItem.Level}";

                // 아이템 장착 효과 부여
                playerstatus.EquipArmor(selectedItem);
            }            
        }
    }

    // 장비 강화
    public void EnhanceItem()
    {
        if (weaponItems.Exists(item => item.Name == selectedItemName))
        {
            EnhanceItemInList(weaponItems);
        }
        else if (armorItems.Exists(item => item.Name == selectedItemName))
        {
            EnhanceItemInList(armorItems);
        }
    }

    private void EnhanceItemInList(List<Item> items)
    {
        foreach (var item in items)
        {
            if (item.Name == selectedItemName)
            {
                if (item.Count >= item.RequiredCount)
                {
                    item.Count -= item.RequiredCount;
                    item.Level++;
                    item.RetentionEffect += 0.0146f;
                    item.EquipEffect += item.EquipEffect / 5;
                    item.RequiredCount += 2;

                    if (item.Type == "무기")
                    {
                        // 보유효과 업데이트
                        playerstatus.UpdateWeaponRetentionEffects(items);

                        // 장착효과 업데이트
                        if (playerstatus.GetEquippedWeapon() != null && playerstatus.GetEquippedWeapon().Name == item.Name)
                        {
                            playerstatus.EquipWeapon(item);
                        }

                        UpdateWeaponInfo(item);
                    }else
                    {
                        // 보유효과 업데이트
                        playerstatus.UpdateArmorRetentionEffects(items);

                        // 장착효과 업데이트
                        if (playerstatus.GetEquippedArmor() != null && playerstatus.GetEquippedArmor().Name == item.Name)
                        {
                            playerstatus.EquipArmor(item);
                        }

                        UpdateArmorInfo(item);
                    }                    

                    UpdateItemText(item.Name, items);

                    if (EquipWeaponText.text == selectedItemName || EquipArmorText.text == selectedItemName)
                    {
                        EquipItem();
                    }
                }
                break;
            }
        }
    }
}
