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

    // ��� �̱� ����� UI�� �ݿ�
    public void UpdateGachaUI(List<Item> resultItemList)
    {
        InitImageScale();
        InitText();

        delay = 0f;

        // �̱� ����� ��íâ �ؽ�Ʈ�� �ݿ�
        for (int i = 0; i < gachaImages.Length; i++)
        {
            if (i < resultItemList.Count)
            {
                var result = resultItemList[i];
                var text = gachaImages[i].GetComponentInChildren<TextMeshProUGUI>();

                if (result.Type == "����")
                {
                    color = itemManager.GetColorForWeapon(result.Name);
                }
                else if (result.Type == "��")
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

    // �� �̱� ����� UI�� �ݿ�
    public void UpdateGachaUI(List<Pet> resultPetList)
    {
        InitImageScale();
        InitText();

        delay = 0f;

        // �̱� ����� ��íâ �ؽ�Ʈ�� �ݿ�
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

    // �ǻ� �̱� ����� UI�� �ݿ�
    public void UpdateGachaUI(List<Clothes> resultClothesList)
    {
        InitImageScale();
        InitText();

         delay = 0f;

        // �̱� ����� ��íâ �ؽ�Ʈ�� �ݿ�
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

    // �̹��� ũ�� 0���� �ʱ�ȭ
    private void InitImageScale()
    {        
        for (int i = 0; i < gachaImages.Length; i++)
        {
            gachaImages[i].rectTransform.localScale = new Vector3(0, 0, 0);
        }
    }

    // ��íâ�� �ؽ�Ʈ�� �ʱ�ȭ (���߿� ��Ʈ ���� ����� �Լ�)
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
