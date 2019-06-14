using UnityEngine;

/// <summary>
/// 单例模板
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this as T;
    }
}