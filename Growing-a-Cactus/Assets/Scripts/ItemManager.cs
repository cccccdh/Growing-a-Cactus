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
    public TextMeshProUGUI WeaponLevelText;
    public TextMeshProUGUI WeaponCountText;
    public TextMeshProUGUI ReactionEffectText;
    public TextMeshProUGUI EquipEffectText;
    public Image[] weaponImages;
    public TextMeshProUGUI[] weaponCountTexts;

    public PlayerStatus playerstatus;
    public EnhancementManager enhancementManager;

    private List<CSVReader.Item> items = new List<CSVReader.Item>();
    private string selectedItemName;
    private Color selectedItemColor;

    private void Awake()
    {
        playerstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        enhancementManager = FindObjectOfType<EnhancementManager>();
    }

    public void SetItems(List<CSVReader.Item> itemList)
    {
        items = itemList;
        playerstatus.UpdateReactionEffects(items);
    }

    public List<CSVReader.Item> GetItems()
    {
        return items;
    }

    // ������ ������ ������Ʈ�ϴ� �޼���
    public void UpdateItemCount(string itemName)
    {
        foreach(var item in items)
        {
            if(item.Name == itemName)
            {
                item.Count++;
                UpdateWeaponCountText(itemName);
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

    // ������ ������ ���� �ؽ�Ʈ ������Ʈ
    public void UpdateWeaponCountText(string itemName)
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (weaponImages[i].name == itemName)
            {
                int count = GetItemCount(itemName);
                int requredcount = GetItemRequiredCount(itemName);
                weaponCountTexts[i].text = $"({count}/{requredcount})";
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

    public void UpdateText()
    {
        foreach (var item in items)
        {
            if (item.Name == selectedItemName)
            {
                WeaponGradeText.text = item.Grade;
                ReactionEffectText.text = $"���ݷ� + {item.ReactionEffect * 100}%";
                EquipEffectText.text = $"���ݷ� + {item.EquipEffect * 100}%";
                WeaponLevelText.text = $"Lv.{item.Level}";
                WeaponCountText.text = $"( {item.Count} / {item.RequiredCount} )";
                break;
            }
        }
    }

    // ���� ������ Ȯ��
    public void UpdateSelected()
    {
        Debug.Log("����");
        // ���� ������ ���� ��������
        int count = GetItemCount(selectedItemName);

        if (count > 0)
        {
            // ���� ������ �� �ٲٱ�
            EquipWeaponImg.color = selectedItemColor;

            // ���� ������ �̸� �ٲٱ�
            EquipWeaponText.text = selectedItemName;

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
        Debug.Log("��ȭ ��ư ����");
        enhancementManager.EnhanceItemByName(selectedItemName);
    }
}
