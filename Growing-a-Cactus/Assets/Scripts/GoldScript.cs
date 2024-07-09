using UnityEngine;
using TMPro;

public class GoldScript : MonoBehaviour
{
    public TextMeshProUGUI GoldText;
    private int gold = 0;

    void Start()
    {
        UpdateGoldText();
    }

    void UpdateGoldText()
    {
        GoldText.text = gold.ToString();
    }

    public void IncreaseGold(int amount)
    {
        gold += amount;
        UpdateGoldText();
    }
}