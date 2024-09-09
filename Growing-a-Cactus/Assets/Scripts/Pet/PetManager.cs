using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetManager : MonoBehaviour
{
    [Header("Pet")]
    public GameObject Pet;
    public TextMeshProUGUI PetName;

    [Header("Information")]
    public Image PetImage;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI GradeText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI RetentionEffect;
    public TextMeshProUGUI EquipEffectText;

    [Header("Scroll VIew")]
    public Image[] petImages;
    public TextMeshProUGUI[] petCountTexts;
    public TextMeshProUGUI[] petLevelTexts;

    [Header("장착 중 텍스트")]
    public GameObject prefabs;
    private GameObject equippedTextObject;

    [Header("스크립트 참조")]
    public PlayerStatus playerstatus;

    [HideInInspector] public List<Pet> pets = new List<Pet>();

    [HideInInspector] public string selectedPetName;
    [HideInInspector] public Color selectedPetColor;

    public void Awake()
    {
        playerstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        Pet.SetActive(false);
    }

    // 펫 리스트 설정
    public void SetPets(List<Pet> petList)
    {
        pets = petList;
        playerstatus.UpdatePetRetentionEffects(pets);
    }
    
    // 펫 개수를 업데이트하는 함수
    public void UpdatePetCount(string petName)
    {
        foreach (var pet in pets)
        {
            if(pet.Name == petName)
            {
                pet.Count++;
                UpdatePetText(petName);
                playerstatus.UpdatePetRetentionEffects(pets);
                break;
            }
        }
    }

    // 펫 보유개수를 가져오는 함수
    public int GetPetCount(string petName)
    {
        foreach(var pet in pets)
        {
            if(petName == pet.Name)
            {
                return pet.Count;
            }
        }
        return 0;
    }

    // 펫 강화 개수를 가져오는 함수
    public int GetPetRequiredCount(string petName)
    {
        foreach(var pet in pets)
        {
            if(pet.Name == petName)
            {
                return pet.RequiredCount;
            }
        }
        return 0;
    }

    // 펫 레벨을 가져오는 함수
    public int GetPetLevel(string petName)
    {
        foreach(var pet in pets)
        {
            if(pet.Name == petName)
            {
                return pet.Level;
            }
        }
        return 0;
    }

    // 펫 색상 값 가져오기
    public Color GetColorForPet(string petName)
    {
        for (int i = 0; i < petImages.Length; i++)
        {
            if (petImages[i].name == petName)
            {
                Color color = petImages[i].color;
                return color;
            }
        }
        return Color.white; 
    }

    // 펫 획득 시 펫 페이지 업데이트
    public void UpdatePetImages(List<Pet> resultPetList)
    {
        foreach(var result in resultPetList)
        {
            foreach(var image in petImages)
            {
                if(image.name == result.Name)
                {
                    // 알파값 변경
                    Color color = image.color;
                    color.a = 1f;
                    image.color = color;
                }
            }
        }
    }

    // 펫에 따라 텍스트 업데이트
    public void UpdatePetText(string petName)
    {
        for(int i = 0; i < petImages.Length; i++)
        {
            if (petImages[i].name == petName)
            {
                int count = GetPetCount(petName);
                int requiredcount = GetPetRequiredCount(petName);
                int level = GetPetLevel(petName);
                petCountTexts[i].text = $"({count}/{requiredcount})";
                petLevelTexts[i].text = $"Lv.{level}";
                break;
            }
        }
    }

    // 선택 펫 이미지, 등급, 텍스트 업데이트
    public void SelectedPet(Image image)
    {
        selectedPetName = image.name;

        selectedPetColor = image.color;
        selectedPetColor.a = 1f;
        PetImage.color = selectedPetColor;

        NameText.text = selectedPetName;

        InfomationText();
    }

    // 선택 펫 업데이트
    public void InfomationText()
    {
        foreach(var pet in pets)
        {
            if(pet.Name == selectedPetName)
            {
                GradeText.text = pet.Grade;
                RetentionEffect.text = $"공격력 + {TextFormatter.FormatText(pet.RetentionEffect * 100)}%";
                EquipEffectText.text = $"공격력 + {TextFormatter.FormatText(pet.EquipEffect * 100)}%";
                LevelText.text = $"Lv.{pet.Level}";
                CountText.text = $"( {pet.Count} / {pet.RequiredCount} )";
                break;
            }
        }
    }

    // 펫 장착
    public void EquipPet()
    {
        int count = GetPetCount(selectedPetName);

        Pet selectedPet = pets.Find(pet => pet.Name == selectedPetName);

        if(count > 0 || selectedPet.Level > 1)
        {
            foreach(var pet in pets)
            {
                if(pet.Name == selectedPetName)
                {   
                    // 플레이어 전투력에 장착효과 부여
                    playerstatus.EquipPet(pet);
                    
                    Pet.SetActive(true);
                    PetName.text = pet.Name;

                    ShowEquippedText(selectedPetName);
                    break;
                }
            }
        }
    }

    // 장착 중 텍스트 표시
    private void ShowEquippedText(string selectedPetName)
    {
        if (equippedTextObject != null)
        {
            Destroy(equippedTextObject);
        }

        for (int i = 0; i < petImages.Length; i++)
        {
            if (petImages[i].name == selectedPetName)
            {
                // 새로운 "장착중" 텍스트 생성
                equippedTextObject = Instantiate(prefabs);
                equippedTextObject.transform.SetParent(petImages[i].transform, false); // 부모를 설정하면서 World Position 유지하지 않음

                // RectTransform 설정
                RectTransform rectTransform = equippedTextObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(35, -20); // 원하는 위치로 설정
                break;
            }
        }
    }

    // 펫 강화
    public void EnhancePet()
    {
        foreach(var pet in pets)
        {
            if(pet.Name == selectedPetName)
            {
                if(pet.Count >= pet.RequiredCount)
                {
                    pet.Count -= pet.RequiredCount;
                    pet.Level++;
                    pet.RetentionEffect += 0.093f;
                    pet.EquipEffect += pet.EquipEffect / 5;
                    pet.RequiredCount += 2;

                    // 보유효과 업데이트
                    playerstatus.UpdatePetRetentionEffects(pets);

                    // 장착효과 업데이트
                    if (playerstatus.GetEquippedPet() != null && playerstatus.GetEquippedPet().Name == pet.Name)
                    {
                        playerstatus.EquipPet(pet);
                    }
                    UpdatePetText(pet.Name);
                    InfomationText();

                    break;
                }
            }
        }
    }
}
