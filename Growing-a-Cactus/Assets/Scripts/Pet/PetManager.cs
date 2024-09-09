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

    [Header("���� �� �ؽ�Ʈ")]
    public GameObject prefabs;
    private GameObject equippedTextObject;

    [Header("��ũ��Ʈ ����")]
    public PlayerStatus playerstatus;

    [HideInInspector] public List<Pet> pets = new List<Pet>();

    [HideInInspector] public string selectedPetName;
    [HideInInspector] public Color selectedPetColor;

    public void Awake()
    {
        playerstatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        Pet.SetActive(false);
    }

    // �� ����Ʈ ����
    public void SetPets(List<Pet> petList)
    {
        pets = petList;
        playerstatus.UpdatePetRetentionEffects(pets);
    }
    
    // �� ������ ������Ʈ�ϴ� �Լ�
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

    // �� ���������� �������� �Լ�
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

    // �� ��ȭ ������ �������� �Լ�
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

    // �� ������ �������� �Լ�
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

    // �� ���� �� ��������
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

    // �� ȹ�� �� �� ������ ������Ʈ
    public void UpdatePetImages(List<Pet> resultPetList)
    {
        foreach(var result in resultPetList)
        {
            foreach(var image in petImages)
            {
                if(image.name == result.Name)
                {
                    // ���İ� ����
                    Color color = image.color;
                    color.a = 1f;
                    image.color = color;
                }
            }
        }
    }

    // �꿡 ���� �ؽ�Ʈ ������Ʈ
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

    // ���� �� �̹���, ���, �ؽ�Ʈ ������Ʈ
    public void SelectedPet(Image image)
    {
        selectedPetName = image.name;

        selectedPetColor = image.color;
        selectedPetColor.a = 1f;
        PetImage.color = selectedPetColor;

        NameText.text = selectedPetName;

        InfomationText();
    }

    // ���� �� ������Ʈ
    public void InfomationText()
    {
        foreach(var pet in pets)
        {
            if(pet.Name == selectedPetName)
            {
                GradeText.text = pet.Grade;
                RetentionEffect.text = $"���ݷ� + {TextFormatter.FormatText(pet.RetentionEffect * 100)}%";
                EquipEffectText.text = $"���ݷ� + {TextFormatter.FormatText(pet.EquipEffect * 100)}%";
                LevelText.text = $"Lv.{pet.Level}";
                CountText.text = $"( {pet.Count} / {pet.RequiredCount} )";
                break;
            }
        }
    }

    // �� ����
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
                    // �÷��̾� �����¿� ����ȿ�� �ο�
                    playerstatus.EquipPet(pet);
                    
                    Pet.SetActive(true);
                    PetName.text = pet.Name;

                    ShowEquippedText(selectedPetName);
                    break;
                }
            }
        }
    }

    // ���� �� �ؽ�Ʈ ǥ��
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
                // ���ο� "������" �ؽ�Ʈ ����
                equippedTextObject = Instantiate(prefabs);
                equippedTextObject.transform.SetParent(petImages[i].transform, false); // �θ� �����ϸ鼭 World Position �������� ����

                // RectTransform ����
                RectTransform rectTransform = equippedTextObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(35, -20); // ���ϴ� ��ġ�� ����
                break;
            }
        }
    }

    // �� ��ȭ
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

                    // ����ȿ�� ������Ʈ
                    playerstatus.UpdatePetRetentionEffects(pets);

                    // ����ȿ�� ������Ʈ
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
