using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sunnD
{
    public class UIController : MonoBehaviour
    {
        #region Singleton

        public bool DontDestroy;

        private static UIController instance;
        public static UIController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<UIController>();

                    if (instance == null)
                    {
                        var singletonObject = new GameObject($"[Singleton - {nameof(UIController)}]");
                        instance = singletonObject.AddComponent<UIController>();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Variables

        private static readonly Dictionary<string, UIBase> loadedUI = new();
        private static readonly List<Transform> uiLayerParents = new();
        private static readonly Stack<UIBase> stackUI = new();
        private static readonly List<UIBase> listUI = new();

        public GameUI StartScreen;
        public string UIPath = "UI/";
        public bool QuitInMenu = true;
        public int QuitCountBack = 2;
        public float DelayQuitBack = 1.5f;

        private int cacheCountBack;
        private float cacheDelayTime;

        public bool Initialized { get; set; }

        #endregion

        #region Unity callback functions

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                if (DontDestroy)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Debug.LogWarning($"Another instance of {nameof(UIController)} is already exist! Destroying self...");
                Destroy(gameObject);
            }

            CheckUIInScene();
        }

        private void CheckUIInScene()
        {
            var layerNames = Enum.GetNames(typeof(UILayer));
            foreach (var layerName in layerNames)
            {
                var layer = transform.Find(layerName);
                if (layer == null)
                {
                    layer = new GameObject(layerName, typeof(RectTransform)).GetComponent<RectTransform>();
                    layer.SetParent(transform);
                    layer.GetComponent<RectTransform>().SetStretch();
                }
                uiLayerParents.Add(layer);
            }
            var allUIInScene = transform.GetComponentsInChildren<UIBase>();
            foreach (UIBase ui in allUIInScene)
            {
                var nameUI = ui.gameObject.name;
                ui.UIID = nameUI;
                ui.Initialize();
                var layerName = Enum.GetName(typeof(UILayer), ui.UILayer);
                var layer = uiLayerParents.Find(o => o.gameObject.name == layerName);
                ui.transform.SetParent(layer);
                ui.IsShow = true;
                ui.Hide();
                loadedUI.Add(nameUI, ui);
            }
        }

        private void Start()
        {
            ShowUI(StartScreen);
            Initialized = true;
        }

        private void Update()
        {
            if (QuitInMenu && cacheDelayTime > 0)
            {
                cacheDelayTime -= Time.deltaTime;
                if (cacheDelayTime <= 0f)
                {
                    cacheCountBack = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) && stackUI != null && stackUI.Count > 0)
            {
                if (stackUI.Count == 1 && QuitInMenu)
                {
                    cacheCountBack++;
                    if (cacheCountBack == QuitCountBack)
                    {
                        Debug.Log("Quit");
                        Application.Quit();
                    }
                    else
                    {
                        cacheDelayTime = DelayQuitBack;
                    }
                }
                else
                {
                    stackUI.Peek().OnBackPressed();
                }
            }
        }

        #endregion

        #region Control functions

        public static bool IsLastUI(GameUI ui)
        {
            if (stackUI.Count != 1)
            {
                var currentUI = stackUI.Pop();
                var lastUI = stackUI.Peek();
                stackUI.Push(currentUI);
                if (lastUI.UIID == Enum.GetName(typeof(GameUI), ui))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsCurrentUI(GameUI ui)
        {
            if (stackUI.Count != 1)
            {
                var currentUI = stackUI.Peek();
                if (currentUI.UIID == Enum.GetName(typeof(GameUI), ui))
                {
                    return true;
                }
            }
            return false;
        }

        public static UIBase GetUI(GameUI ui)
        {
            foreach (var uiBase in stackUI)
            {
                if (uiBase.UIID == Enum.GetName(typeof(GameUI), ui))
                {
                    return uiBase;
                }
            }
            return null;
        }

        public static UIBase GetLoadedUI(GameUI ui)
        {
            foreach (var (_, value) in loadedUI)
            {
                if (value.UIID == Enum.GetName(typeof(GameUI), ui))
                {
                    return value;
                }
            }
            return null;
        }

        public static void LoadUI(GameUI ui, bool delayDeactive = false)
        {
            var nameUI = Enum.GetName(typeof(GameUI), ui);
            LoadUI(nameUI, delayDeactive);
        }

        public static void LoadUI(string nameUI, bool delayDeactive = false)
        {
            if (!loadedUI.TryGetValue(nameUI, out var newUI))
            {
                var uiPrefab = Resources.Load<UIBase>(Instance.UIPath + nameUI);
                newUI = Instantiate(uiPrefab, uiLayerParents[(int)uiPrefab.UILayer]);
                newUI.UIID = nameUI;
                if (!newUI.IsInitialize)
                {
                    newUI.Initialize();
                }
                loadedUI.Add(nameUI, newUI);
                newUI.transform.SetAsLastSibling();
                if (delayDeactive)
                {
                    Instance.StartCoroutine(DelayDeactive(newUI.gameObject));
                }
                else
                {
                    newUI.gameObject.SetActive(false);
                }
            }
            return;

            static IEnumerator DelayDeactive(GameObject target)
            {
                yield return new WaitForSeconds(0.5f);
                target.SetActive(false);
            }
        }

        public static void ShowUI(string nameUI, bool hideCurrentUI = true, int overrideChildIndex = 99)
        {
            if (stackUI.Count > 0)
            {
                var oldScreen = stackUI.Peek();
                if (hideCurrentUI)
                {
                    oldScreen.Hide();
                }
            }

            if (loadedUI.TryGetValue(nameUI, out var newUI))
            {
                if (overrideChildIndex != 99)
                {
                    newUI.transform.SetSiblingIndex(overrideChildIndex);
                }
                else
                {
                    newUI.transform.SetAsLastSibling();
                }
                newUI.Show();
            }
            else
            {
                var uiPrefab = Resources.Load<UIBase>(Instance.UIPath + nameUI);
                newUI = Instantiate(uiPrefab, uiLayerParents[(int)uiPrefab.UILayer]);
                newUI.UIID = nameUI;
                if (!newUI.IsInitialize)
                {
                    newUI.Initialize();
                }
                loadedUI.Add(nameUI, newUI);

                if (overrideChildIndex != 99)
                {
                    newUI.transform.SetSiblingIndex(overrideChildIndex);
                }
                else
                {
                    newUI.transform.SetAsLastSibling();
                }
                newUI.Show();
            }

            stackUI.Push(newUI);
        }

        public static void ShowUI(GameUI ui, bool hideCurrentUI = true, int overrideChildIndex = 99)
        {
            var nameUI = Enum.GetName(typeof(GameUI), ui);
            ShowUI(nameUI, hideCurrentUI, overrideChildIndex);
        }

        public static void ShowUIWithoutStack(string nameUI, int overrideChildIndex = 99)
        {
            if (listUI.Find(o => o.UIID == nameUI))
            {
                return;
            }

            if (loadedUI.TryGetValue(nameUI, out var newUI))
            {
                if (overrideChildIndex != 99)
                {
                    newUI.transform.SetSiblingIndex(overrideChildIndex);
                }
                else
                {
                    newUI.transform.SetAsLastSibling();
                }
                newUI.Show();
            }
            else
            {
                var uiPrefab = Resources.Load<UIBase>(Instance.UIPath + nameUI);
                newUI = Instantiate(uiPrefab, uiLayerParents[(int)uiPrefab.UILayer]);
                newUI.UIID = nameUI;
                if (!newUI.IsInitialize)
                {
                    newUI.Initialize();
                }
                loadedUI.Add(nameUI, newUI);

                if (overrideChildIndex != 99)
                {
                    newUI.transform.SetSiblingIndex(overrideChildIndex);
                }
                else
                {
                    newUI.transform.SetAsLastSibling();
                }
                newUI.Show();
            }

            listUI.Add(newUI);
        }

        public static void ShowUIWithoutStack(GameUI ui, int overrideChildIndex = 99)
        {
            var nameUI = Enum.GetName(typeof(GameUI), ui);
            ShowUIWithoutStack(nameUI, overrideChildIndex);
        }

        public static void HideUI(bool showPeekScreen = true)
        {
            if (stackUI.Count > 1)
            {
                var popScreen = stackUI.Pop();
                popScreen.Hide();

                var peekScreen = stackUI.Peek();
                if (showPeekScreen)
                {
                    peekScreen.Show();
                }
            }
        }

        public static void HideUIWithoutStack(string nameUI)
        {
            var uiHide = listUI.Find(o => o.UIID == nameUI);
            if (uiHide != null)
            {
                uiHide.Hide();
                listUI.Remove(uiHide);
            }
        }

        public static void HideUIWithoutStack(GameUI ui)
        {
            var nameUI = Enum.GetName(typeof(GameUI), ui);
            HideUIWithoutStack(nameUI);
        }

        public static void HideAllUI()
        {
            var totalInStack = stackUI.Count;
            while (totalInStack > 2)
            {
                HideUI(false);
                totalInStack--;
            }

            HideUI();
        }

        #endregion
    }
}