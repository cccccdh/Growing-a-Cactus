using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    //public GameObject touchscreen;
    public TextMeshProUGUI title;

    private float blinkSpeed = 1.0f; // �����̴� �ӵ�

    void Start()
    {
        StartCoroutine(Blink()); // �����̴� �ڷ�ƾ ����
    }

    private IEnumerator Blink()
    {
        while (true) // ���� �ݺ�
        {
            // �ؽ�Ʈ�� ���� ���� ���������� ����
            for (float alpha = 1.0f; alpha >= 0.0f; alpha -= Time.deltaTime * blinkSpeed)
            {
                SetTextAlpha(alpha);
                yield return null;
            }

            // �ؽ�Ʈ�� ���� ���� ���������� ����
            for (float alpha = 0.0f; alpha <= 1.0f; alpha += Time.deltaTime * blinkSpeed)
            {
                SetTextAlpha(alpha);
                yield return null;
            }
        }
    }

    // �ؽ�Ʈ�� ���� �� ���� �޼ҵ�
    private void SetTextAlpha(float alpha)
    {
        Color color = title.color;
        color.a = alpha;
        title.color = color;
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene("Loading");
    }
}
