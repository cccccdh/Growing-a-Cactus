using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetManager : MonoBehaviour
{
    public GameObject Pet;
    public TextMeshProUGUI PetName;

    public Image PetImage;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI GradeText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI RetentionEffect;

    public Image[] petImages;
    public TextMeshProUGUI[] petCountTexts;
    public TextMeshProUGUI[] petLevelTexts;

    public PlayerStatus playerstatus;

    private List<Pet> pets = new List<Pet>();
    private string selectedPetName;
    private Color selectedPetColor;

    private void Awake()
    {
        playerstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        Pet.SetActive(false);
    }

    // 펫 리스트 설정
    public void SetItems(List<Pet> petList)
    {
        pets = petList;
        //playerstatus.UpdateReactionEffects(items);
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
                //RetentionEffect.text = $"공격력 + {TextFormatter.FormatText_F(pet.RetentionEffect * 100)}%";
                //EquipEffectText.text = $"공격력 + {TextFormatter.FormatText_F(pet.EquipEffect * 100)}%";
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

                    Pet.SetActive(true);
                    PetName.text = pet.Name;
                    break;
                }
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
                    // 펫 보유효과 증가
                    // 펫 장착효과 증가
                    pet.RequiredCount += 2;

                    // 보유효과 업데이트
                    // 장착효과 업데이트

                    UpdatePetText(pet.Name);
                    InfomationText();

                    break;
                }
            }
        }
    }
}
