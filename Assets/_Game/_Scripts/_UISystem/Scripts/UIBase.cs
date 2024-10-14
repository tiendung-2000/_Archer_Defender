using UnityEngine;

namespace sunnD
{
    public abstract class UIBase : MonoBehaviour
    {
        #region Variables

        public UILayer UILayer;
        public bool UsingSafeArea;
        public RectTransform SafePanel;
        public bool UsingStrechBackground;
        public RectTransform BackRect;
        public float DelayClick;

        public string UIID { get; set; }
        public bool IsShow { get; set; }
        public bool IsInitialize { get; set; }
        public bool FirstShowing { get; set; }

        public bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > lastTimeClick + DelayClick;
                if (result)
                {
                    lastTimeClick = Time.unscaledTime;
                }
                return result;
            }
        }

        private float lastTimeClick;
        private RectTransform cacheRect;

        #endregion

        #region Unity callback functions

        private void Awake()
        {
            if (cacheRect == null)
            {
                cacheRect = GetComponent<RectTransform>();
            }
            if (canvasRect == null)
            {
                canvasRect = GetComponentInParent<RectTransform>();
            }
        }

        private void OnEnable()
        {
            cacheRect.SetStretch();
            RefreshSafeArea();
            FitImageToScreen();
        }

        #endregion

        #region Background

        private Vector2 lastBackSize = Vector2.zero;
        private RectTransform canvasRect;

        private void FitImageToScreen()
        {
            if (UsingStrechBackground)
            {
                if (BackRect == null)
                {
                    Debug.LogWarning($"Check BackRect in {transform.name}");
                    return;
                }

                float canvasWidth = canvasRect.rect.width;
                float canvasHeight = canvasRect.rect.height;
                Vector2 cacheCanvas = new(canvasWidth, canvasHeight);

                if (cacheCanvas != lastBackSize)
                {
                    lastBackSize = cacheCanvas;

                    float imageOriginalWidth = 1080f;
                    float imageOriginalHeight = 1920f;

                    float canvasAspect = canvasWidth / canvasHeight;
                    float imageAspect = imageOriginalWidth / imageOriginalHeight;

                    if (canvasAspect < imageAspect)
                    {
                        BackRect.sizeDelta = new Vector2(canvasHeight * imageAspect, canvasHeight);
                    }
                    else
                    {
                        BackRect.sizeDelta = new Vector2(canvasWidth, canvasWidth / imageAspect);
                    }
                }
            }
        }

        #endregion

        #region Safe area

        private Rect lastSafeArea = new(0, 0, 0, 0);
        private Vector2Int lastScreenSize = new(0, 0);
        private ScreenOrientation lastOrientation = ScreenOrientation.AutoRotation;

        private void FixedUpdate()
        {
            RefreshSafeArea();
            FitImageToScreen();
        }

        private void RefreshSafeArea()
        {
            if (UsingSafeArea)
            {
                var safeArea = Screen.safeArea;
                if (safeArea != lastSafeArea || Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y || Screen.orientation != lastOrientation)
                {
                    lastSafeArea = safeArea;
                    lastScreenSize.x = Screen.width;
                    lastScreenSize.y = Screen.height;
                    lastOrientation = Screen.orientation;

                    ApplySafeArea();
                }
            }
        }

        private void ApplySafeArea()
        {
            if (SafePanel == null)
            {
                Debug.LogWarning($"Check SafePanel in {transform.name}");
                return;
            }

            var safeArea = Screen.safeArea;
            if (Screen.width > 0 && Screen.height > 0)
            {
                var anchorMin = safeArea.position;
                var anchorMax = safeArea.position + safeArea.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    SafePanel.anchorMin = anchorMin;
                    SafePanel.anchorMax = anchorMax;
                }
            }
        }

        #endregion

        #region Functions

        public virtual void Initialize()
        {
            IsInitialize = true;
            FirstShowing = true;
        }

        public void Show()
        {
            if (IsShow)
            {
                return;
            }
            gameObject.SetActive(true);
            ShowInside();
            if (FirstShowing)
            {
                FirstShowing = false;
                ShowFirst();
            }
            else
            {
                ShowNotFirst();
            }

            IsShow = true;
        }

        public void Hide()
        {
            if (!IsShow)
            {
                return;
            }
            gameObject.SetActive(false);
            IsShow = false;
        }

        public virtual void ShowInside()
        {
        }

        public virtual void ShowFirst()
        {
        }

        public virtual void ShowNotFirst()
        {
        }

        public virtual void HideInside()
        {
        }

        public virtual void OnBackPressed()
        {
        }

        #endregion
    }
}