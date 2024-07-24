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

    // ������ ������ ������Ʈ�ϴ� �޼���
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

    // ���� �̹���, ���, ��� ������Ʈ
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
                ReactionEffectText.text = $"���ݷ� + {item.ReactionEffect * 100}%";
                EquipEffectText.text = $"���ݷ� + {item.EquipEffect * 100}%";
                break;
            }
        }
    }

    // ������ ����
    public void UpdateEquip()
    {
        // ���� ������ ���� ��������
        int count = GetItemCount(selectedItemName);

        if(count > 0)
        {
            // ���� ������ �� �ٲٱ�
            EquipWeaponImg.color = selectedItemColor;

            // ���� ������ �̸� �ٲٱ�
            EquipWeaponText.text = selectedItemName;

            Update_PowerLevel();
        }
    }

    // ������ ������Ʈ �޼���
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
