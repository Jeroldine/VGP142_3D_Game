using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<T>();
            if (!instance)
            {
                GameObject go = new GameObject();
                go.name = typeof(T).Name;
                instance = go.AddComponent<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
}
