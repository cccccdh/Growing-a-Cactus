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
        if (gold >= 1000)
        {
            float goldInK = gold / 1000f;
            GoldText.text = goldInK.ToString("0.0") + "k";
        }
        else
        {
            GoldText.text = gold.ToString();
        }
    }

    public void IncreaseGold(int amount)
    {
        gold += amount;
        UpdateGoldText();
    }
}