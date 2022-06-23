using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yue.DesignPatterns
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;
        private static readonly object instanceLock = new object();
        private static bool quitting = false;

        public static T Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (!quitting)
                    {
                        if (instance == null)
                        {
                            GameObject newInstance = new GameObject(typeof(T).ToString());
                            instance = newInstance.AddComponent<T>();

                            DontDestroyOnLoad(instance);
                        }

                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = gameObject.GetComponent<T>();
                DontDestroyOnLoad(instance);
            }
            else if (instance.GetInstanceID() != GetInstanceID())
            {
                Destroy(gameObject);
                throw new System.Exception(string.Format("Instance of {0} already exists, removing {1}", GetType().FullName, ToString()));
            }
        }

        protected virtual void OnApplicationQuit()
        {
            quitting = true;
        }
    }
}