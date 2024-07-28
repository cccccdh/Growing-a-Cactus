using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PoolManager>();
            return instance;
        }
    }

    public GameObject dmgText;

    public void CreateDamageText(Vector3 hitPoint, float damage, bool isCritical)
    {
        GameObject damageTextObj = Instantiate(dmgText, hitPoint, Quaternion.identity);
        TextMeshPro damageText = damageTextObj.GetComponentInChildren<TextMeshPro>();
        
        if (damage >= 1000)
        {
            int unitIndex = -1;
            float displaydamage = damage;

            while (displaydamage >= 1000 && unitIndex < 25)
            {
                displaydamage /= 1000f;
                unitIndex++;
            }

            char unitChar = (char)('A' + unitIndex);
            damageText.text = $"{displaydamage:F1}{unitChar}";
        }
        else
        {
            damageText.text = $"{(int)damage}";
        }

        if (isCritical)
        {
            damageText.color = Color.red;
        }
        else
        {
            damageText.color = Color.white;
        }

        Destroy(damageTextObj, 1.0f);
    }
}

