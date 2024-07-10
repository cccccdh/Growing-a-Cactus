using System;
using TMPro;
using UnityEngine;

public class ArmorManager : MonoBehaviour
{  
    public TextMeshProUGUI Stext;
    public TextMeshProUGUI Atext;
    public TextMeshProUGUI Btext;
    public TextMeshProUGUI Ctext;
    public TextMeshProUGUI Dtext;

    int SCount;
    int ACount;
    int BCount;
    int CCount;
    int DCount;

    void Start()
    {
        Init_Count();
    }

    private void Init_Count()
    {
        SCount = 0;
        ACount = 0;
        BCount = 0;            
        CCount = 0; 
        DCount = 0;
    }

    public void UpdateGradeCount(string grade)
    {
        switch (grade)
        {
            case "S":
                SCount++;
                break;
            case "A":
                ACount++;
                break;
            case "B":
                BCount++;
                break;
            case "C":
                CCount++;
                break;
            case "D":
                DCount++;
                break;
        }
        UpdateTexts(); 
    }

    void UpdateTexts()
    {
        Stext.text = SCount.ToString();
        Atext.text = ACount.ToString();
        Btext.text = BCount.ToString();
        Ctext.text = CCount.ToString();
        Dtext.text = DCount.ToString();
    }
}
