using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yue.FileIO;
using System;

namespace Yue.DataCollection
{
    public class DataCollection : DesignPatterns.Singleton<DataCollection>
    { 
        private CSV csv;
        public CSV CSV { get { return csv; } }

        private FileObject currentObject = null;

        protected override void Awake()
        {
            base.Awake();

            csv = new CSV();
            //string time = DateTime.Now.Date.ToString();
            string time = DateTime.Now.ToString("dd-MM-yyyy");
            //time = time.Replace(" ", "--");
            //time = time.Replace("/", "-");
            //time = time.Replace(":", "-");
            csv.Read(Application.persistentDataPath + "/" + time + ".csv");

            NewData("Time Level Started", "Time Level Ended", "Level", "Time Taken", "Times Collided", "Stars", "Finish Count", "Retry Count");
        }

        public void NewData(string data)
        {
            csv.CreateHeader(data);
        }
        public void NewData(params string[] data)
        {
            foreach (string s in data)
                csv.CreateHeader(s);
        }

        public void WriteCurrent(string header, string data)
        {
            if (currentObject == null)
                currentObject = csv.WriteNewObject();

            csv.WriteOn(currentObject, header, data);
        }

        public void WriteNew(string header, string data)
        {
            currentObject = csv.WriteNewObject();
            csv.WriteOn(currentObject, header, data);
        }

        public void Save()
        {
            string time = DateTime.Now.ToString("dd-MM-yyyy");
            //time = time.Replace(" ", "--");
            //time = time.Replace("/", "-");
            //time = time.Replace(":", "-");
            csv.Save(Application.persistentDataPath + "/" + time + ".csv");
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            Save();
        }
    }
}