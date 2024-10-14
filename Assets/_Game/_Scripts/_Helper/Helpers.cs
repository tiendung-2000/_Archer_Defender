using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Helpers
{
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if(_camera == null) _camera = Camera.main;
            return _camera;
        }
    }
    static public int ToInt(this char c)
    {
        return (int)(c - '0');
    }
    /// <summary>
    // Save WaitForSeconds func memory
    /// </summary>
    private static readonly Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if(waitDictionary.TryGetValue(time, out var wait)) return wait;
        waitDictionary[time] = new WaitForSeconds(time);
        return waitDictionary[time];
    }
    /// <summary>
    // check OverUI
    /// </summary>
    private static PointerEventData _enventDataCurrentPostion;
    private static List<RaycastResult> _raycastResults;
    public static bool isOverUI()
    {
        _enventDataCurrentPostion = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        _raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_enventDataCurrentPostion,_raycastResults); ;
        return _raycastResults.Count() > 0;
        //x => x.gameObject.layer == LayerMask.NameToLayer("UI")
    }

    /// <summary>
    // Canvas Helper
    /// </summary>
    public static Vector3 GetWoldPositionOfCanvasElement(RectTransform elment)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(elment, elment.position, Camera, out var result);
        return result;
    }
    //public static Vector2 WorldToCanvas(this Canvas canvas,
    //                                    Vector3 world_position,
    //                                    Camera camera = null)
    //{
    //    if (camera == null)
    //    {
    //        camera = Camera.main;
    //    }

    //    var viewport_position = camera.WorldToViewportPoint(world_position);
    //    var canvas_rect = canvas.GetComponent<RectTransform>();

    //    return new Vector2((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
    //                       (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
    //}

    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }
    public static string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        int hundredths = d % 100;
        return String.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }
    public static string FormatTimeSpanToHour(TimeSpan _timer)
    {
        return string.Format("{0:00}:{1:00}:{2:00}", _timer.Days * 24 + _timer.Hours, _timer.Minutes, _timer.Seconds);
    }
    public static string FormatTimeSpanToMinute(TimeSpan _timer)
    {
        return string.Format("{0:00}:{1:00}:{2:00}", _timer.Hours, _timer.Minutes, _timer.Seconds);
    }

    public static string FormatSecondsUI(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        int hundredths = d % 100;
        return String.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public static string FormatDistance(float _distance)
    {
        return String.Format("{0:00.00}km", _distance);
    }
    public static void BringChildIntoView(RectTransform child, RectTransform contentPanel, RectTransform viewport, ScrollRect scrollRect, bool _immediately = false, float duration = 0.5f)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = viewport.localPosition;
        Vector2 childLocalPosition = child.localPosition;
        Vector2 result = new Vector2(
            0 - (viewportLocalPosition.x + childLocalPosition.x),
            0 - (viewportLocalPosition.y + childLocalPosition.y)
        );
        Vector2 temp = new Vector2(result.x, contentPanel.localPosition.y);
        //contentPanel.localPosition = ;
        if (contentPanel.gameObject.activeInHierarchy && !_immediately)
        {
            contentPanel.DOKill();
            contentPanel.DOLocalMove(temp, duration).OnUpdate(() =>
            {
                scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition, 0f, 1f);
                scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
            });
        }
        else
        {
            contentPanel.localPosition = new Vector2(result.x, contentPanel.localPosition.y);
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition, 0f, 1f);
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
        }
    }

}
/// <summary>
/// This transforms the static instance into a basic singleton. This will destroy any new
/// versions created, leaving the original instance intact
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        base.Awake();
    }
}

/// <summary>
/// A static instance is similar to a singleton, but instead of destroying any new
/// instances, it overrides the current instance. This is handy for resetting the state
/// and saves you doing it manually
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance?.StopAllCoroutines();
        Instance = null;
        Destroy(gameObject);
    }
    protected virtual void OnDestroy()
    {
        Instance?.StopAllCoroutines();
        Instance = null;
    }
}

/// <summary>
/// Finally we have a persistent version of the singleton. This will survive through scene
/// loads. Perfect for system classes which require stateful, persistent data. Or audio sources
/// where music plays through loading screens, etc
/// </summary>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
