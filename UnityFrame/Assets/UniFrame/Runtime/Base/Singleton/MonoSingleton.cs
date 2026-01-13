using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
{
    protected static T _Instance = null;
    private static bool _IsApplicationQuiting = false;

    public static T Instance
    {
        get
        {
            if (_Instance == null && !_IsApplicationQuiting)
            {
                _Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (_Instance == null)
                {
                    _Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    if (!_Instance)
                    {
                        Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                    }
                }
            }
            return _Instance;
        }
        protected set { _Instance = value; }
    }

    private bool _isInitialized = false;

    public bool IsInitialized
    {
        get { return _isInitialized; }
    }
    
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this as T;
        }
        else if (_Instance != this)
        {
            Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
            DestroyImmediate(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        if (!IsInitialized)
        {
            Init();
        }
    }

    private void OnDestroy()
    {
        if (_Instance == this)
        {
            if (IsInitialized)
            {
                Uninit();
            }
            _Instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        _IsApplicationQuiting = true;
    }
    
    public virtual bool Init()
    {
        _isInitialized = true;
        return true;
    }

    public virtual void Uninit()
    {
        _isInitialized = false;
    }
}