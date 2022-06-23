using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yue.DesignPatterns
{
    /// <summary>
    /// Inherit this class to use pooling.
    /// The template object must inherit MonoBehaviour to work with Unity.
    /// </summary>
    /// <typeparam name="T">
    /// Template for the class you are pooling
    /// </typeparam>
    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] protected GameObject objectToPool;
        [SerializeField] protected int poolCount = 10;
        [SerializeField] protected bool enableObjects = false;

        protected List<T> pool = new List<T>();

        // Start is called before the first frame update
        protected void Awake()
        {
            CreateObjects(poolCount);
        }

        protected virtual void CreateObjects(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                GameObject newObject = Instantiate(objectToPool);
                newObject.SetActive(enableObjects);
                newObject.transform.SetParent(transform);
                pool.Add(newObject.GetComponent<T>());
            }
        }

        public T Get()
        {
            for (int i = 0; i < pool.Count; ++i)
            {
                if (!pool[i].gameObject.activeSelf)
                {
                    T poolObject = pool[i];
                    poolObject.gameObject.SetActive(true);
                    return poolObject;
                }
            }

            CreateObjects(10);
            return Get();
        }

        public virtual void Return(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        public void Print()
        {
            Debug.Log("HELLO! Reflection called");
        }

        public void PrintWithParam(string param1, string param2)
        {
            Debug.Log("HELLO! Reflection called with parameters: " + param1 + " & " + param2);
        }

        private void OnDestroy()
        {
            foreach (T p in pool)
                Destroy(p.gameObject);
        }
    }
}