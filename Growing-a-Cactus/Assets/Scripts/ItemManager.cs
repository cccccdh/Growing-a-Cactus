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

    // ������ ������ ������Ʈ�ϴ� �޼���
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
                    color.a = 1f; 
                    image.color = color;
                }
            }
        }
    }

    // �����ۿ� ���� �ؽ�Ʈ ������Ʈ
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

    // ���� �̹���, ���, ��� ������Ʈ
    public void UpdateWeapon(Image image)
    {
        selectedItemName = image.name;

        selectedItemColor = image.color;
        selectedItemColor.a = 1f;
        WeaponImg.color = selectedItemColor;

        WeaponNameText.text = selectedItemName;

        UpdateText();
    }

    // ���� ��� ������Ʈ
    public void UpdateText()
    {
        foreach (var item in items)
        {
            if (item.Name == selectedItemName)
            {
                WeaponGradeText.text = item.Grade;
                ReactionEffectText.text = $"���ݷ� + {TextFormatter.FormatText_F(item.ReactionEffect * 100)}%";
                EquipEffectText.text = $"���ݷ� + {TextFormatter.FormatText_F(item.EquipEffect * 100)}%";
                WeaponLevelText.text = $"Lv.{item.Level}";
                WeaponCountText.text = $"( {item.Count} / {item.RequiredCount} )";
                break;
            }
        }
    }

    // ��� ����
    public void UpdateSelected()
    {
        // ���� ������ ���� ��������
        int count = GetItemCount(selectedItemName);
        
        // ���� ������ ã��
        CSVReader.Item selectedItem = items.Find(item => item.Name == selectedItemName);

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
                    item.ReactionEffect += 0.0146f;
                    item.EquipEffect += item.EquipEffect / 5;
                    item.RequiredCount += 2;

                    // ����ȿ�� ������Ʈ
                    playerstatus.UpdateReactionEffects(items);

                    // ����ȿ�� ������Ʈ
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
