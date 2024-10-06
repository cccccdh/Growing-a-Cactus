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

    // 장비 퀘스트 초기화
    public void InitializeItems(List<Item> items)
    {
        itemList = items;
    }

    // 펫 퀘스트 초기화
    public void InitializePets(List<Pet> pets)
    {
        petList = pets;
    }

    // 의상 퀘스트 초기화
    public void InitializeClothes(List<Clothes> clothes)
    {
        clothesList = clothes;
    }

    // 뽑기 해금
    public void Unlock(string Name)
    {
        if (Name == "장비")
        {
            UnLockEquipment = true;
            foreach (var btn in equipmentLockBtn)
            {
                btn.SetActive(false);
            }
        }
        else if (Name == "펫")
        {
            UnLockPet = true;
            foreach (var btn in petLockBtn)
            {
                btn.SetActive(false);
            }
        }
        else if (Name == "의상")
        {
            UnLockClothes = true;
            foreach (var btn in clothesLockBtn)
            {
                btn.SetActive(false);
            }
        }
    }

    public void Relock(string Name)
    {
        if (Name == "장비")
        {
            UnLockEquipment = false;
            foreach (var btn in equipmentLockBtn)
            {
                btn.SetActive(true);
            }
        }
        else if (Name == "펫")
        {
            UnLockPet = false;
            foreach (var btn in petLockBtn)
            {
                btn.SetActive(true);
            }
        }
        else if (Name == "의상")
        {
            UnLockClothes = false;
            foreach (var btn in clothesLockBtn)
            {
                btn.SetActive(true);
            }
        }
    }

    // 장비 뽑기
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
                        itemManager.UpdateItemCount(item.Name); 
                        break;
                    }
                }
            }

            itemManager.UpdateItemImages(resultItemList); 
            gachaUIManager.UpdateGachaUI(resultItemList); 

            // 퀘스트 진행상황 업데이트
            QuestManager.instance.DrawEquipment(times);
        }        
    }

    // 펫 뽑기
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
                        break;
                    }
                }
            }

            petManager.UpdatePetImages(resultPetList); 
            gachaUIManager.UpdateGachaUI(resultPetList); 

            // 퀘스트 진행상황 업데이트
            QuestManager.instance.DrawPet(times);
        }        
    }

    // 의상 뽑기
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
                        break;
                    }
                }
            }

            clothesManager.UpdateClothesImages(resultClothesList); 
            gachaUIManager.UpdateGachaUI(resultClothesList); 

            // 퀘스트 진행상황 업데이트
            QuestManager.instance.DrawClothes(times);
        }
    }
}
