using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScroll : MonoBehaviour
{
    public static TextScroll Ins;
    private float speed;
    [SerializeField]
    private TextMeshProUGUI textComponent;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private float delayTime;
    private float width;

    Coroutine coroutine;

    private void Awake()
    {
        OnInit();
    }

    private void OnEnable()
    {
        OnInit();
    }

    public void OnInit()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        speed = 5f;
        rectTransform.anchoredPosition = Vector2.zero;
        textComponent.ForceMeshUpdate();
        width = textComponent.textBounds.size.x;
        if (width > rectTransform.rect.width)
        {
            coroutine = StartCoroutine(ScrollText());
        }
    }
    
    IEnumerator ScrollText()
    {
        while (true)
        {
            rectTransform.anchoredPosition -= new Vector2(speed, 0f);

            if (rectTransform.anchoredPosition.x <= -width)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.rect.width, 0f);
            }

            yield return null;
        }
    }
}
