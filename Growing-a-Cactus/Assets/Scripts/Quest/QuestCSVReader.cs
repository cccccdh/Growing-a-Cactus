using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestCSVReader : MonoBehaviour
{
    public TextAsset csvFile;
    public QuestManager questmanager;

    private List<Quest> questList = new List<Quest>();

    void Start()
    {
        LoadQuests();
        questmanager.SetQuest(questList);
    }

    private void LoadQuests()
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

                string[] fields = line.Split(',');

                Quest quest = new Quest()
                {
                    Id = int.Parse(fields[0]),
                    Title = fields[1],
                    Description = fields[2],
                    Goal = int.Parse(fields[3]),
                    Reward = int.Parse(fields[4]),
                    Requirement = fields[5],
                    UnlockFeature = fields[6],
                    GoalCount = int.Parse(fields[7]),
                    IsActive = bool.Parse(fields[8])
                };

                questList.Add(quest);
            }
        }
    }
}
