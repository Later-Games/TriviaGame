using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Transform loadingImage;
    void Start()
    {
        loadingImage.transform.DORotate(Vector3.back * 90f, 0.25f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }
    
    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
