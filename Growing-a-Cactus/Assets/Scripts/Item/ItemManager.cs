using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    // ����
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

    // ��
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
            if (item.Type == "����")
            {
                weaponItems.Add(item);
            }
            else if (item.Type == "��")
            {
                armorItems.Add(item);
            }
        }
        playerstatus.UpdateWeaponRetentionEffects(weaponItems);
        playerstatus.UpdateArmorRetentionEffects(armorItems);
    }

    // ������ ������ ������Ʈ�ϴ� �޼���
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

    // ������ ������ �������� �޼���
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

    // ������ ��ȭ������ �������� �޼���
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


    // ������ ������ �������� �޼���
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

    // ���â ������Ʈ
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

    // �����ۿ� ���� �ؽ�Ʈ ������Ʈ
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

    // ���� �̹���, ���, ��� ������Ʈ
    public void SelectedItem(Image image)
    {
        selectedItemName = image.name;

        selectedItemColor = image.color;
        selectedItemColor.a = 1f;

        // ���� ������ ��������
        Item selectedItem = weaponItems.Find(item => item.Name == selectedItemName)
                        ?? armorItems.Find(item => item.Name == selectedItemName);

        if (selectedItem == null) return; 

        if (selectedItem.Type == "����")
        {
            WeaponImg.color = selectedItemColor;
            WeaponNameText.text = selectedItemName;
            UpdateWeaponInfo(selectedItem);
        }
        else if (selectedItem.Type == "��")
        {
            ArmorImg.color = selectedItemColor;
            ArmorNameText.text = selectedItemName;
            UpdateArmorInfo(selectedItem);
        }
    }

    // ���� ���� ������Ʈ
    private void UpdateWeaponInfo(Item item)
    {
        WeaponGradeText.text = item.Grade;
        WeaponRetentionEffect.text = $"���ݷ� + {TextFormatter.FormatText(item.RetentionEffect * 100)}%";
        WeaponEquipEffectText.text = $"���ݷ� + {TextFormatter.FormatText(item.EquipEffect * 100)}%";
        WeaponLevelText.text = $"Lv.{item.Level}";
        WeaponCountText.text = $"( {item.Count} / {item.RequiredCount} )";
    }

    // �� ���� ������Ʈ
    private void UpdateArmorInfo(Item item)
    {
        ArmorGradeText.text = item.Grade;
        ArmorRetentionEffect.text = $"ü�� + {TextFormatter.FormatText(item.RetentionEffect * 100)}%";
        ArmorEquipEffectText.text = $"ü�� + {TextFormatter.FormatText(item.EquipEffect * 100)}%";
        ArmorLevelText.text = $"Lv.{item.Level}";
        ArmorCountText.text = $"( {item.Count} / {item.RequiredCount} )";
    }

    // ��� ����
    public void EquipItem()
    {
        // ���� ������ ���� ��������
        int count = GetItemCount(selectedItemName);

        // ���� ������ ã��
        Item selectedItem = weaponItems.Find(item => item.Name == selectedItemName) 
            ?? armorItems.Find(item => item.Name == selectedItemName);

        if (selectedItem != null && count > 0 || selectedItem.Level > 1)
        {
            if (selectedItem.Type == "����")
            {
                EquipWeaponImg.color = selectedItemColor;
                EquipWeaponText.text = selectedItemName;
                EquipWeaponLevelText.text = $"Lv.{selectedItem.Level}";

                // ������ ���� ȿ�� �ο�
                playerstatus.EquipWeapon(selectedItem);
            }
            else if (selectedItem.Type == "��")
            {
                EquipArmorImg.color = selectedItemColor;
                EquipArmorText.text = selectedItemName;
                EquipArmorLevelText.text = $"Lv.{selectedItem.Level}";

                // ������ ���� ȿ�� �ο�
                playerstatus.EquipArmor(selectedItem);
            }            
        }
    }

    // ��� ��ȭ
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

                    if (item.Type == "����")
                    {
                        // ����ȿ�� ������Ʈ
                        playerstatus.UpdateWeaponRetentionEffects(items);

                        // ����ȿ�� ������Ʈ
                        if (playerstatus.GetEquippedWeapon() != null && playerstatus.GetEquippedWeapon().Name == item.Name)
                        {
                            playerstatus.EquipWeapon(item);
                        }

                        UpdateWeaponInfo(item);
                    }else
                    {
                        // ����ȿ�� ������Ʈ
                        playerstatus.UpdateArmorRetentionEffects(items);

                        // ����ȿ�� ������Ʈ
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
