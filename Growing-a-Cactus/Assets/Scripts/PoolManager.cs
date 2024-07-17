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
        damageText.text = damage.ToString();

        if (isCritical)
        {
            damageText.color = Color.red;
        }
        else
        {
            damageText.color = Color.white;
        }

        Destroy(damageText, 1.0f);
    }
}

