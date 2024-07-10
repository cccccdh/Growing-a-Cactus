using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomPick : MonoBehaviour
{
    public List<TextMeshProUGUI> textObjects = new List<TextMeshProUGUI>();
    
    ArmorManager armorManager;

    private void Awake()
    {
        armorManager = GetComponent<ArmorManager>();
    }

    public void RandomBtn()
    {
        PickRandomText();
    }

    void PickRandomText()
    {
        float[] probabilities = { 0.05f, 0.15f, 0.3f, 0.35f, 0.15f }; // S, A, B, C, D

        float totalProbability = 0;

        foreach (float prob in probabilities)
        {
            totalProbability += prob;
        }

        if (totalProbability != 1f)
        {
            Debug.LogError("Probabilities must sum to 1!");
            return;
        }

        foreach (TextMeshProUGUI textObj in textObjects)
        {
            string pickedGrade = PickGrade(probabilities);
            textObj.text = pickedGrade;
            SetTextColor(textObj, pickedGrade);
            armorManager.UpdateGradeCount(pickedGrade);
        }
    }

    string PickGrade(float[] probabilities)
    {
        float randomPoint = Random.value;
        float cumulativeProbability = 0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomPoint < cumulativeProbability)
            {
                switch (i)
                {
                    case 0: return "S";
                    case 1: return "A";
                    case 2: return "B";
                    case 3: return "C";
                    case 4: return "D";
                }
            }
        }

        return "D"; 
    }

    void SetTextColor(TextMeshProUGUI textObj, string grade)
    {
        switch (grade)
        {
            case "S":
                textObj.color = Color.red;
                break;
            case "A":
                textObj.color = new Color(1.0f, 0.5f, 0.0f); // Orange
                break;
            case "B":
                textObj.color = Color.yellow;
                break;
            case "C":
                textObj.color = Color.green;
                break;
            case "D":
                textObj.color = Color.cyan;
                break;
        }
    }
}
