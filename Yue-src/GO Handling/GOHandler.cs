using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yue.GOHandling
{
    public class GOHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] objects;

        [Space]
        [SerializeField]
        private GameObject defaultObject;

        private List<GameObject> previous = new List<GameObject>();
        private GameObject current;

        //[SerializeField]
        //private bool setDefaults;
        // Start is called before the first frame update
        void Start()
        {
            //if (setDefaults)
            {
                current = defaultObject;
                current.SetActive(true);
                foreach (GameObject obj in objects)
                {
                    if (obj != defaultObject)
                        obj.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Goes to the next object, specified by an object name
        /// </summary>
        /// <param name="objectName">
        /// The name of the GameObject to switch next (set in the hierarchy of Unity)
        /// </param>
        public void NextObject(string objectName)
        {
            foreach (GameObject obj in objects)
            {
                if (obj.name == objectName)
                {
                    SetObject(obj);
                    return;
                }
            }

            Debug.LogError("Can't find object: " + objectName + " when iterating through objects of " + gameObject.name
                + "\nAre you sure that the object is valid in this container of GOHandler: " + gameObject.name + "?");
        }

        /// <summary>
        /// Goes to the next object, specified by an object index according to the object array
        /// </summary>
        /// <param name="objectIndex">
        /// The index of the GameObject to switch next
        /// </param>
        public void NextObject(int objectIndex)
        {
            if (objectIndex < objects.Length)
                SetObject(objects[objectIndex]);
            else
                Debug.LogError("Can't find object at index: " + objectIndex + " when iterating through objects of " + gameObject.name
                    + "\nAre you sure that the object is valid in this container of GOHandler: " + gameObject.name + "?");
        }

        /// <summary>
        /// Sets the current object to the previous object recorded in history (previous list)
        /// </summary>
        public void PreviousObject()
        {
            if (previous.Count > 0)
            {
                SetObject(previous[previous.Count - 1], false);
                //current.SetActive(false);
                //current = previous[previous.Count - 1];
                //current.SetActive(true);

                previous.RemoveAt(previous.Count - 1);
            }
        }

        public void DisableObject(string objectName)
        {
            foreach (GameObject obj in objects)
            {
                if (obj.name == objectName)
                {
                    obj.SetActive(false);
                    return;
                }
            }
            Debug.LogError("Can't find object: " + objectName + " when iterating through objects of " + gameObject.name
               + "\nAre you sure that the object is valid in this container of GOHandler: " + gameObject.name + "?");
        }

        public void EnableObject(string objectName)
        {
            foreach (GameObject obj in objects)
            {
                if (obj.name == objectName)
                {
                    EnableObject(obj);
                    //obj.SetActive(true);
                    return;
                }
            }
            Debug.LogError("Can't find object: " + objectName + " when iterating through objects of " + gameObject.name
               + "\nAre you sure that the object is valid in this container of GOHandler: " + gameObject.name + "?");
        }

        public void EnableObjectSingleton(string objectName)
        {
            foreach (GameObject obj in objects)
            {
                if (obj.name == objectName)
                {
                    DisableAllPreviousExceptDefault();
                    SetObject(obj);
                    //obj.SetActive(true);
                    return;
                }
            }

            Debug.LogError("Can't find object: " + objectName + " when iterating through objects of " + gameObject.name
              + "\nAre you sure that the object is valid in this container of GOHandler: " + gameObject.name + "?");
        }

        public void ToggleObject(string objectName, bool toggle)
        {
            foreach (GameObject obj in objects)
            {
                if (obj.name == objectName)
                {
                    obj.SetActive(toggle);
                    return;
                }
            }

            Debug.LogError("Can't find object: " + objectName + " when iterating through objects of " + gameObject.name
                + "\nAre you sure that the object is valid in this container of GOHandler: " + gameObject.name + "?");
        }

        /// <summary>
        /// Flushes all data from this object and resets
        /// This function should only be called OnDestroy or on memory release
        /// </summary>
        public void Flush()
        {
            previous.Clear();
            current = defaultObject;
        }

        public void DisableAllPrevious()
        {
            foreach (GameObject obj in previous)
                obj.SetActive(false);
        }
        public void DisableAllPreviousExceptDefault()
        {
            foreach (GameObject obj in previous)
            {
                if (obj != defaultObject)
                    obj.SetActive(false);
            }
        }

        private void EnableObject(GameObject obj, bool setPrevious = true)
        {
            if (setPrevious)
                previous.Add(current);

            //current.SetActive(false);

            obj.SetActive(true);
            current = obj;
        }

        /// <summary>
        /// Sets the current object to the next object
        /// </summary>
        /// <param name="obj">
        /// The object to set as the next object
        /// </param>
        /// <param name="setPrevious">
        /// Whether to add the current object as previous
        /// </param>
        private void SetObject(GameObject obj, bool setPrevious = true)
        {
            DisableAllPrevious();
            if (setPrevious)
                previous.Add(current);

            current.SetActive(false);

            obj.SetActive(true);
            current = obj;
        }
    }
}