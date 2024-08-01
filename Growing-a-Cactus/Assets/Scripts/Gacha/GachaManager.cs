using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public ItemManager itemManager;
    public GachaUIManager gachaUIManager;
    public PetManager petManager;

    private List<Item> itemList;
    private List<Pet> petList;

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

    // 장비 가챠를 수행하는 메서드
    public void PerformGachaWithEquip(int times)
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
    }

    // 펫 가챠를 수행하는 메서드
    public void PerformGachaWithPet(int times)
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
    }
}
