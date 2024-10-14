using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;

public static class TextExtension
{
    public static void DONumber(this Text target,float _duration, float startValue, float endValue, Action actionOnDone = null)
    {
        target.DONumber(startValue, endValue, _duration, "#0", actionOnDone);
    }

    public static void DONumber(this Text target, float startValue, float endValue, float duration, string format = "#0", Action actionOnDone = null)
    {
        if (target == null)
        {
            return;
        }
        if (!target.gameObject.activeInHierarchy) 
        { 
            target.text = endValue.ToString(format);
        }
        else
        {
            DOTween.Complete(target.GetInstanceID());
            target.DOComplete();
            target.transform.DOComplete();
            DOVirtual.Float(startValue, endValue, duration, delegate (float s)
            {
                target.text = s.ToString(format);
            }).SetId(target.GetInstanceID()).OnComplete(delegate
            {
                target.transform.DOScale(1.1f, 0.125f).SetEase(Ease.InCubic).OnComplete(delegate
                {
                    target.transform.DOScale(1f, 0.25f).SetEase(Ease.OutCubic).OnComplete(delegate
                    {
                        actionOnDone?.Invoke();
                    });
                });
            });
        }
    }
}