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
    Color color = Color.white;

    // 장비 뽑기 결과를 UI에 반영
    public void UpdateGachaUI(List<Item> resultItemList)
    {
        InitImageScale();
        InitText();

        delay = 0f;

        // 뽑기 결과를 가챠창 텍스트에 반영
        for (int i = 0; i < gachaImages.Length; i++)
        {
            if (i < resultItemList.Count)
            {
                var result = resultItemList[i];
                var text = gachaImages[i].GetComponentInChildren<TextMeshProUGUI>();

                if (result.Type == "무기")
                {
                    color = itemManager.GetColorForWeapon(result.Name);
                }
                else if (result.Type == "방어구")
                {
                    color = itemManager.GetColorForArmor(result.Name);
                }

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

    // 펫 뽑기 결과를 UI에 반영
    public void UpdateGachaUI(List<Pet> resultPetList)
    {
        InitImageScale();
        InitText();

        delay = 0f;

        // 뽑기 결과를 가챠창 텍스트에 반영
        for (int i = 0; i < gachaImages.Length; i++)
        {
            if (i < resultPetList.Count)
            {
                var result = resultPetList[i];
                var text = gachaImages[i].GetComponentInChildren<TextMeshProUGUI>();

                color = petManager.GetColorForPet(result.Name);
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

    // 의상 뽑기 결과를 UI에 반영
    public void UpdateGachaUI(List<Clothes> resultClothesList)
    {
        InitImageScale();
        InitText();

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

    // 가챠창의 텍스트를 초기화 (나중에 아트 오면 사라질 함수)
    private void InitText()
    {        
        foreach (var image in gachaImages)
        {
            var text = image.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = "";
            }
        }
    }
}
