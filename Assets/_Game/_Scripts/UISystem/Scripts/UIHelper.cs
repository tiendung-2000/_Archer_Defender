using System;
using UnityEngine;

namespace sunnD
{
    [Flags]
    public enum UsingTween
    {
        None, Show = 1 << 1, Hide = 1 << 2
    }

    public enum UILayer
    {
        Bottom, Menu, PopupBottom, PopupMiddle, PopupTop, Top
    }

    public enum GameUI
    {
        // Done
        Home, Setting, PlayerInfo, Banner, InternetConnect, Spin, Daily, OnlineGift, Shop, Level, Ads, Loading, Gameplay, Pause,













        // Fix
        Win,













        // Not now
        EndGame,
    }

    public static class UIHelper
    {
        public static void SetStretch(this RectTransform rect)
        {
            rect.transform.localScale = Vector3.one;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
