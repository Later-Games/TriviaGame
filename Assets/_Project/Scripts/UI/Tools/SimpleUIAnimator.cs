using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SimpleUIAnimator : MonoBehaviour
{
    [Header("Scale Animation")]
    [SerializeField] bool shouldScale = true;
    [SerializeField] float scaleAmount = 1.25f; // ShowIf("shouldScale") 
    [SerializeField] float scaleDuration = 2f;
    [SerializeField] Ease scaleEase = Ease.Linear;

    [Header("Levitation Animation")]
    [SerializeField] bool shouldLevitate = true;
    [SerializeField] float levitationDuration = 0.5f;
    [SerializeField] float levitationAmount = 0.5f;
    [SerializeField] Ease levitationEase = Ease.InOutQuad;
    
    void Start()
    {
        StartAnimating();
    }
    
    void StartAnimating()
    {
        var ts = transform;
        if (shouldScale)
        {
            var scale = ts.localScale;
            ts.DOScale(scale * scaleAmount, scaleDuration).SetEase(scaleEase).SetLoops(-1, LoopType.Yoyo);
        }

        if (shouldLevitate)
        {
            ts.DOLocalMoveY(ts.localPosition.y + levitationAmount, levitationDuration).SetEase(levitationEase).SetLoops(-1, LoopType.Yoyo);
        }
    }

    void StopAnimation()
    {
        transform.DOKill();
    }

    void OnDestroy()
    {
        StopAnimation();
    }
}
