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
    public TextMeshProUGUI WeaponRetentionEffect;
    public TextMeshProUGUI WeaponEquipEffectText;
    public Image[] weaponImages;
    public TextMeshProUGUI[] weaponCountTexts;
    public TextMeshProUGUI[] weaponLevelTexts;

    public PlayerStatus playerstatus;

    private List<Item> items = new List<Item>();
    private string selectedItemName;
    private Color selectedItemColor;

    private void Awake()
    {
        playerstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    public void SetItems(List<Item> itemList)
    {
        items = itemList;
        playerstatus.UpdateRetentionEffects(items);
    }

    // ������ ������ ������Ʈ�ϴ� �޼���
    public void UpdateItemCount(string itemName)
    {
        foreach(var item in items)
        {
            if(item.Name == itemName)
            {
                item.Count++;
                UpdateItemText(itemName);
                playerstatus.UpdateRetentionEffects(items);
                break;
            }
        }        
    }

    // ������ ������ �������� �޼���
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

    // ������ ��ȭ������ �������� �޼���
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


    // ������ ������ �������� �޼���
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

    // ���â ���� ������Ʈ
    public void UpdateItemImages(List<Item> resultItemList)
    {
        // ��í ����� �������� ���â �̹����� ������Ʈ
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

    // �����ۿ� ���� �ؽ�Ʈ ������Ʈ
    public void UpdateItemText(string itemName)
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
    }

    // ���� �̹���, ���, ��� ������Ʈ
    public void SelectedItem(Image image)
    {
        selectedItemName = image.name;

        selectedItemColor = image.color;
        selectedItemColor.a = 1f;
        WeaponImg.color = selectedItemColor;

        WeaponNameText.text = selectedItemName;

        InfomationText();
    }

    // ���� ��� ������Ʈ
    public void InfomationText()
    {
        foreach (var item in items)
        {
            if (item.Name == selectedItemName)
            {
                WeaponGradeText.text = item.Grade;
                WeaponRetentionEffect.text = $"���ݷ� + {TextFormatter.FormatText_F(item.RetentionEffect * 100)}%";
                WeaponEquipEffectText.text = $"���ݷ� + {TextFormatter.FormatText_F(item.EquipEffect * 100)}%";
                WeaponLevelText.text = $"Lv.{item.Level}";
                WeaponCountText.text = $"( {item.Count} / {item.RequiredCount} )";
                break;
            }
        }
    }

    // ��� ����
    public void EquipItem()
    {
        // ���� ������ ���� ��������
        int count = GetItemCount(selectedItemName);
        
        // ���� ������ ã��
        Item selectedItem = items.Find(item => item.Name == selectedItemName);

        if (count > 0 || selectedItem.Level > 1)
        {
            // ���� ������ �� �ٲٱ�
            EquipWeaponImg.color = selectedItemColor;

            // ���� ������ �̸� �ٲٱ�
            EquipWeaponText.text = selectedItemName;

            // ���� ������ ���� �ٲٱ�
            EquipWeaponLevelText.text = $"Lv.{selectedItem.Level}";

            // ������ ���� ȿ�� �ο�
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

    // ��� ��ȭ
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
                    item.RetentionEffect += 0.0146f;
                    item.EquipEffect += item.EquipEffect / 5;
                    item.RequiredCount += 2;

                    // ����ȿ�� ������Ʈ
                    playerstatus.UpdateRetentionEffects(items);

                    // ����ȿ�� ������Ʈ
                    if (playerstatus.GetEquippedItem() != null && playerstatus.GetEquippedItem().Name == item.Name)
                    {
                        playerstatus.EquipItem(item);
                    }

                    UpdateItemText(item.Name);
                    InfomationText();

                    if (EquipWeaponText.text == selectedItemName)
                    {
                        EquipItem();
                    }
                }
                break;
            }
        }
    }
}
