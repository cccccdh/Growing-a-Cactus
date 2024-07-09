using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;

        if(Time.deltaTime >= 0.7f)
            Destroy(gameObject);
    }
}
