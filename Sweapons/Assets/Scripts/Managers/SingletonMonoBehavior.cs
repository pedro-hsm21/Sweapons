using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    _instance = new GameObject("Singleton").AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {

        if (SingletonMonoBehavior<T>._instance != null)
            Destroy(this);
    }
}
