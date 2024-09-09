using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Image[] image;

    private Transform imagepos;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void TEST()
    {
        var delay = 0f;

        for(int i =0; i < image.Length; i++)
        {
            imagepos = image[i].transform;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(imagepos.DOScale(2f, 0.3f).SetDelay(delay).SetEase(Ease.InOutBack));

            delay += 0.05f;
        }
    }
}
