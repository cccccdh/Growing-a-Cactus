using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float displayTime = 0.7f;
    private float time;

    void Update()
    {
        time += Time.deltaTime;

        transform.position += Vector3.up * Time.deltaTime;

        if(time > displayTime)
        {
            Destroy(gameObject);
        }
    }
}
