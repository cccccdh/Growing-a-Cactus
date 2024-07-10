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
    public int DamageTextPoolSize = 7;
    private Queue<GameObject> damageTextPool;

    private void Awake()
    {
        damageTextPool = new Queue<GameObject>();

        for (int i = 0; i < DamageTextPoolSize; i++)
        {
            GameObject obj = Instantiate(dmgText);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            damageTextPool.Enqueue(obj);
        }
    }

    public void CreateDamageText(Vector3 hitPoint, float damage)
    {
        if (damageTextPool.Count > 0)
        {
            GameObject damageText = damageTextPool.Dequeue();
            damageText.transform.position = new Vector3(hitPoint.x, hitPoint.y + 0.5f, hitPoint.z);
            damageText.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
            damageText.SetActive(true);
        }
    }

    public void ReturnToPool(GameObject damageText)
    {
        damageText.SetActive(false);
        damageTextPool.Enqueue(damageText);
    }
}
