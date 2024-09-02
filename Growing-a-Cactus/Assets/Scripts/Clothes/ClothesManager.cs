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

    [Header("참조")]
    public PlayerStatus playerstatus;

    private List<Clothes> clothes = new List<Clothes>();
    private string selectedClothesName;

    // 의상 리스트 설정
    public void SetClothes(List<Clothes> clothesList)
    {
        clothes = clothesList;
        // 보유효과 갱신 추가
    }

    // 의상 개수를 업데이트하는 함수
    public void UpdateClothesCount(string clothesName)
    {
        foreach (var cloth in clothes)
        {
            if (cloth.Name == clothesName)
            {
                cloth.Count++;
                // 보유효과 갱신 추가
                //playerstatus.UpdatePetRetentionEffects(pets);
                break;
            }
        }
    }

    // 의상 보유개수를 가져오는 함수
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

    // 의상 강화 개수를 가져오는 함수
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

    // 의상 레벨을 가져오는 함수
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

    // 의상 획득 시 의상 UI 업데이트
    public void UpdatePetImages(List<Clothes> resultClothesList)
    {
        foreach (var result in resultClothesList)
        {
            foreach (var image in clothesImages)
            {
                if (image.name == result.Name)
                {
                    // 알파값 변경
                    Color color = image.color;
                    color.a = 1f;
                    image.color = color;
                }
            }
        }
    }

    // 의상에 따라 텍스트 업데이트
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

    // 선택 의상 이름, 투사체 텍스트 업데이트
    public void SelectedClothes(Image image)
    {
        selectedClothesName = image.name;

        NameText.text = selectedClothesName;

        InfomationText();
    }

    // 선택 의상 업데이트
    public void InfomationText()
    {
        foreach (var cloth in clothes)
        {
            if (cloth.Name == selectedClothesName)
            {
                RetentionEffectText.text = $"공격력 + {TextFormatter.FormatText(cloth.RetentionEffect * 100)}%";
                LevelText.text = $"Lv.{cloth.Level}";
                break;
            }
        }
    }

    // 의상 장착
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
                    // 플레이어에게 장착효과 부여 => 투사체 변경 및 이펙트 변경 예정
                    break;
                }
            }
        }
    }

    // 의상 강화
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

                    // 보유효과 업데이트
                    //playerstatus.UpdatePetRetentionEffects(pets);
                                        
                    UpdateClothesText(cloth.Name);

                    InfomationText();
                    break;
                }
            }
        }
    }
}
