using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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

    [Header("Information")]
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ProjectileText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI RetentionEffectText;

    [Header("Scroll VIew")]
    public Image[] clothesImages;
    public TextMeshProUGUI[] clothesCountTexts;
    public TextMeshProUGUI[] clothesLevelTexts;

    [Header("����")]
    public PlayerStatus playerstatus;

    private List<Clothes> clothes = new List<Clothes>();
    private string selectedClothesName;

    // �ǻ� ����Ʈ ����
    public void SetClothes(List<Clothes> clothesList)
    {
        clothes = clothesList;
        // ����ȿ�� ���� �߰�
    }

    // �ǻ� ������ ������Ʈ�ϴ� �Լ�
    public void UpdateClothesCount(string clothesName)
    {
        foreach (var cloth in clothes)
        {
            if (cloth.Name == clothesName)
            {
                cloth.Count++;
                // ����ȿ�� ���� �߰�
                //playerstatus.UpdatePetRetentionEffects(pets);
                break;
            }
        }
    }

    // �ǻ� ���������� �������� �Լ�
    public int GetPetCount(string clothesName)
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
    public int GetPetRequiredCount(string clothesName)
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
    public int GetPetLevel(string clothesName)
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

    // �ǻ� ȹ�� �� �ǻ� UI ������Ʈ
    public void UpdatePetImages(List<Clothes> resultClothesList)
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
                int count = GetPetCount(clothesName);
                int requiredcount = GetPetRequiredCount(clothesName);
                int level = GetPetLevel(clothesName);
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
            if (cloth.Name == selectedClothesName)
            {
                RetentionEffectText.text = $"���ݷ� + {TextFormatter.FormatText(cloth.RetentionEffect * 100)}%";
                LevelText.text = $"Lv.{cloth.Level}";
                break;
            }
        }
    }

    // �ǻ� ����
    public void EquipClothes()
    {
        int count = GetPetCount(selectedClothesName);

        Clothes selectedClothes = clothes.Find(cloth => cloth.Name == selectedClothesName);

        if (count > 0 || selectedClothes.Level > 1)
        {
            foreach (var cloth in clothes)
            {
                if (cloth.Name == selectedClothesName)
                {
                    // �÷��̾�� ����ȿ�� �ο� => ����ü ���� �� ����Ʈ ���� ����
                    break;
                }
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
                    cloth.RetentionEffect += 0.093f;
                    cloth.RequiredCount += 2;

                    // ����ȿ�� ������Ʈ
                    //playerstatus.UpdatePetRetentionEffects(pets);
                                        
                    UpdateClothesText(cloth.Name);

                    InfomationText();
                    break;
                }
            }
        }
    }
}
