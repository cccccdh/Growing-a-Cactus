using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private MeshRenderer render;
    public float speed;
    private float offset;

    void Start()
    {
        render = GetComponent<MeshRenderer>();
        speed = 1;
    }

    void Update()
    {
        // 기존 로직 제거
    }

    public void MoveBackgroundForDuration(float duration)
    {
        StartCoroutine(MoveBackground(duration));
    }

    private IEnumerator MoveBackground(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            movebg();
            yield return null;
        }
    }

    public void movebg()
    {
        offset += Time.deltaTime * speed;
        render.material.mainTextureOffset = new Vector2(offset, 0);
    }
}