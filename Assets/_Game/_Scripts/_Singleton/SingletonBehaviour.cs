using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable CheckNamespace

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    #region Variables

    [field: BoxGroup, SerializeField] public bool DontDestroy { get; set; }

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    var singletonObject = new GameObject($"[Singleton - {typeof(T).Name}]");
                    _instance = singletonObject.AddComponent<T>();
                    _instance.DontDestroy = true;
                }
            }

            return _instance;
        }
    }

    #endregion

    #region Unity callback functions

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            LoadInAwake();
            if (DontDestroy) DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Another instance of " + GetType().Name + " is already exist! Destroying self...");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadInStart();
    }

    public virtual void LoadInAwake()
    {
    }

    public virtual void LoadInStart()
    {
    }

    #endregion
}