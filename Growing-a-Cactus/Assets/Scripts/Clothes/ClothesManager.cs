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

    [Header("장착 중 텍스트")]
    public GameObject prefabs;
    private GameObject equippedTextObject;

    [Header("참조")]
    public PlayerStatus playerstatus;

    private List<Clothes> clothes = new List<Clothes>();
    private string selectedClothesName;
        
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        // 텍스트 초기화
        NameText.text = $"기본";
        ProjectileText.text = "가시";
        RetentionEffectText.text = $"공격력 + 0.00%";
        
        // 버튼 상태 초기화
        clothesEquipButton.interactable = false;
        clothesEnhanceButton.interactable = false;
    }

    // 의상 리스트 설정
    public void SetClothes(List<Clothes> clothesList)
    {
        clothes = clothesList;
        playerstatus.UpdateClothesRetentionEffects(clothesList);
    }

    // 의상 개수를 업데이트하는 함수
    public void UpdateClothesCount(string clothesName)
    {
        foreach (var cloth in clothes)
        {
            if (cloth.Name == clothesName)
            {
                cloth.Count++;
                UpdateClothesText(clothesName);
                // 보유효과 갱신 추가
                playerstatus.UpdateClothesRetentionEffects(clothes);
                break;
            }
        }
    }

    // 의상 보유개수를 가져오는 함수
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

    // 의상 강화 개수를 가져오는 함수
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

    // 의상 레벨을 가져오는 함수
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

    // 의상 색상 값 가져오기
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

    // 의상 획득 시 의상 UI 업데이트
    public void UpdateClothesImages(List<Clothes> resultClothesList)
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
                int count = GetClothesCount(clothesName);
                int requiredcount = GetClothesRequiredCount(clothesName);
                int level = GetClothesLevel(clothesName);
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
            if (selectedClothesName == cloth.Name)
            {
                RetentionEffectText.text = $"공격력 + {TextFormatter.FormatText(cloth.RetentionEffect * 100)}%";
                LevelText.text = $"Lv.{cloth.Level}";
                CountText.text = $"( {cloth.Count} / {cloth.RequiredCount} )";
                ProjectileText.text = (cloth.Set == "Null" ? "가시" : $"{cloth.Set}");

                int level = GetClothesLevel(cloth.Name);
                int count = GetClothesCount(cloth.Name);
                int requirecount = GetClothesRequiredCount(cloth.Name);
                clothesEquipButton.interactable = (count > 0 || level > 1);
                clothesEnhanceButton.interactable = count >= requirecount;
                break;
            }
        }
    }

    // 의상 장착
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

                    // 플레이어에게 장착효과 부여 => 투사체 변경 및 이펙트 변경 예정
                    ShowEquippedText(selectedClothes.Name);
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

        for (int i = 0; i < clothesImages.Length; i++)
        {
            if (clothesImages[i].name == selectedPetName)
            {
                // 새로운 "장착중" 텍스트 생성
                equippedTextObject = Instantiate(prefabs);
                equippedTextObject.transform.SetParent(clothesImages[i].transform, false); // 부모를 설정하면서 World Position 유지하지 않음

                // RectTransform 설정
                RectTransform rectTransform = equippedTextObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(35, -20); // 원하는 위치로 설정
                break;
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
                    cloth.RetentionEffect += 0.071f;
                    cloth.RequiredCount += 2;

                    // 보유효과 업데이트
                    playerstatus.UpdateClothesRetentionEffects(clothes);
                                        
                    UpdateClothesText(cloth.Name);

                    InfomationText();
                    break;
                }
            }
        }
    }
}
