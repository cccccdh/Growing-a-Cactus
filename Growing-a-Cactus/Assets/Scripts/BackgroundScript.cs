using System.Collections;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField][Range(1f, 20f)] float speed = 3f;
    [SerializeField] float posValue;
    [SerializeField] bool isMoving = false; // ����Ƽ �ν����Ϳ��� ���� ������ bool ��

    Vector2 startPos;
    float newpos;
    float time = 0f; // �ܺο��� ����� time ����

    void Start()
    {
        isMoving = false;
        startPos = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            time += Time.deltaTime; // isMoving�� true�� ���� time�� ������Ŵ
            newpos = Mathf.Repeat(time * speed, posValue);
            transform.position = startPos + Vector2.left * newpos;
        }
    }

    // isMoving ���� �����ϴ� �޼���
    public void SetMoving(bool moving)
    {
        isMoving = moving;
    }

    // ��� ���� �׾��� �� ȣ��Ǵ� �޼���
    public void OnKilled()
    {
        SetMoving(true); // ��� ������ ����
        StartCoroutine(StopMovementAfterDelay(0.9f)); // 0.9�� �Ŀ� ��� ������ ����
    }

    private IEnumerator StopMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetMoving(false); // ��� ������ ���߱�
    }
}
