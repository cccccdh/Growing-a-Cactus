using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetManager : MonoBehaviour
{
    [Header("Pet")]
    public GameObject Pet;

    [Header("Information")]
    public Image PetBGImage;
    public Image PetImage;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI GradeText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI RetentionEffect;
    public TextMeshProUGUI EquipEffectText;

    public Button petEquipButton;
    public Button petEnhanceButton;

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
        Initialize();        
    }

    public void Initialize()
    {
        // �� UI �ʱ�ȭ
        Pet.gameObject.SetActive(false);

        // ��ư ���� �ʱ�ȭ
        petEquipButton.interactable = false;
        petEnhanceButton.interactable = false;

        // �ؽ�Ʈ �ʱ�ȭ        
        PetImage.sprite = ChangeSpriteWithPet("�䳢");
        NameText.text = "�䳢";
        GradeText.text = "�Ϲ�";
        LevelText.text = "Lv.1";
        CountText.text = "( 0 / 0 )";
        RetentionEffect.text = "���ݷ� + 7.3%";
        EquipEffectText.text = "���ݷ� + 24.7%";

        InitializePetImages();
    }

    // �� �̹��� �ʱ�ȭ
    private void InitializePetImages()
    {
        foreach (var image in petImages)
        {
            Transform child = image.transform.GetChild(3);
            child.gameObject.SetActive(true);
        }
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
                    Transform child = image.transform.GetChild(3);
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    // �� �̹��� ��ȯ �Լ�
    public Sprite ChangeSpriteWithPet(string name)
    {
        var sprite = Resources.Load<Sprite>($"_Pet/{name}");

        return sprite;
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

        PetBGImage.color = selectedPetColor;

        InfomationText();
    }

    // ���� �� ������Ʈ
    public void InfomationText()
    {
        foreach(var pet in pets)
        {
            if(pet.Name == selectedPetName)
            {
                PetImage.sprite = ChangeSpriteWithPet(selectedPetName);
                NameText.text = selectedPetName;
                GradeText.text = pet.Grade;
                RetentionEffect.text = $"���ݷ� + {TextFormatter.FormatText(pet.RetentionEffect * 100)}%";
                EquipEffectText.text = $"���ݷ� + {TextFormatter.FormatText(pet.EquipEffect * 100)}%";
                LevelText.text = $"Lv.{pet.Level}";
                CountText.text = $"( {pet.Count} / {pet.RequiredCount} )";

                int level = GetPetLevel(pet.Name);
                int count = GetPetCount(pet.Name);
                int requirecount = GetPetRequiredCount(pet.Name);
                petEquipButton.interactable = (count > 0 || level > 1);
                petEnhanceButton.interactable = count >= requirecount;
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
                    
                    Pet.gameObject.SetActive(true);
                    Pet.GetComponent<SpriteRenderer>().sprite = ChangeSpriteWithPet(selectedPetName);

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
                    pet.RetentionEffect += pet.RetentionEffect / 15;
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

    public PetTextData GetPetTextData()
    {
        PetTextData textData = new PetTextData
        {
            PetCountTexts = Array.ConvertAll(petCountTexts, text => text.text),
            PetLevelTexts = Array.ConvertAll(petLevelTexts, text => text.text),
        };
        return textData;
    }

    public void SetTextData(PetTextData textData)
    {
        for (int i = 0; i < petCountTexts.Length; i++)
        {
            if (i < textData.PetCountTexts.Length)
                petCountTexts[i].text = textData.PetCountTexts[i];
        }
        for (int i = 0; i < petLevelTexts.Length; i++)
        {
            if (i < textData.PetLevelTexts.Length)
                petLevelTexts[i].text = textData.PetLevelTexts[i];
        }
    }
}
