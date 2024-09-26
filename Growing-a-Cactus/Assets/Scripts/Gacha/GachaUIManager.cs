using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GachaUIManager : MonoBehaviour
{
    public Image[] gachaImages;

    public ItemManager itemManager;
    public PetManager petManager;
    public ClothesManager clothesManager;

    private float delay;

    // 배경색
    Color color = Color.white;

    // 이미지
    Sprite gachaSprite;
    
    // 무기인지 방어구인지 타입 확인
    Transform typeImage;

    // 장비 뽑기 결과를 UI에 반영
    public void UpdateGachaUI(List<Item> resultItemList)
    {
        InitImageScale();
        InitImages();

        delay = 0f;

        // 뽑기 결과를 가챠창 텍스트에 반영
        for (int i = 0; i < gachaImages.Length; i++)
        {
            if (i < resultItemList.Count)
            {
                var result = resultItemList[i];
                var image = gachaImages[i].transform.GetChild(0).GetComponent<Image>();                

                if (result.Type == "무기")
                {
                    typeImage = gachaImages[i].transform.GetChild(1);
                    gachaSprite = Resources.Load<Sprite>($"_Item/Weapons/{result.Name}");
                    color = itemManager.GetColorForWeapon(result.Name);                    
                }
                else if (result.Type == "방어구")
                {
                    typeImage = gachaImages[i].transform.GetChild(2);
                    gachaSprite = Resources.Load<Sprite>($"_Item/Armors/{result.Name}");
                    color = itemManager.GetColorForArmor(result.Name);
                }

                typeImage.gameObject.SetActive(true);
                gachaImages[i].color = color;
                image.sprite = gachaSprite;

                // 가챠 애니메이션
                Sequence sequence = DOTween.Sequence();
                sequence.Append(gachaImages[i].transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.InOutBack));

                delay += 0.05f;
            }
        }
    }

    // 펫 뽑기 결과를 UI에 반영
    public void UpdateGachaUI(List<Pet> resultPetList)
    {
        InitImageScale();
        InitImages();

        delay = 0f;

        // 뽑기 결과를 가챠창 텍스트에 반영
        for (int i = 0; i < gachaImages.Length; i++)
        {
            if (i < resultPetList.Count)
            {
                var result = resultPetList[i];
                var image = gachaImages[i].transform.GetChild(0).GetComponent<Image>();

                // 이미지 변경
                gachaSprite = Resources.Load<Sprite>($"_Pet/{result.Name}");
                image.sprite = gachaSprite;

                // 색상 변경
                color = petManager.GetColorForPet(result.Name);
                gachaImages[i].color = color;

                // 가챠 애니메이션
                Sequence sequence = DOTween.Sequence();
                sequence.Append(gachaImages[i].transform.DOScale(new Vector3(-1,1,1), 0.3f).SetDelay(delay).SetEase(Ease.InOutBack));

                delay += 0.05f;
            }        
        }
    }

    // 의상 뽑기 결과를 UI에 반영
    public void UpdateGachaUI(List<Clothes> resultClothesList)
    {
        InitImageScale();
        InitImages();

         delay = 0f;

        // 뽑기 결과를 가챠창 텍스트에 반영
        for (int i = 0; i < gachaImages.Length; i++)
        {
            if (i < resultClothesList.Count)
            {
                var result = resultClothesList[i];
                var text = gachaImages[i].GetComponentInChildren<TextMeshProUGUI>();

                color = clothesManager.GetColorForClothes(result.Name);
                gachaImages[i].color = color;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(gachaImages[i].transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.InOutBack))
                    .OnComplete(() =>
                    {
                        text.text = result.Name;
                    });

                delay += 0.05f;
            }
        }
    }

    // 이미지 크기 0으로 초기화
    private void InitImageScale()
    {        
        for (int i = 0; i < gachaImages.Length; i++)
        {
            gachaImages[i].rectTransform.localScale = new Vector3(0, 0, 0);
        }
    }

    // 이미지 초기화
    private void InitImages()
    {        
        foreach (var image in gachaImages)
        {
            image.transform.GetChild(1).gameObject.SetActive(false);
            image.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
