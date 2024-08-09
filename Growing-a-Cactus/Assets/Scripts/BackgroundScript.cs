using System.Collections;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField] float speed = 20f; 
    [SerializeField] float posValue;

    Vector2 startPos;
    float newPos;

    void Start()
    {
        startPos = transform.position;
    }

    public void StartMoveBg()
    {
        StartCoroutine(MoveBgForOneSecond());
    }

    private IEnumerator MoveBgForOneSecond()
    {
        float startTime = Time.time;
        while (Time.time - startTime < 1f)
        {
            MoveBg();
            yield return null; 
        }
    }

    private void MoveBg()
    {
        newPos = Mathf.Repeat(Time.time * speed, posValue);
        transform.position = startPos + Vector2.left * newPos; 
    }
}