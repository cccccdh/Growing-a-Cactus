using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ClothesCSVReader : MonoBehaviour
{
    public List<Clothes> clothesList = new List<Clothes>();
    public TextAsset csvFile;

    public ClothesManager clothesManager;
    public GachaManager gachaManager;

    void Start()
    {
        ReadCSV();
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
                Clothes clothes = new Clothes
                {
                    Name = values[0],
                    Grade = values[1],
                    Probability = float.Parse(values[2]),
                    Set = values[3],
                    RetentionEffect = float.Parse(values[4]) / 100,
                };
                clothesList.Add(clothes);
            }
        }

        gachaManager.InitializeClothes(clothesList);
    }
}
