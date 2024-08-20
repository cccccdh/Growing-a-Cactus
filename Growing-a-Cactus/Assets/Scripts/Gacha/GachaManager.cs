using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public ItemManager itemManager;
    public GachaUIManager gachaUIManager;
    public PetManager petManager;

    public GameObject[] equipmentLockBtn;
    public GameObject[] petLockBtn;

    public bool UnLockEquipment = false;
    public bool UnLockPet = false;

    private List<Item> itemList;
    private List<Pet> petList;

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

    public void Unlock(string Name)
    {
        if (!UnLockEquipment && Name == "���")
        {
            UnLockEquipment = true;
            foreach(var btn in equipmentLockBtn)
            {
                btn.SetActive(false);
            }
        }
        else if(!UnLockPet && Name == "��")
        {
            UnLockPet = true;
            foreach (var btn in petLockBtn)
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

            // ����Ʈ ���� ��Ȳ ������Ʈ
            QuestManager.instance.UpdateQuestProgress(times, "��� �̱�");
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
                        //Debug.Log($"�̱� {i + 1}: {pet.Name} / ���: {pet.Grade}");
                        break;
                    }
                }
            }

            petManager.UpdatePetImages(resultPetList); // ��â ������Ʈ
            gachaUIManager.UpdateGachaUI(resultPetList); // UI ������Ʈ

            // ����Ʈ ���� ��Ȳ ������Ʈ
            QuestManager.instance.UpdateQuestProgress(times, "�� �̱�");
        }        
    }
}
