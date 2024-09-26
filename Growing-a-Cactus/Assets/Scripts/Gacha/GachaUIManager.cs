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

    // ����
    Color color = Color.white;

    // �̹���
    Sprite gachaSprite;
    
    // �������� ������ Ÿ�� Ȯ��
    Transform typeImage;

    // ��� �̱� ����� UI�� �ݿ�
    public void UpdateGachaUI(List<Item> resultItemList)
    {
        InitImageScale();
        InitImages();

        delay = 0f;

        // �̱� ����� ��íâ �ؽ�Ʈ�� �ݿ�
        for (int i = 0; i < gachaImages.Length; i++)
        {
            if (i < resultItemList.Count)
            {
                var result = resultItemList[i];
                var image = gachaImages[i].transform.GetChild(0).GetComponent<Image>();                

                if (result.Type == "����")
                {
                    typeImage = gachaImages[i].transform.GetChild(1);
                    gachaSprite = Resources.Load<Sprite>($"_Item/Weapons/{result.Name}");
                    color = itemManager.GetColorForWeapon(result.Name);                    
                }
                else if (result.Type == "��")
                {
                    typeImage = gachaImages[i].transform.GetChild(2);
                    gachaSprite = Resources.Load<Sprite>($"_Item/Armors/{result.Name}");
                    color = itemManager.GetColorForArmor(result.Name);
                }

                typeImage.gameObject.SetActive(true);
                gachaImages[i].color = color;
                image.sprite = gachaSprite;

                // ��í �ִϸ��̼�
                Sequence sequence = DOTween.Sequence();
                sequence.Append(gachaImages[i].transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.InOutBack));

                delay += 0.05f;
            }
        }
    }

    // �� �̱� ����� UI�� �ݿ�
    public void UpdateGachaUI(List<Pet> resultPetList)
    {
        InitImageScale();
        InitImages();

        delay = 0f;

        // �̱� ����� ��íâ �ؽ�Ʈ�� �ݿ�
        for (int i = 0; i < gachaImages.Length; i++)
        {
            if (i < resultPetList.Count)
            {
                var result = resultPetList[i];
                var image = gachaImages[i].transform.GetChild(0).GetComponent<Image>();

                // �̹��� ����
                gachaSprite = Resources.Load<Sprite>($"_Pet/{result.Name}");
                image.sprite = gachaSprite;

                // ���� ����
                color = petManager.GetColorForPet(result.Name);
                gachaImages[i].color = color;

                // ��í �ִϸ��̼�
                Sequence sequence = DOTween.Sequence();
                sequence.Append(gachaImages[i].transform.DOScale(new Vector3(-1,1,1), 0.3f).SetDelay(delay).SetEase(Ease.InOutBack));

                delay += 0.05f;
            }        
        }
    }

    // �ǻ� �̱� ����� UI�� �ݿ�
    public void UpdateGachaUI(List<Clothes> resultClothesList)
    {
        InitImageScale();
        InitImages();

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

    // �̹��� �ʱ�ȭ
    private void InitImages()
    {        
        foreach (var image in gachaImages)
        {
            image.transform.GetChild(1).gameObject.SetActive(false);
            image.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
