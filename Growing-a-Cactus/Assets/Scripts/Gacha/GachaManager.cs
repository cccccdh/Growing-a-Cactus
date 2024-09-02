using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public GachaUIManager gachaUIManager;
    public ItemManager itemManager;
    public PetManager petManager;

    public GameObject[] equipmentLockBtn;
    public GameObject[] petLockBtn;
    public GameObject[] clothesLockBtn;

    public bool UnLockEquipment = false;
    public bool UnLockPet = false;
    public bool UnLockClothes = false;
    

    public List<Item> itemList;
    public List<Pet> petList;
    public List<Clothes> clothesList;

    // 장비 리스트 초기화
    public void InitializeItems(List<Item> items)
    {
        itemList = items;
    }

    // 펫 리스트 초기화
    public void InitializePets(List<Pet> pets)
    {
        petList = pets;
    }

    // 의상 리스트 초기화
    public void InitializeClothes(List<Clothes> clothes)
    {
        clothesList = clothes;
    }

    // 가챠 버튼 해금
    public void Unlock(string Name)
    {
        if (!UnLockEquipment && Name == "장비")
        {
            UnLockEquipment = true;
            foreach (var btn in equipmentLockBtn)
            {
                btn.SetActive(false);
            }
        }
        else if (!UnLockPet && Name == "펫")
        {
            UnLockPet = true;
            foreach (var btn in petLockBtn)
            {
                btn.SetActive(false);
            }
        }
        //else if (!UnLockClothes && Name == "의상")
        //{
        //    UnLockClothes = true;
        //    foreach (var btn in clothesLockBtn)
        //    {
        //        btn.SetActive(false);
        //    }
        //}
    }

    // 장비 가챠를 수행하는 메서드
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
                        itemManager.UpdateItemCount(item.Name); // 아이템 개수 업데이트
                        //Debug.Log($"뽑기 {i + 1}: {item.Name} (타입: {item.Type}, 등급: {item.Grade}), 개수 : {item.Count}");
                        break;
                    }
                }
            }
            itemManager.UpdateItemImages(resultItemList); // 장비창 업데이트
            gachaUIManager.UpdateGachaUI(resultItemList); // UI 업데이트

            // 퀘스트 진행 상황 업데이트
            QuestManager.instance.UpdateQuestProgress(times, "장비 뽑기");
        }        
    }

    // 펫 가챠를 수행하는 메서드
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
                        //Debug.Log($"뽑기 {i + 1}: {pet.Name} / 등급: {pet.Grade}");
                        break;
                    }
                }
            }

            petManager.UpdatePetImages(resultPetList); // 펫창 업데이트
            gachaUIManager.UpdateGachaUI(resultPetList); // UI 업데이트

            // 퀘스트 진행 상황 업데이트
            QuestManager.instance.UpdateQuestProgress(times, "펫 뽑기");
        }        
    }
}
