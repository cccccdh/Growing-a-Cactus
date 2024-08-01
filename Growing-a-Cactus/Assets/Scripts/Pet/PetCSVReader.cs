using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PetCSVReader : MonoBehaviour
{
    public List<Pet> petList = new List<Pet>();
    public TextAsset csvFile;

    public PetManager petManager;
    public GachaManager gachaManager;    

    void Start()
    {
        ReadCSV();
        petManager.SetItems(petList);
    }

    void ReadCSV()
    {
        using (StringReader reader = new StringReader(csvFile.text))
        {
            bool isHeader = true;

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                string[] values = line.Split(',');
                Pet pet = new Pet
                {
                    Name = values[0],
                    Grade = values[1],
                    Probability = float.Parse(values[2]),
                    //ReactionEffect = float.Parse(values[3]) / 100,
                    //EquipEffect = float.Parse(values[4]) / 100,
                    Level = 1,
                    Count = 0,
                    RequiredCount = 2
                };
                petList.Add(pet);
            }
        }

        gachaManager.InitializePets(petList);
    }
}
