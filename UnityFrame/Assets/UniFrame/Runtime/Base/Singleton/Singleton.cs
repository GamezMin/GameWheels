using System;

public abstract class Singleton<T>  where T : class, new()
{
    private bool _isInited = false;
    private static T _instance;
    private static object _lock = new object();
    
    protected Singleton()
    {
        if (null != _instance)
        {
            throw new Exception(_instance.ToString() + @" can not be created again.");
        }
        _isInited = false;
    }
    
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }

    public  bool IsInited => _isInited;

    public virtual bool Init()
    {
        _isInited = true;
        return _isInited;
    }
    
    public virtual void Uninit()
    {
        _isInited = false; 
    }
    
}