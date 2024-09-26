using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemCSVReader : MonoBehaviour
{
    public List <Item> itemList = new List <Item>();
    public TextAsset csvFile;
    public ItemManager itemManager; 
    public GachaManager gachaManager; 

    void Start()
    {
        ReadCSV();
        itemManager.SetItems(itemList);
    }

    public void ReadCSV()
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
                Item item = new Item
                {
                    Name = values[0],
                    Type = values[1],
                    Grade = values[2],
                    Probability = float.Parse(values[3]),
                    RetentionEffect = float.Parse(values[4]) / 100,
                    EquipEffect = float.Parse(values[5]) / 100,
                    Level = int.Parse(values[6]),
                    Count = int.Parse(values[7]),
                    RequiredCount = int.Parse(values[8])
                };
                itemList.Add(item);
            }
        }

        gachaManager.InitializeItems(itemList);
    }
}
