using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CSVReader : MonoBehaviour
{
    public class Item
    {
        public string Name;
        public string Type;
        public string Grade;
        public float Probability;
    }

    public List<Item> itemList = new List<Item>();
    public Image[] images;
    public TextAsset csvFile;

    private List<Item> ResultItemList = new List<Item>();

    void Start()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        StringReader reader = new StringReader(csvFile.text);
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
                Probability = float.Parse(values[3]) / 100f
            };
            itemList.Add(item);
        }
    }

    public void PerformGacha(int times)
    {
        for (int i = 0; i < times; i++)
        {
            float rand = Random.value;
            float cumulative = 0f;
            foreach (var item in itemList)
            {
                cumulative += item.Probability;
                if (rand < cumulative)
                {
                    ResultItemList.Add(item);
                    Debug.Log($"뽑기 {i + 1}: {item.Name} (타입: {item.Type}, 등급: {item.Grade})");
                    Results();
                    break;
                }
            }
        }
    }

    void Results()
    {
        foreach(var result in ResultItemList)
        {
            foreach(var image in images)
            {
                if(image.name == result.Name)
                {
                    Color color = image.color;
                    color.a= 1f;
                    image.color = color;
                }
            }
        }
    }
}
