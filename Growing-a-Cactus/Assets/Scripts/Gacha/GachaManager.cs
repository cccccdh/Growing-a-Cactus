using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public GachaUIManager gachaUIManager;
    public ItemManager itemManager;
    public PetManager petManager;
    public ClothesManager clothesManager;

    public GameObject[] equipmentLockBtn;
    public GameObject[] petLockBtn;
    public GameObject[] clothesLockBtn;

    public bool UnLockEquipment = false;
    public bool UnLockPet = false;
    public bool UnLockClothes = false;    

    public List<Item> itemList;
    public List<Pet> petList;
    public List<Clothes> clothesList;

    // ��� ����Ʈ �ʱ�ȭ
    public void InitializeItems(List<Item> items)
    {
        itemList = items;
    }

    // �� ����Ʈ �ʱ�ȭ
    public void InitializePets(List<Pet> pets)
    {
        petList = pets;
    }

    // �ǻ� ����Ʈ �ʱ�ȭ
    public void InitializeClothes(List<Clothes> clothes)
    {
        clothesList = clothes;
    }

    // ��í ��ư �ر�
    public void Unlock(string Name)
    {
        if (Name == "���")
        {
            UnLockEquipment = true;
            foreach (var btn in equipmentLockBtn)
            {
                btn.SetActive(false);
            }
        }
        else if (Name == "��")
        {
            UnLockPet = true;
            foreach (var btn in petLockBtn)
            {
                btn.SetActive(false);
            }
        }
        else if (Name == "�ǻ�")
        {
            UnLockClothes = true;
            foreach (var btn in clothesLockBtn)
            {
                btn.SetActive(false);
            }
        }
    }

    // ��� ��í�� �����ϴ� �޼���
    public void PerformGachaWithEquip(int times)
    {
        if (UnLockEquipment)
        {
            var resultItemList = new List<Item>();
            for (int i = 0; i < times; i++)
            {
                float rand = Random.Range(0, 100f);
                float cumulative = 0f;
                foreach (var item in itemList)
                {
                    cumulative += item.Probability;
                    if (rand < cumulative)
                    {
                        resultItemList.Add(item);
                        itemManager.UpdateItemCount(item.Name); // ������ ���� ������Ʈ
                        //Debug.Log($"�̱� {i + 1}: {item.Name} (Ÿ��: {item.Type}, ���: {item.Grade}), ���� : {item.Count}");
                        break;
                    }
                }
            }
            itemManager.UpdateItemImages(resultItemList); // ���â ������Ʈ
            gachaUIManager.UpdateGachaUI(resultItemList); // UI ������Ʈ


            // ����Ʈ ���� ��Ȳ ������Ʈ;

            QuestManager.instance.DrawEquipment(times);
        }        
    }

    // �� ��í�� �����ϴ� �޼���
    public void PerformGachaWithPet(int times)
    {
        if (UnLockPet)
        {
            var resultPetList = new List<Pet>();
            for (int i = 0; i < times; i++)
            {
                float rand = Random.Range(0, 100f);
                float cumulative = 0f;
                foreach (var pet in petList)
                {
                    cumulative += pet.Probability;
                    if (rand < cumulative)
                    {
                        resultPetList.Add(pet);
                        petManager.UpdatePetCount(pet.Name);
                        //Debug.Log($"�̱� {i + 1}: {Pet.Name} / ���: {Pet.Grade}");
                        break;
                    }
                }
            }

            petManager.UpdatePetImages(resultPetList); // ��â ������Ʈ
            gachaUIManager.UpdateGachaUI(resultPetList); // UI ������Ʈ

            // ����Ʈ ���� ��Ȳ ������Ʈ
            QuestManager.instance.DrawPet(times);
        }        
    }

    // �ǻ� ��í�� �����ϴ� �޼���
    public void PerformGachaWithClothes(int times)
    {
        if (UnLockClothes)
        {
            var resultClothesList = new List<Clothes>();
            for (int i = 0; i < times; i++)
            {
                float rand = Random.Range(0, 100f);
                float cumulative = 0f;
                foreach (var cloth in clothesList)
                {
                    cumulative += cloth.Probability;
                    if (rand < cumulative)
                    {
                        resultClothesList.Add(cloth);
                        clothesManager.UpdateClothesCount(cloth.Name);
                        //Debug.Log($"�̱� {i + 1}: {cloth.Name} / ���: {cloth.Grade}");
                        break;
                    }
                }
            }

            clothesManager.UpdateClothesImages(resultClothesList); // �ǻ� �̹��� ������Ʈ
            gachaUIManager.UpdateGachaUI(resultClothesList); // UI ������Ʈ

            // ����Ʈ ���� ��Ȳ ������Ʈ
            QuestManager.instance.DrawClothes(times);
        }
    }
}
