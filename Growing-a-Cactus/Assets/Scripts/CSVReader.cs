using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public class Item
    {
        public string Name;
        public string Type;
        public string Grade;
        public float Probability;
    }

    public Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();
    public TextAsset csvFile;
    public ItemManager itemManager; 
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
                Item item = new Item
                {
                    Name = values[0],
                    Type = values[1],
                    Grade = values[2],
                    Probability = float.Parse(values[3])
                };
                itemDictionary[item.Name] = item; // �������� ��ųʸ��� �߰�
            }
        }

        // CSV ���� �ε� ��, ��í �Ŵ����� ������ ������ ����
        gachaManager.InitializeItems(itemDictionary);
    }
}
