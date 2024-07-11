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

    public void CreateDamageText(Vector3 hitPoint, float damage)
    {
        GameObject damageText = Instantiate(dmgText, hitPoint, Quaternion.identity);
        damageText.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
    }
}

