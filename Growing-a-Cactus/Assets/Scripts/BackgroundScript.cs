using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField][Range(1f, 100f)] float speed = 20f; // �ִ� ���� 100f�� ����, �⺻���� 20f�� ����
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
            yield return null; // ���� �����ӱ��� ���
        }
    }

    private void MoveBg()
    {
        newPos = Mathf.Repeat(Time.time * speed, posValue);
        transform.position = startPos + Vector2.left * newPos; // �����ʿ��� �������� �����̰� ����
    }
}