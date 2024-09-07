using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    //public GameObject touchscreen;
    public TextMeshProUGUI title;

    private float blinkSpeed = 1.0f; // 깜빡이는 속도

    void Start()
    {
        StartCoroutine(Blink()); // 깜빡이는 코루틴 시작
    }

    private IEnumerator Blink()
    {
        while (true) // 무한 반복
        {
            // 텍스트의 알파 값을 점진적으로 감소
            for (float alpha = 1.0f; alpha >= 0.0f; alpha -= Time.deltaTime * blinkSpeed)
            {
                SetTextAlpha(alpha);
                yield return null;
            }

            // 텍스트의 알파 값을 점진적으로 증가
            for (float alpha = 0.0f; alpha <= 1.0f; alpha += Time.deltaTime * blinkSpeed)
            {
                SetTextAlpha(alpha);
                yield return null;
            }
        }
    }

    // 텍스트의 알파 값 설정 메소드
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
