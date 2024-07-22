using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField][Range(1f, 100f)] float speed = 20f; // 최대 값을 100f로 증가, 기본값을 20f로 설정
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
            yield return null; // 다음 프레임까지 대기
        }
    }

    private void MoveBg()
    {
        newPos = Mathf.Repeat(Time.time * speed, posValue);
        transform.position = startPos + Vector2.left * newPos; // 오른쪽에서 왼쪽으로 움직이게 설정
    }
}