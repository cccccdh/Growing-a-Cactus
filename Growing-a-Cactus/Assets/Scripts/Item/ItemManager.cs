using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [Header ("���� ���� UI ���")]
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
    public Image[] weaponImages; // ���� ������ �迭
    public TextMeshProUGUI[] weaponCountTexts; // ���� ���� �ؽ�Ʈ �迭
    public TextMeshProUGUI[] weaponLevelTexts; // ���� ���� �ؽ�Ʈ �迭

    [Header("�� ���� UI ���")]
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
    public Image[] armorImages; // �� ������ �迭
    public TextMeshProUGUI[] armorCountTexts; // �� ���� �ؽ�Ʈ �迭
    public TextMeshProUGUI[] armorLevelTexts; // �� ���� �ؽ�Ʈ �迭

    [HideInInspector]
    public List<Item> weaponItems = new List<Item>(); // ���� ������ ����Ʈ
    [HideInInspector]
    public List<Item> armorItems = new List<Item>(); // �� ������ ����Ʈ

    [Header ("��ũ��Ʈ ����")]
    public PlayerStatus playerstatus; // �÷��̾� ���� ����

    [HideInInspector]
    public string selectedItemName; // ���õ� �������� �̸�
    [HideInInspector]
    public Color selectedItemColor; // ���õ� �������� ����

    // UI ������Ʈ �Լ�
    public void UpdateUI()
    {
        // TODO: ���� �� �� UI ������Ʈ ���� �߰�
    }

    // ������ ����Ʈ�� �����ϴ� �޼���
    public void SetItems(List<Item> itemList)
    {
        weaponItems.Clear(); // ���� ����Ʈ �ʱ�ȭ
        armorItems.Clear(); // �� ����Ʈ �ʱ�ȭ

        // �־��� ������ ����Ʈ���� ����� ���� �����Ͽ� ������ ����Ʈ�� �߰�
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
        // �÷��̾� ������ ���� �� �� ���� ȿ�� ������Ʈ
        playerstatus.UpdateWeaponRetentionEffects(weaponItems);
        playerstatus.UpdateArmorRetentionEffects(armorItems);
    }

    // ������ ������ ������Ʈ�ϴ� �޼���
    public void UpdateItemCount(string itemName)
    {
        // ���� ����Ʈ�� �������� �����ϴ� ���, �ش� �������� ������ ������Ʈ
        if (weaponItems.Exists(item => item.Name == itemName))
        {
            UpdateItemCountInList(weaponItems, itemName);
        }
        // �� ����Ʈ�� �������� �����ϴ� ���, �ش� �������� ������ ������Ʈ
        else if (armorItems.Exists(item => item.Name == itemName))
        {
            UpdateItemCountInList(armorItems, itemName);
        }
    }

    // ������ ����Ʈ���� ������ ������Ʈ�ϰ� �ؽ�Ʈ�� �����ϴ� �޼���
    public void UpdateItemCountInList(List<Item> items, string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                item.Count++; // ������ ���� ����
                UpdateItemText(itemName, items); // �ؽ�Ʈ ������Ʈ
                playerstatus.UpdateWeaponRetentionEffects(weaponItems); // ���� ���� ȿ�� ������Ʈ
                playerstatus.UpdateArmorRetentionEffects(armorItems); // �� ���� ȿ�� ������Ʈ
                break;
            }
        }
    }

    // ������ ������ �������� �޼���
    public int GetItemCount(string itemName)
    {
        int count = GetItemCountInList(weaponItems, itemName); // ���� ����Ʈ���� ���� ��������
        if (count == 0)
        {
            count = GetItemCountInList(armorItems, itemName); // �� ����Ʈ���� ���� ��������
        }
        return count;
    }

    private int GetItemCountInList(List<Item> items, string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                return item.Count; // ������ ���� ��ȯ
            }
        }
        return 0; // �������� ����Ʈ�� ������ 0 ��ȯ
    }

    // ������ ��ȭ�� �ʿ��� ������ �������� �޼���
    public int GetItemRequiredCount(string itemName)
    {
        int requiredCount = GetItemRequiredCountInList(weaponItems, itemName); // ���� ����Ʈ���� �ʿ� ���� ��������
        if (requiredCount == 0)
        {
            requiredCount = GetItemRequiredCountInList(armorItems, itemName); // �� ����Ʈ���� �ʿ� ���� ��������
        }
        return requiredCount;
    }

    private int GetItemRequiredCountInList(List<Item> items, string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                return item.RequiredCount; // �������� ��ȭ�� �ʿ��� ���� ��ȯ
            }
        }
        return 0; // �������� ����Ʈ�� ������ 0 ��ȯ
    }

    // ������ ������ �������� �޼���
    public int GetItemLevel(string itemName)
    {
        int level = GetItemLevelInList(weaponItems, itemName); // ���� ����Ʈ���� ���� ��������
        if (level == 0)
        {
            level = GetItemLevelInList(armorItems, itemName); // �� ����Ʈ���� ���� ��������
        }
        return level;
    }

    public int GetItemLevelInList(List<Item> items, string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                return item.Level; // �������� ���� ��ȯ
            }
        }
        return 0; // �������� ����Ʈ�� ������ 0 ��ȯ
    }

    // ���â�� ������ �̹����� ������Ʈ�ϴ� �޼���
    public void UpdateItemImages(List<Item> resultItemList)
    {
        UpdateItemImagesInList(resultItemList, weaponImages); // ���� �̹��� ������Ʈ
        UpdateItemImagesInList(resultItemList, armorImages); // �� �̹��� ������Ʈ
    }

    public void UpdateItemImagesInList(List<Item> resultItemList, Image[] images)
    {
        foreach (var result in resultItemList)
        {
            foreach (var image in images)
            {
                if (image.name == result.Name)
                {
                    Color color = image.color;
                    color.a = 1f; // �̹����� ���� ���� 1�� �����Ͽ� ���̰� ��
                    image.color = color;
                }
            }
        }
    }


    // �����ۿ� ���� UI �ؽ�Ʈ�� ������Ʈ�ϴ� �޼���
    public void UpdateItemText(string itemName, List<Item> items)
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (weaponImages[i].name == itemName)
            {
                int count = GetItemCount(itemName); // ������ ���� ��������
                int requiredcount = GetItemRequiredCount(itemName); // �ʿ��� ���� ��������
                int level = GetItemLevel(itemName); // ������ ���� ��������
                weaponCountTexts[i].text = $"({count}/{requiredcount})"; // ���� �ؽ�Ʈ ������Ʈ
                weaponLevelTexts[i].text = $"Lv.{level}"; // ���� �ؽ�Ʈ ������Ʈ
                break;
            }
        }
        for (int i = 0; i < armorImages.Length; i++)
        {
            if (armorImages[i].name == itemName)
            {
                int count = GetItemCount(itemName); // ������ ���� ��������
                int requiredcount = GetItemRequiredCount(itemName); // �ʿ��� ���� ��������
                int level = GetItemLevel(itemName); // ������ ���� ��������
                armorCountTexts[i].text = $"({count}/{requiredcount})"; // ���� �ؽ�Ʈ ������Ʈ
                armorLevelTexts[i].text = $"Lv.{level}"; // ���� �ؽ�Ʈ ������Ʈ
                break;
            }
        }
    }

    // ���õ� �������� �̹����� ������Ʈ�ϰ� �ش� �������� ������ UI�� ǥ��
    public void SelectedItem(Image image)
    {
        selectedItemName = image.name;
        selectedItemColor = image.color;
        selectedItemColor.a = 1f; // ���õ� �������� ���� ���� ���� 1�� ����

        // ���õ� �������� ���⳪ �� ����Ʈ���� ã��
        Item selectedItem = weaponItems.Find(item => item.Name == selectedItemName)
                        ?? armorItems.Find(item => item.Name == selectedItemName);

        if (selectedItem == null) return; // �������� ������ ��ȯ

        if (selectedItem.Type == "����")
        {
            WeaponImg.color = selectedItemColor; // ���� �̹��� ���� ������Ʈ
            WeaponNameText.text = selectedItemName; // ���� �̸� �ؽ�Ʈ ������Ʈ
            UpdateWeaponInfo(selectedItem); // ���� ���� ������Ʈ
        }
        else if (selectedItem.Type == "��")
        {
            ArmorImg.color = selectedItemColor; // �� �̹��� ���� ������Ʈ
            ArmorNameText.text = selectedItemName; // �� �̸� �ؽ�Ʈ ������Ʈ
            UpdateArmorInfo(selectedItem); // �� ���� ������Ʈ
        }
    }

    // ���� ������ UI�� ������Ʈ�ϴ� �޼���
    public void UpdateWeaponInfo(Item item)
    {
        WeaponGradeText.text = item.Grade; // ���� ��� �ؽ�Ʈ ������Ʈ
        WeaponRetentionEffect.text = $"���ݷ� + {TextFormatter.FormatText(item.RetentionEffect * 100)}%"; // ���� ȿ�� �ؽ�Ʈ ������Ʈ
        WeaponEquipEffectText.text = $"���ݷ� + {TextFormatter.FormatText(item.EquipEffect * 100)}%"; // ���� ȿ�� �ؽ�Ʈ ������Ʈ
        WeaponLevelText.text = $"Lv.{item.Level}"; // ���� ���� �ؽ�Ʈ ������Ʈ
        WeaponCountText.text = $"( {item.Count} / {item.RequiredCount} )"; // ���� ���� �ؽ�Ʈ ������Ʈ
    }

    // �� ������ UI�� ������Ʈ�ϴ� �޼���
    public void UpdateArmorInfo(Item item)
    {
        ArmorGradeText.text = item.Grade; // �� ��� �ؽ�Ʈ ������Ʈ
        ArmorRetentionEffect.text = $"ü�� + {TextFormatter.FormatText(item.RetentionEffect * 100)}%"; // ���� ȿ�� �ؽ�Ʈ ������Ʈ
        ArmorEquipEffectText.text = $"ü�� + {TextFormatter.FormatText(item.EquipEffect * 100)}%"; // ���� ȿ�� �ؽ�Ʈ ������Ʈ
        ArmorLevelText.text = $"Lv.{item.Level}"; // �� ���� �ؽ�Ʈ ������Ʈ
        ArmorCountText.text = $"( {item.Count} / {item.RequiredCount} )"; // �� ���� �ؽ�Ʈ ������Ʈ
    }

    // ���õ� �������� �����ϴ� �޼���
    public void EquipItem()
    {
        int count = GetItemCount(selectedItemName); // ���õ� �������� ���� ��������

        // ���õ� �������� ���⳪ �� ����Ʈ���� ã��
        Item selectedItem = weaponItems.Find(item => item.Name == selectedItemName)
            ?? armorItems.Find(item => item.Name == selectedItemName);

        if (selectedItem != null && count > 0 || selectedItem.Level > 1)
        {
            if (selectedItem.Type == "����")
            {
                EquipWeaponImg.color = selectedItemColor; // ������ ���� �̹��� ���� ������Ʈ
                EquipWeaponText.text = selectedItemName; // ������ ���� �̸� �ؽ�Ʈ ������Ʈ
                EquipWeaponLevelText.text = $"Lv.{selectedItem.Level}"; // ������ ���� ���� �ؽ�Ʈ ������Ʈ

                // �÷��̾�� ���� ���� ȿ�� �ο�
                playerstatus.EquipWeapon(selectedItem);
            }
            else if (selectedItem.Type == "��")
            {
                EquipArmorImg.color = selectedItemColor; // ������ �� �̹��� ���� ������Ʈ
                EquipArmorText.text = selectedItemName; // ������ �� �̸� �ؽ�Ʈ ������Ʈ
                EquipArmorLevelText.text = $"Lv.{selectedItem.Level}"; // ������ �� ���� �ؽ�Ʈ ������Ʈ

                // �÷��̾�� �� ���� ȿ�� �ο�
                playerstatus.EquipArmor(selectedItem);
            }
        }
    }

    // ���õ� �������� ��ȭ�ϴ� �޼���
    public void EnhanceItem()
    {
        // ���� ����Ʈ�� ���õ� �������� �����ϴ� ��� ��ȭ
        if (weaponItems.Exists(item => item.Name == selectedItemName))
        {
            EnhanceItemInList(weaponItems);
        }
        // �� ����Ʈ�� ���õ� �������� �����ϴ� ��� ��ȭ
        else if (armorItems.Exists(item => item.Name == selectedItemName))
        {
            EnhanceItemInList(armorItems);
        }
    }

    // ������ ����Ʈ���� ���õ� �������� ��ȭ�ϴ� �޼���
    public void EnhanceItemInList(List<Item> items)
    {
        foreach (var item in items)
        {
            if (item.Name == selectedItemName)
            {
                if (item.Count >= item.RequiredCount) // ��ȭ�� �ʿ��� ������ ����� ���
                {
                    item.Count -= item.RequiredCount; // ������ ���� ����
                    item.Level++; // ������ ���� ����

                    if (item.Type == "����")
                    {
                        item.RetentionEffect += 0.0598f; // ���� ȿ�� ����
                        item.EquipEffect += item.EquipEffect / 5; // ���� ȿ�� ����
                        item.RequiredCount += 2; // ��ȭ�� �ʿ��� ���� ����

                        // ���� ���� ȿ�� ������Ʈ
                        playerstatus.UpdateWeaponRetentionEffects(items);

                        // ������ ���� ȿ�� ������Ʈ
                        if (playerstatus.GetEquippedWeapon() != null && playerstatus.GetEquippedWeapon().Name == item.Name)
                        {
                            playerstatus.EquipWeapon(item);
                        }

                        UpdateWeaponInfo(item); // ���� ���� UI ������Ʈ
                    }
                    else if(item.Type == "��")
                    {
                        item.RetentionEffect += 0.0355f; // ���� ȿ�� ����
                        item.EquipEffect += item.EquipEffect / 10; // ���� ȿ�� ����
                        item.RequiredCount += 2; // ��ȭ�� �ʿ��� ���� ����

                        // �� ���� ȿ�� ������Ʈ
                        playerstatus.UpdateArmorRetentionEffects(items);

                        // ������ �� ȿ�� ������Ʈ
                        if (playerstatus.GetEquippedArmor() != null && playerstatus.GetEquippedArmor().Name == item.Name)
                        {
                            playerstatus.EquipArmor(item);
                        }

                        UpdateArmorInfo(item); // �� ���� UI ������Ʈ
                    }

                    UpdateItemText(item.Name, items); // ������ �ؽ�Ʈ ������Ʈ

                    if (EquipWeaponText.text == selectedItemName || EquipArmorText.text == selectedItemName)
                    {
                        EquipItem(); // ������ ������ UI ������Ʈ
                    }
                }
                break;
            }
        }
    }
}
