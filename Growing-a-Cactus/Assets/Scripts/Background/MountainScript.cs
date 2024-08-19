using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainScript : MonoBehaviour
{
    [SerializeField][Range(0.1f, 20f)] float speed = 0.1f;
    [SerializeField] float posValue;

    Vector2 startpos;
    float newPos;

    void Start()
    {
        speed = 0.1f;
        startpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newPos = Mathf.Repeat(Time.time * speed, posValue);
        transform.position = startpos + Vector2.left * newPos;
    }
}
