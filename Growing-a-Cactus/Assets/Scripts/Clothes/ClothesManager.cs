using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothesManager : MonoBehaviour
{
    public static ClothesManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Charater")]
    public TextMeshProUGUI equipClothesName;
    public TextMeshProUGUI equipProjectileName;

    [Header("Information")]
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ProjectileText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI RetentionEffectText;
    public Button clothesEquipButton;
    public Button clothesEnhanceButton;

    [Header("Scroll VIew")]
    public Image[] clothesImages;
    public TextMeshProUGUI[] clothesCountTexts;
    public TextMeshProUGUI[] clothesLevelTexts;

    [Header("���� �� �ؽ�Ʈ")]
    public GameObject prefabs;
    private GameObject equippedTextObject;

    [Header("����")]
    public PlayerStatus playerstatus;

    private List<Clothes> clothes = new List<Clothes>();
    private string selectedClothesName;
        
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        // �ؽ�Ʈ �ʱ�ȭ
        NameText.text = $"�⺻";
        ProjectileText.text = "����";
        RetentionEffectText.text = $"���ݷ� + 0.00%";
        
        // ��ư ���� �ʱ�ȭ
        clothesEquipButton.interactable = false;
        clothesEnhanceButton.interactable = false;
    }

    // �ǻ� ����Ʈ ����
    public void SetClothes(List<Clothes> clothesList)
    {
        clothes = clothesList;
        playerstatus.UpdateClothesRetentionEffects(clothesList);
    }

    // �ǻ� ������ ������Ʈ�ϴ� �Լ�
    public void UpdateClothesCount(string clothesName)
    {
        foreach (var cloth in clothes)
        {
            if (cloth.Name == clothesName)
            {
                cloth.Count++;
                UpdateClothesText(clothesName);
                // ����ȿ�� ���� �߰�
                playerstatus.UpdateClothesRetentionEffects(clothes);
                break;
            }
        }
    }

    // �ǻ� ���������� �������� �Լ�
    public int GetClothesCount(string clothesName)
    {
        foreach (var cloth in clothes)
        {
            if (cloth.Name == clothesName)
            {
                return cloth.Count;
            }
        }
        return 0;
    }

    // �ǻ� ��ȭ ������ �������� �Լ�
    public int GetClothesRequiredCount(string clothesName)
    {
        foreach (var cloth in clothes)
        {
            if (cloth.Name == clothesName)
            {
                return cloth.RequiredCount;
            }
        }
        return 0;
    }

    // �ǻ� ������ �������� �Լ�
    public int GetClothesLevel(string clothesName)
    {
        foreach (var cloth in clothes)
        {
            if (cloth.Name == clothesName)
            {
                return cloth.Level;
            }
        }
        return 0;
    }

    // �ǻ� ���� �� ��������
    public Color GetColorForClothes(string clothesName)
    {
        for (int i = 0; i < clothesImages.Length; i++)
        {
            if (clothesImages[i].name == clothesName)
            {
                Color color = clothesImages[i].color;
                return color;
            }
        }
        return Color.white;
    }

    // �ǻ� ȹ�� �� �ǻ� UI ������Ʈ
    public void UpdateClothesImages(List<Clothes> resultClothesList)
    {
        foreach (var result in resultClothesList)
        {
            foreach (var image in clothesImages)
            {
                if (image.name == result.Name)
                {
                    // ���İ� ����
                    Color color = image.color;
                    color.a = 1f;
                    image.color = color;
                }
            }
        }
    }

    // �ǻ� ���� �ؽ�Ʈ ������Ʈ
    public void UpdateClothesText(string clothesName)
    {
        for (int i = 0; i < clothesImages.Length; i++)
        {
            if (clothesImages[i].name == clothesName)
            {
                int count = GetClothesCount(clothesName);
                int requiredcount = GetClothesRequiredCount(clothesName);
                int level = GetClothesLevel(clothesName);
                clothesLevelTexts[i].text = $"Lv.{level}";
                clothesCountTexts[i].text = $"({count}/{requiredcount})";
                break;
            }
        }
    }

    // ���� �ǻ� �̸�, ����ü �ؽ�Ʈ ������Ʈ
    public void SelectedClothes(Image image)
    {
        selectedClothesName = image.name;

        NameText.text = selectedClothesName;

        InfomationText();
    }

    // ���� �ǻ� ������Ʈ
    public void InfomationText()
    {
        foreach (var cloth in clothes)
        {
            if (selectedClothesName == cloth.Name)
            {
                RetentionEffectText.text = $"���ݷ� + {TextFormatter.FormatText(cloth.RetentionEffect * 100)}%";
                LevelText.text = $"Lv.{cloth.Level}";
                CountText.text = $"( {cloth.Count} / {cloth.RequiredCount} )";
                ProjectileText.text = (cloth.Set == "Null" ? "����" : $"{cloth.Set}");

                int level = GetClothesLevel(cloth.Name);
                int count = GetClothesCount(cloth.Name);
                int requirecount = GetClothesRequiredCount(cloth.Name);
                clothesEquipButton.interactable = (count > 0 || level > 1);
                clothesEnhanceButton.interactable = count >= requirecount;
                break;
            }
        }
    }

    // �ǻ� ����
    public void EquipClothes()
    {
        int count = GetClothesCount(selectedClothesName);

        Clothes selectedClothes = clothes.Find(cloth => cloth.Name == selectedClothesName);

        if (count > 0 || selectedClothes.Level > 1)
        {
            foreach (var cloth in clothes)
            {
                if (cloth.Name == selectedClothes.Name)
                {
                    equipClothesName.text = cloth.Name;
                    equipProjectileName.text = cloth.Set;

                    // �÷��̾�� ����ȿ�� �ο� => ����ü ���� �� ����Ʈ ���� ����
                    ShowEquippedText(selectedClothes.Name);
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

        for (int i = 0; i < clothesImages.Length; i++)
        {
            if (clothesImages[i].name == selectedPetName)
            {
                // ���ο� "������" �ؽ�Ʈ ����
                equippedTextObject = Instantiate(prefabs);
                equippedTextObject.transform.SetParent(clothesImages[i].transform, false); // �θ� �����ϸ鼭 World Position �������� ����

                // RectTransform ����
                RectTransform rectTransform = equippedTextObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(35, -20); // ���ϴ� ��ġ�� ����
                break;
            }
        }
    }

    // �ǻ� ��ȭ
    public void EnhanceClothes()
    {
        foreach (var cloth in clothes)
        {
            if (cloth.Name == selectedClothesName)
            {
                if (cloth.Count >= cloth.RequiredCount)
                {
                    cloth.Count -= cloth.RequiredCount;
                    cloth.Level++;
                    cloth.RetentionEffect += 0.071f;
                    cloth.RequiredCount += 2;

                    // ����ȿ�� ������Ʈ
                    playerstatus.UpdateClothesRetentionEffects(clothes);
                                        
                    UpdateClothesText(cloth.Name);

                    InfomationText();
                    break;
                }
            }
        }
    }
}
