using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [Header ("무기 관련 UI 요소")]
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
    public Image[] weaponImages; // 무기 아이콘 배열
    public TextMeshProUGUI[] weaponCountTexts; // 무기 개수 텍스트 배열
    public TextMeshProUGUI[] weaponLevelTexts; // 무기 레벨 텍스트 배열
    public Button weaponEquipButton;
    public Button weaponEnhanceButton;

    [Header("방어구 관련 UI 요소")]
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
    public Image[] armorImages; // 방어구 아이콘 배열
    public TextMeshProUGUI[] armorCountTexts; // 방어구 개수 텍스트 배열
    public TextMeshProUGUI[] armorLevelTexts; // 방어구 레벨 텍스트 배열
    public Button armorEquipButton;
    public Button armorEnhanceButton;

    [Header("장착 중 텍스트")]
    public GameObject prefabs;
    [HideInInspector] public GameObject weaponEquippedObject; // 무기 장착 중 오브젝트
    [HideInInspector] public GameObject armorEquippedObject; // 방어구 장착 중 오브젝트

    [HideInInspector] public List<Item> weaponItems = new List<Item>(); // 무기 아이템 리스트
    [HideInInspector] public List<Item> armorItems = new List<Item>(); // 방어구 아이템 리스트

    [Header ("스크립트 참조")]
    public PlayerStatus playerstatus; // 플레이어 상태 참조

    [HideInInspector] public string selectedItemName; // 선택된 아이템의 이름
    [HideInInspector] public Color selectedItemColor; // 선택된 아이템의 색상

    private void Start()
    {
        Initialize();       
    }

    private void Initialize()
    {
        // 버튼 상태 초기화
        weaponEquipButton.interactable = false;
        weaponEnhanceButton.interactable = false;
        armorEquipButton.interactable = false;
        armorEnhanceButton.interactable = false;
    }


    // 아이템 리스트를 설정하는 메서드
    public void SetItems(List<Item> itemList)
    {
        weaponItems.Clear(); // 무기 리스트 초기화
        armorItems.Clear(); // 방어구 리스트 초기화

        // 주어진 아이템 리스트에서 무기와 방어구를 구분하여 각각의 리스트에 추가
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
        // 플레이어 상태의 무기 및 방어구 보유 효과 업데이트
        playerstatus.UpdateWeaponRetentionEffects(weaponItems);
        playerstatus.UpdateArmorRetentionEffects(armorItems);
    }

    // 아이템 개수를 업데이트하는 메서드
    public void UpdateItemCount(string itemName)
    {
        // 무기 리스트에 아이템이 존재하는 경우, 해당 아이템의 개수를 업데이트
        if (weaponItems.Exists(item => item.Name == itemName))
        {
            UpdateItemCountInList(weaponItems, itemName);
        }
        // 방어구 리스트에 아이템이 존재하는 경우, 해당 아이템의 개수를 업데이트
        else if (armorItems.Exists(item => item.Name == itemName))
        {
            UpdateItemCountInList(armorItems, itemName);
        }
    }

    // 아이템 리스트에서 개수를 업데이트하고 텍스트를 갱신하는 메서드
    public void UpdateItemCountInList(List<Item> items, string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                item.Count++; // 아이템 개수 증가
                UpdateItemText(itemName, items); // 텍스트 업데이트
                playerstatus.UpdateWeaponRetentionEffects(weaponItems); // 무기 보유 효과 업데이트
                playerstatus.UpdateArmorRetentionEffects(armorItems); // 방어구 보유 효과 업데이트
                break;
            }
        }
    }

    // 아이템 개수를 가져오는 메서드
    public int GetItemCount(string itemName)
    {
        int count = GetItemCountInList(weaponItems, itemName); // 무기 리스트에서 개수 가져오기
        if (count == 0)
        {
            count = GetItemCountInList(armorItems, itemName); // 방어구 리스트에서 개수 가져오기
        }
        return count;
    }

    private int GetItemCountInList(List<Item> items, string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                return item.Count; // 아이템 개수 반환
            }
        }
        return 0; // 아이템이 리스트에 없으면 0 반환
    }

    // 아이템 강화에 필요한 개수를 가져오는 메서드
    public int GetItemRequiredCount(string itemName)
    {
        int requiredCount = GetItemRequiredCountInList(weaponItems, itemName); // 무기 리스트에서 필요 개수 가져오기
        if (requiredCount == 0)
        {
            requiredCount = GetItemRequiredCountInList(armorItems, itemName); // 방어구 리스트에서 필요 개수 가져오기
        }
        return requiredCount;
    }

    private int GetItemRequiredCountInList(List<Item> items, string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                return item.RequiredCount; // 아이템의 강화에 필요한 개수 반환
            }
        }
        return 0; // 아이템이 리스트에 없으면 0 반환
    }

    // 아이템 레벨을 가져오는 메서드
    public int GetItemLevel(string itemName)
    {
        int level = GetItemLevelInList(weaponItems, itemName); // 무기 리스트에서 레벨 가져오기
        if (level == 0)
        {
            level = GetItemLevelInList(armorItems, itemName); // 방어구 리스트에서 레벨 가져오기
        }
        return level;
    }

    public int GetItemLevelInList(List<Item> items, string itemName)
    {
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                return item.Level; // 아이템의 레벨 반환
            }
        }
        return 0; // 아이템이 리스트에 없으면 0 반환
    }

    // 무기 아이템 색상 값 가져오기
    public Color GetColorForWeapon(string weaponName)
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (weaponImages[i].name == weaponName)
            {
                Color color = weaponImages[i].color;
                return color;
            }
        }
        return Color.white; 
    }

    // 방어구 아이템 색상 값 가져오기
    public Color GetColorForArmor(string armorName)
    {
        for (int i = 0; i < armorImages.Length; i++)
        {
            if (armorImages[i].name == armorName)
            {
                Color color = armorImages[i].color;
                return color;
            }
        }
        return Color.white; 
    }

    // 장비창의 아이템 이미지를 업데이트하는 메서드
    public void UpdateItemImages(List<Item> resultItemList)
    {
        UpdateItemImagesInList(resultItemList, weaponImages); // 무기 이미지 업데이트
        UpdateItemImagesInList(resultItemList, armorImages); // 방어구 이미지 업데이트
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
                    color.a = 1f; // 이미지의 알파 값을 1로 설정하여 보이게 함
                    image.color = color;
                }
            }
        }
    }

    // 아이템에 따라 UI 텍스트를 업데이트하는 메서드
    public void UpdateItemText(string itemName, List<Item> items)
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (weaponImages[i].name == itemName)
            {
                int count = GetItemCount(itemName); // 아이템 개수 가져오기
                int requiredcount = GetItemRequiredCount(itemName); // 필요한 개수 가져오기
                int level = GetItemLevel(itemName); // 아이템 레벨 가져오기
                weaponCountTexts[i].text = $"({count}/{requiredcount})"; // 개수 텍스트 업데이트
                weaponLevelTexts[i].text = $"Lv.{level}"; // 레벨 텍스트 업데이트
                break;
            }
        }
        for (int i = 0; i < armorImages.Length; i++)
        {
            if (armorImages[i].name == itemName)
            {
                int count = GetItemCount(itemName); // 아이템 개수 가져오기
                int requiredcount = GetItemRequiredCount(itemName); // 필요한 개수 가져오기
                int level = GetItemLevel(itemName); // 아이템 레벨 가져오기
                armorCountTexts[i].text = $"({count}/{requiredcount})"; // 개수 텍스트 업데이트
                armorLevelTexts[i].text = $"Lv.{level}"; // 레벨 텍스트 업데이트
                break;
            }
        }
    }

    // 선택된 아이템의 이미지를 업데이트하고 해당 아이템의 정보를 UI에 표시
    public void SelectedItem(Image image)
    {
        selectedItemName = image.name;
        selectedItemColor = image.color;
        selectedItemColor.a = 1f; // 선택된 아이템의 색상 알파 값을 1로 설정

        // 선택된 아이템을 무기나 방어구 리스트에서 찾음
        Item selectedItem = weaponItems.Find(item => item.Name == selectedItemName)
                        ?? armorItems.Find(item => item.Name == selectedItemName);

        if (selectedItem == null) return; // 아이템이 없으면 반환

        if (selectedItem.Type == "무기")
        {
            WeaponImg.color = selectedItemColor; // 무기 이미지 색상 업데이트
            WeaponNameText.text = selectedItemName; // 무기 이름 텍스트 업데이트
            UpdateWeaponInfo(selectedItem); // 무기 정보 업데이트
        }
        else if (selectedItem.Type == "방어구")
        {
            ArmorImg.color = selectedItemColor; // 방어구 이미지 색상 업데이트
            ArmorNameText.text = selectedItemName; // 방어구 이름 텍스트 업데이트
            UpdateArmorInfo(selectedItem); // 방어구 정보 업데이트
        }
    }

    // 무기 정보를 UI에 업데이트하는 메서드
    public void UpdateWeaponInfo(Item item)
    {
        WeaponGradeText.text = item.Grade; // 무기 등급 텍스트 업데이트
        WeaponRetentionEffect.text = $"공격력 + {TextFormatter.FormatText(item.RetentionEffect * 100)}%"; // 보유 효과 텍스트 업데이트
        WeaponEquipEffectText.text = $"공격력 + {TextFormatter.FormatText(item.EquipEffect * 100)}%"; // 장착 효과 텍스트 업데이트
        WeaponLevelText.text = $"Lv.{item.Level}"; // 무기 레벨 텍스트 업데이트
        WeaponCountText.text = $"( {item.Count} / {item.RequiredCount} )"; // 무기 개수 텍스트 업데이트

        // 버튼 상태 업데이트 
        int level = GetItemLevel(item.Name);
        int count = GetItemCount(item.Name);
        int requirecount = GetItemRequiredCount(item.Name);
        weaponEquipButton.interactable = (count > 0 || level > 1);
        weaponEnhanceButton.interactable = count >= requirecount;
    }

    // 방어구 정보를 UI에 업데이트하는 메서드
    public void UpdateArmorInfo(Item item)
    {
        ArmorGradeText.text = item.Grade; // 방어구 등급 텍스트 업데이트
        ArmorRetentionEffect.text = $"체력 + {TextFormatter.FormatText(item.RetentionEffect * 100)}%"; // 보유 효과 텍스트 업데이트
        ArmorEquipEffectText.text = $"체력 + {TextFormatter.FormatText(item.EquipEffect * 100)}%"; // 장착 효과 텍스트 업데이트
        ArmorLevelText.text = $"Lv.{item.Level}"; // 방어구 레벨 텍스트 업데이트
        ArmorCountText.text = $"( {item.Count} / {item.RequiredCount} )"; // 방어구 개수 텍스트 업데이트

        // 버튼 상태 업데이트 
        int level = GetItemLevel(item.Name);
        int count = GetItemCount(item.Name);
        int requirecount = GetItemRequiredCount(item.Name);
        armorEquipButton.interactable = (count > 0 || level > 1);
        armorEnhanceButton.interactable = count >= requirecount;
    }

    // 선택된 아이템을 장착하는 메서드
    public void EquipItem()
    {
        int count = GetItemCount(selectedItemName); // 선택된 아이템의 개수 가져오기

        // 선택된 아이템을 무기나 방어구 리스트에서 찾음
        Item selectedItem = weaponItems.Find(item => item.Name == selectedItemName)
            ?? armorItems.Find(item => item.Name == selectedItemName);

        if (selectedItem != null && (count > 0 || selectedItem.Level > 1))
        {
            if (selectedItem.Type == "무기")
            {
                EquipWeaponImg.color = selectedItemColor; // 장착된 무기 이미지 색상 업데이트
                EquipWeaponText.text = selectedItemName; // 장착된 무기 이름 텍스트 업데이트
                EquipWeaponLevelText.text = $"Lv.{selectedItem.Level}"; // 장착된 무기 레벨 텍스트 업데이트

                // 플레이어에게 무기 장착 효과 부여
                playerstatus.EquipWeapon(selectedItem);

                // 장착 중 텍스트 표시
                ShowEquippedText(selectedItem);
            }
            else if (selectedItem.Type == "방어구")
            {
                EquipArmorImg.color = selectedItemColor; // 장착된 방어구 이미지 색상 업데이트
                EquipArmorText.text = selectedItemName; // 장착된 방어구 이름 텍스트 업데이트
                EquipArmorLevelText.text = $"Lv.{selectedItem.Level}"; // 장착된 방어구 레벨 텍스트 업데이트

                // 플레이어에게 방어구 장착 효과 부여
                playerstatus.EquipArmor(selectedItem);

                // 장착 중 텍스트 표시
                ShowEquippedText(selectedItem);
            }
        }
    }

    public void ShowEquippedText(Item item)
    {
        // 장착 중인 무기 텍스트가 존재하면 삭제
        if (weaponEquippedObject != null && item.Type == "무기")
        {
            Destroy(weaponEquippedObject);
        }

        // 장착 중인 방어구 텍스트가 존재하면 삭제
        if (armorEquippedObject != null && item.Type == "방어구")
        {
            Destroy(armorEquippedObject);
        }

        // 무기 장착 중 텍스트 생성
        if (item.Type == "무기")
        {
            for (int i = 0; i < weaponImages.Length; i++)
            {
                if (weaponImages[i].name == item.Name)
                {
                    weaponEquippedObject = Instantiate(prefabs);
                    weaponEquippedObject.transform.SetParent(weaponImages[i].transform, false);

                    // RectTransform 설정
                    RectTransform rectTransform = weaponEquippedObject.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(35, -20); // 원하는 위치로 설정
                    break;
                }
            }            
        }
        // 방어구 장착 중 텍스트 생성
        else if (item.Type == "방어구")
        {
            for (int i = 0; i < armorImages.Length; i++)
            {
                if (armorImages[i].name == item.Name)
                {
                    armorEquippedObject = Instantiate(prefabs);
                    armorEquippedObject.transform.SetParent(armorImages[i].transform, false);

                    // RectTransform 설정
                    RectTransform rectTransform = armorEquippedObject.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(35, -20); // 원하는 위치로 설정
                    break;
                }
            }
        }
    }

    // 선택된 아이템을 강화하는 메서드
    public void EnhanceItem()
    {
        // 무기 리스트에 선택된 아이템이 존재하는 경우 강화
        if (weaponItems.Exists(item => item.Name == selectedItemName))
        {
            EnhanceItemInList(weaponItems);
        }
        // 방어구 리스트에 선택된 아이템이 존재하는 경우 강화
        else if (armorItems.Exists(item => item.Name == selectedItemName))
        {
            EnhanceItemInList(armorItems);
        }
    }

    // 아이템 리스트에서 선택된 아이템을 강화하는 메서드
    public void EnhanceItemInList(List<Item> items)
    {
        foreach (var item in items)
        {
            if (item.Name == selectedItemName)
            {
                if (item.Count >= item.RequiredCount) // 강화에 필요한 개수가 충분한 경우
                {
                    item.Count -= item.RequiredCount; // 아이템 개수 감소
                    item.Level++; // 아이템 레벨 증가

                    if (item.Type == "무기")
                    {
                        item.RetentionEffect += 0.0598f; // 보유 효과 증가
                        item.EquipEffect += item.EquipEffect / 5; // 장착 효과 증가
                        item.RequiredCount += 2; // 강화에 필요한 개수 증가

                        // 무기 보유 효과 업데이트
                        playerstatus.UpdateWeaponRetentionEffects(items);

                        // 장착된 무기 효과 업데이트
                        if (playerstatus.GetEquippedWeapon() != null && playerstatus.GetEquippedWeapon().Name == item.Name)
                        {
                            playerstatus.EquipWeapon(item);
                        }

                        UpdateWeaponInfo(item); // 무기 정보 UI 업데이트
                    }
                    else if(item.Type == "방어구")
                    {
                        item.RetentionEffect += 0.0355f; // 보유 효과 증가
                        item.EquipEffect += item.EquipEffect / 10; // 장착 효과 증가
                        item.RequiredCount += 2; // 강화에 필요한 개수 증가

                        // 방어구 보유 효과 업데이트
                        playerstatus.UpdateArmorRetentionEffects(items);

                        // 장착된 방어구 효과 업데이트
                        if (playerstatus.GetEquippedArmor() != null && playerstatus.GetEquippedArmor().Name == item.Name)
                        {
                            playerstatus.EquipArmor(item);
                        }

                        UpdateArmorInfo(item); // 방어구 정보 UI 업데이트
                    }

                    UpdateItemText(item.Name, items); // 아이템 텍스트 업데이트

                    if (EquipWeaponText.text == selectedItemName || EquipArmorText.text == selectedItemName)
                    {
                        EquipItem(); // 장착된 아이템 UI 업데이트
                    }
                }
                break;
            }
        }
    }

    public TextData GetTextData()
    {
        TextData textData = new TextData
        {
            weaponCountTexts = Array.ConvertAll(weaponCountTexts, text => text.text),
            weaponLevelTexts = Array.ConvertAll(weaponLevelTexts, text => text.text),
            armorCountTexts = Array.ConvertAll(armorCountTexts, text => text.text),
            armorLevelTexts = Array.ConvertAll(armorLevelTexts, text => text.text)
        };
        return textData;
    }

    public void SetTextData(TextData textData)
    {
        for (int i = 0; i < weaponCountTexts.Length; i++)
        {
            if (i < textData.weaponCountTexts.Length)
                weaponCountTexts[i].text = textData.weaponCountTexts[i];
        }
        for (int i = 0; i < weaponLevelTexts.Length; i++)
        {
            if (i < textData.weaponLevelTexts.Length)
                weaponLevelTexts[i].text = textData.weaponLevelTexts[i];
        }
        for (int i = 0; i < armorCountTexts.Length; i++)
        {
            if (i < textData.armorCountTexts.Length)
                armorCountTexts[i].text = textData.armorCountTexts[i];
        }
        for (int i = 0; i < armorLevelTexts.Length; i++)
        {
            if (i < textData.armorLevelTexts.Length)
                armorLevelTexts[i].text = textData.armorLevelTexts[i];
        }
    }
}
