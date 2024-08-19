using UnityEngine;

public class DamageText : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;
    }
}
