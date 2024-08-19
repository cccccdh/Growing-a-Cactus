using System.Collections;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField][Range(1f, 20f)] float speed = 3f;
    [SerializeField] float posValue;
    [SerializeField] bool isMoving = false; // 유니티 인스펙터에서 제어 가능한 bool 값

    Vector2 startPos;
    float newpos;
    float time = 0f; // 외부에서 선언된 time 변수

    void Start()
    {
        isMoving = false;
        startPos = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            time += Time.deltaTime; // isMoving이 true일 때만 time을 증가시킴
            newpos = Mathf.Repeat(time * speed, posValue);
            transform.position = startPos + Vector2.left * newpos;
        }
    }

    // isMoving 값을 변경하는 메서드
    public void SetMoving(bool moving)
    {
        isMoving = moving;
    }

    // 모든 적이 죽었을 때 호출되는 메서드
    public void OnKilled()
    {
        SetMoving(true); // 배경 움직임 시작
        StartCoroutine(StopMovementAfterDelay(0.9f)); // 0.9초 후에 배경 움직임 멈춤
    }

    private IEnumerator StopMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetMoving(false); // 배경 움직임 멈추기
    }
}
