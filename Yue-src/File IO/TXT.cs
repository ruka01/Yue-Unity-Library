using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;

namespace Yue.FileIO
{
    public class TXT : FileBase
    {
        /// <summary>
        /// Class to define one header in txt
        /// </summary>
        private class TXTHeaderObject : FileObject
        {
            public TXTHeaderObject()
            {

            }
            public TXTHeaderObject(string header, HeaderType type = HeaderType.ENTIRE_LINE)
            {
                this.header = header;
            }

            public enum HeaderType
            {
                EQUALS,     // E.g. "volume = 0.5f"
                ENTIRE_LINE // E.g. "helloworld"
            }

            public HeaderType headerType;

            public override void AddContent(string add)
            {
                content.Add(add);
            }
            //public string header;
            //public List<string> content = new List<string>();
        }

        //private List<TXTHeaderObject> headerObjects = new List<TXTHeaderObject>();

        
        //public override bool Read(string path)
        //{
        //    if (File.Exists(path))
        //    {
        //        txtInformation = File.ReadAllLines(path);
        //        ReadHeaderObjects(txtInformation);
        //        return true;
        //    }
        //    else
        //        File.Create(path).Dispose();

        //    return false;
        //}

        /// <summary>
        /// Reads all headers objects in a given txt file
        /// </summary>
        /// <param name="data">
        /// All the data in a txt file
        /// </param>
        protected override void ReadFileObjects(string[] data)
        {
            try
            {
                for (int i = 0; i < data.Length; ++i)
                {
                    // Check if there is any valid data
                    if (IsValidData(data[i]))
                    {
                        // Found a header: First element of header should be [Header]
                        if (data[i][0] == '[')
                        {
                            // Get the header's name to determine which function to 
                            // delegate to
                            List<char> toRemove = new List<char> { '[', ']' };
                            string headerName = Filter(data[i], toRemove);

                            TXTHeaderObject newHeaderObject = new TXTHeaderObject();
                            newHeaderObject.header = headerName;
                            fileObjects.Add(newHeaderObject);
                            // Go to the next element
                            ++i;

                            bool isEqual = data[i].Contains("=");

                            if (isEqual)
                                newHeaderObject.headerType = TXTHeaderObject.HeaderType.EQUALS;
                            else
                                newHeaderObject.headerType = TXTHeaderObject.HeaderType.ENTIRE_LINE;

                            // Continuously loop while not reaching empty space empty space
                            while (data[i].Length > 0)
                            {
                                if (data[i][0] != '[' && IsValidData(data[i]))
                                {
                                    // If header is objects, we don't want metadata names.
                                    if (headerName.CompareTo(headerName) == 0)
                                        newHeaderObject.AddContent(data[i]);
                                }
                                else // if reached a header
                                {
                                    break;
                                }
                                // Add one to loop to go to the next element in this while
                                ++i;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
                //Debug.Log("Exception: " + e.Message);
            }
        }

        private bool IsValidData(string line)
        {
            if (line.Length <= 0 || line[0] == '/')
                return false;

            return true;
        }
        /// <summary>
        /// Temporarily writes the data specified into this class
        /// WARNING: This does not write to the txt file in the folder.
        /// </summary>
        /// <param name="header">
        /// The name of the header to write to
        /// </param>
        /// <param name="data">
        /// What data to write
        /// </param>
        public override void Write(string header, string data)
        {
            TXTHeaderObject headerObject = (TXTHeaderObject)GetFileObject(header);

            headerObject.AddContent(data);
            //for (int i = 0; i < headerObjects.Count; ++i)
            //{
            //    if (headerObjects[i].header.CompareTo(header) == 0)
            //        headerObjects[i].content.Add(data);
            //}
        }

        /// <summary>
        /// Clears all content below a specified header
        /// </summary>
        /// <param name="header">
        /// The name of the header to clear
        /// </param>
        public void Clear(string header)
        {
            GetFileObject(header).Content.Clear();
            //for (int i = 0; i < headerObjects.Count; ++i)
            //{
            //    if (headerObjects[i].header.CompareTo(header) == 0)
            //        headerObjects[i].content.Clear();
            //}
        }

        /// <summary>
        /// Saves current text information into specified path.
        /// WARNING: If you have not loaded anything into the variable txtInformation, it may overwrite the entire file and make it empty. 
        /// </summary>
        /// <param name="path">
        /// The specified folder path
        /// </param>
        /// <param name="append">
        /// Whether to append to end of file
        /// </param>
        public override void Save(string path, bool append = false)
        {
            string content = "// Last edited: " + DateTime.Now.ToString() + "\n";

            foreach (TXTHeaderObject ho in fileObjects)
            {
                content += "[" + ho.header + "]\n";

                foreach (string c in ho.Content)
                {
                    content += c + "\n";
                }

                content += "\n";
            }

            if (!File.Exists(path))
                File.Create(path).Dispose();

            StreamWriter writer = new StreamWriter(path, append);
            writer.WriteLine(content);
            writer.Close();
        }

        /// <summary>
        /// Reads the entire line from a specified header
        /// </summary>
        /// <param name="data">
        /// All data lines in txt file
        /// </param>
        /// <param name="header">
        /// The information below the header you want
        /// </param>
        /// <returns>
        /// All lines below this header only
        /// </returns>
        public string[] ReadHeaderSingleLine(string header)
        {
            try
            {
                List<string> returnData = new List<string>();
                List<string> data = GetFileObject(header).Content;

                for (int i = 0; i < data.Count; ++i)
                {
                    // Check if there even any data
                    if (data[i].Length > 0)
                        returnData.Add(data[i]);
                }

                return returnData.ToArray();
            }
            catch (Exception e)
            {
                throw e;
                //Debug.Log("Exception: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Reads the entire line specified by header and returns a Tuple (pair)
        /// For example, in the txt file the line would be represented as "accuracy=0.5"
        /// And the tuple will return "accuracy" and "0.5"
        /// </summary>
        /// <param name="data"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public Tuple<string, string>[] ReadHeaderEqual(string header)
        {
            try
            {
                List<Tuple<string, string>> returnData = new List<Tuple<string, string>>();
                List<string> data = GetFileObject(header).Content;

                for (int i = 0; i < data.Count; ++i)
                {
                    // Check if there even any data
                    if (data[i].Length > 0)
                        returnData.Add(ReadMetaDataEqual(data[i]));
                }

                return returnData.ToArray();
            }
            catch (Exception e)
            {
                throw e;
                //Debug.Log("Exception: " + e.Message);
            }
        }

        private string Filter(string str, List<char> charsToRemove)
        {
            foreach (char c in charsToRemove)
            {
                str = str.Replace(c.ToString(), string.Empty);
            }

            return str;
        }

        private Tuple<string, string> ReadMetaDataEqual(string data)
        {
            string metaDataName = "";
            string actualValue = "";

            bool foundName = false;
            for (int i = 0; i < data.Length; ++i)
            {
                if (!foundName)
                {
                    // Look for the =
                    while (data[i] != '=')
                    {
                        // Add to the metadata's name
                        metaDataName += data[i];
                        ++i;
                    }
                    foundName = true;

                    // If the current letter is =, just skip one
                    if (data[i] == '=')
                        ++i;
                    else
                    {
                        // Skip until after the =
                        do { ++i; } while (data[i] != '=');
                    }
                }

                // At this point, should be left the actual value only
                actualValue += data[i];
            }

            return new Tuple<string, string>(metaDataName, actualValue);
        }

        protected override FileObject GetFileObject(string headerName)
        {
            for (int i = 0; i < fileObjects.Count; ++i)
            {
                if (fileObjects[i].header.CompareTo(headerName) == 0)
                    return fileObjects[i];
            }

            TXTHeaderObject headerObject = new TXTHeaderObject(headerName);
            headerObject.header = headerName;
            fileObjects.Add(headerObject);

            return headerObject;
            //Debug.LogError("Unable to find header " + headerName);
            //return null;
        }
    }
}