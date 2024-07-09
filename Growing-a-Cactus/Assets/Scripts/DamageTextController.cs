using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    private static DamageTextController instance;

    public static DamageTextController Instance
    {
        get {
            if(instance == null)
                instance = GameObject.FindObjectOfType<DamageTextController>();
            return instance; 
        }
    }

    public GameObject dmgText;

    public void CreateDamageText(Vector3 hitPoint, float damage)
    {
        GameObject damageText = Instantiate(dmgText, hitPoint, Quaternion.identity);
        damageText.GetComponent<TextMeshProUGUI>().text = damage.ToString();
    }
}
