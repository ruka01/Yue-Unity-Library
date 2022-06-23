using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Yue.FileIO
{
    public abstract class FileBase
    {
        private bool isNewFile = true;
        public bool IsNewFile { get { return isNewFile; } }

        protected string[] fileData;
        public string[] FileData { get { return fileData; } }

        protected List<FileObject> fileObjects = new List<FileObject>();

        /// <summary>
        /// Reads the whole txt file from specified path
        /// </summary>
        /// <param name="path">
        /// The specified folder path
        /// </param>
        /// <returns>
        /// string data in each line represented in string array
        /// </returns>
        public bool Read(string path)
        {
            if (File.Exists(path))
            {
                fileData = File.ReadAllLines(path);
                ReadFileObjects(fileData);
                isNewFile = false;
                return true;
            }
            else
                File.Create(path).Dispose();

            return false;
        }

        public abstract void Write(string header, string data);

        public abstract void Save(string path, bool append = false);

        protected abstract void ReadFileObjects(string[] data);

        protected abstract FileObject GetFileObject(string headerName);

        public string[] GetFileObjectContent(string header)
        {
            foreach (FileObject ho in fileObjects)
            {
                if (ho.header == header)
                    return ho.Content.ToArray();
            }

            throw new Exception("Unable to find specified header of: " + header);
        }
    }

    public abstract class FileObject
    {
        public string header;
        protected List<string> content = new List<string>();
        public List<string> Content { get { return content; } }

        public abstract void AddContent(string add);
    }
}