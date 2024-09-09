using DG.Tweening;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject coinPrefabs;
    public RectTransform target;

    private Vector3 targetPos;

    void Start()
    {
        targetPos = target.position;
    }

    public void MoveCoin(Vector3 pos)
    {
        GameObject coin = PoolManager.instance.GetGold(pos);
        coin.SetActive(true);

        float delay = 0f;
        Sequence finalSequence = DOTween.Sequence(); // 모든 자식 애니메이션이 끝난 후 호출할 시퀀스

        // 자식 객체 개별 애니메이션
        for (int i = 0; i < coin.transform.childCount; i++)
        {
            Transform child = coin.transform.GetChild(i);
            Vector3 childpos = child.position;

            // 애니메이션을 개별적으로 설정
            Sequence sequence = DOTween.Sequence();

            // 자식 객체의 애니메이션을 설정
            sequence.Append(child.DOScale(0.15f, 0.3f).SetEase(Ease.OutBack).SetDelay(delay));
            sequence.Join(child.DOMove(targetPos, 0.8f).SetEase(Ease.InOutBack).SetDelay(delay + 0.5f));
            sequence.Append(child.DOScale(0f, 0.3f).SetEase(Ease.OutBack).SetDelay(delay));

            // 애니메이션이 끝난 후 위치를 초기화
            sequence.OnComplete(() =>
            {
                child.position = childpos;
            });

            finalSequence.Insert(0, sequence); // 모든 시퀀스를 finalSequence에 추가

            delay += 0.1f;
        }

        // 모든 자식 애니메이션이 끝난 후 풀에 반환
        finalSequence.OnComplete(() =>
        {
            PoolManager.instance.ReturnToGoldPool(coin); // 풀에 반환
        });
    }
}
