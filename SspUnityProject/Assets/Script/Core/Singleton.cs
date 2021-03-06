﻿using UnityEngine;


public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static readonly string PREFIX = @"[Singleton] " + typeof(T);
    private static T _instance;
    //private static readonly object _lock = new object();
    protected static bool isAppQuitting = false;
    public static bool IsAppQuitting
    {
        get { return isAppQuitting; }
    }

    public static T Instance
    {
        get
        {
            if (IsAppQuitting)
            {
                LogError("already destroyed on application quit.");
                return null;
            }

            if (_instance == null)
            {
                LogWarning("should call Create() in VRApplication.CreateSingleton");
            }

            return _instance;
        }

    }
    public static T Create()
    {
        if (_instance == null)
        {
            isAppQuitting = false;
            GameObject singleton = new GameObject();
            _instance = singleton.AddComponent<T>();
            singleton.name = PREFIX;
            singleton.hideFlags = HideFlags.DontSave;
            DontDestroyOnLoad(singleton);
            Instance.OnInitialize();
            Log("Created.");
        }
        else
        {
            LogWarning("already created, not need to create again.");
        }
        return Instance;
    }

    public static T Create(GameObject obj)
    {
        if (_instance == null)
        {
            isAppQuitting = false;
            _instance = obj.AddComponent<T>();
            obj.name = PREFIX + obj.name;
            obj.hideFlags = HideFlags.DontSave;
            DontDestroyOnLoad(obj);
            Instance.OnInitialize();
            Log("Created.");
        }
        else
        {
            LogWarning("already created, not need to create again.");
        }
        return Instance;
    }

    public static void Destroy()
    {
        GameObject.Destroy(_instance.gameObject);
    }

    void OnDestroy()
    {
        isAppQuitting = true;
        _instance.OnUninitialize();
        _instance = null;
        Log("Destroy.");
    }

    public abstract void OnInitialize();

    public abstract void OnUninitialize();


    protected static void Log(string message)
    {
        Debug.LogFormat(@"{0} {1}", PREFIX, message);
    }
    protected static void LogWarning(string message)
    {
        Debug.LogWarningFormat(@"{0} {1}", PREFIX, message);
    }
    protected static void LogError(string message)
    {
        Debug.LogErrorFormat(@"{0} {1}", PREFIX, message);
    }
}

