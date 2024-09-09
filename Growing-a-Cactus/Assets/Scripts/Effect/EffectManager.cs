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
        Sequence finalSequence = DOTween.Sequence(); // ��� �ڽ� �ִϸ��̼��� ���� �� ȣ���� ������

        // �ڽ� ��ü ���� �ִϸ��̼�
        for (int i = 0; i < coin.transform.childCount; i++)
        {
            Transform child = coin.transform.GetChild(i);
            Vector3 childpos = child.position;

            // �ִϸ��̼��� ���������� ����
            Sequence sequence = DOTween.Sequence();

            // �ڽ� ��ü�� �ִϸ��̼��� ����
            sequence.Append(child.DOScale(0.15f, 0.3f).SetEase(Ease.OutBack).SetDelay(delay));
            sequence.Join(child.DOMove(targetPos, 0.8f).SetEase(Ease.InOutBack).SetDelay(delay + 0.5f));
            sequence.Append(child.DOScale(0f, 0.3f).SetEase(Ease.OutBack).SetDelay(delay));

            // �ִϸ��̼��� ���� �� ��ġ�� �ʱ�ȭ
            sequence.OnComplete(() =>
            {
                child.position = childpos;
            });

            finalSequence.Insert(0, sequence); // ��� �������� finalSequence�� �߰�

            delay += 0.1f;
        }

        // ��� �ڽ� �ִϸ��̼��� ���� �� Ǯ�� ��ȯ
        finalSequence.OnComplete(() =>
        {
            PoolManager.instance.ReturnToGoldPool(coin); // Ǯ�� ��ȯ
        });
    }
}
