using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

namespace Yue.FileIO
{
    public class CSV : FileBase
    {
        public CSV()
        {
        }

        string fileName = "";
        private List<CSVHeaderObject> headers = new List<CSVHeaderObject>();

        // Represents one grid in the excel sheet.
        private class CSVObject
        {
            public string header;
            public string content;
        }

        private class CSVHeaderObject : FileObject
        {
            public CSVHeaderObject(string header)
            {
                this.header = header;
            }

            public override void AddContent(string add)
            {
                throw new NotImplementedException();
            }
        }

        private class CSVColumnObject : FileObject
        {
            private List<CSVObject> contentObjects = new List<CSVObject>();

            public CSVColumnObject(int index)
            {
                this.index = index;
                header = index.ToString();
            }

            public int index;

            public override void AddContent(string add)
            {
                content.Add(add);
            }

            public void InitializeCell(string header)
            {
                CSVObject obj = new CSVObject();
                obj.header = header;
                contentObjects.Add(obj);
            }

            public void ReplaceCellContent(string header, string replace)
            {
                foreach (CSVObject col in contentObjects)
                {
                    if (col.header.CompareTo(header) == 0)
                    {
                        col.content = replace;
                        return;
                    }
                }

                Debug.LogError("Unable to find header: " + header + "\n" +
                    "Does it not exist?");
            }

            public string GetAllCellContent()
            {
                List<string> cells = new List<string>();
                foreach (CSVObject col in contentObjects)
                {
                    cells.Add(col.content);
                }

                return string.Join(",", cells);
            }
        }

        public override void Write(string header, string data)
        {
            CSVColumnObject headerObject = (CSVColumnObject)GetFileObject(header);
            headerObject.Content.Add(data);
        }

        public FileObject WriteNewObject()
        {
            return GetNewFileObject(fileObjects.Count.ToString());
        }

        public void WriteOn(FileObject fileObject, string header, string data)
        {
            CSVColumnObject colObject = (CSVColumnObject)fileObject;
            colObject.ReplaceCellContent(header, data);
        }

        protected override void ReadFileObjects(string[] data)
        {
            if (data.Length <= 0)
                throw new Exception("No data to parse.");

            string[] headerStrings = data[0].Split(',');
            foreach (string h in headerStrings)
            {
                CSVHeaderObject headerObject = new CSVHeaderObject(h);
                headers.Add(headerObject);
            }

            //int headerObjectCount = headerObjects.Count;
            for (int i = 1; i < data.Length; ++i)
            {
                string[] splitData = data[i].Split(',');

                CSVColumnObject columnObject = (CSVColumnObject)GetNewFileObject(i.ToString());

                //CSVColumnObject columnObject = new CSVColumnObject(i);
                //fileObjects.Add(columnObject);

                for (int h = 0; h < splitData.Length; ++h)
                {
                    columnObject.ReplaceCellContent(headers[h].header, splitData[h]);
                }
            }
        }

        public void CreateHeader(string headerName)
        {
            if (IsNewFile)
            {
                CSVHeaderObject headerObject = new CSVHeaderObject(headerName);
                headers.Add(headerObject);
            }
            else
                Debug.LogWarning("This CSV file is NOT a new file. If you create new headers, it may affect the existing data." +
                    "Creating new headers on an existing CSV with data is not allowed.");
        }

        public override void Save(string path, bool append = false)
        {
            //List<string> headerStrings = new List<string>();
            //List<string[]> contentStrings = new List<string[]>();

            //foreach (CSVHeaderObject ho in headerObjects)
            //{
            //    headerStrings.Add(ho.header);
            //    contentStrings.Add(ho.content.ToArray());
            //}

            //string headerString = string.Join(",", headerStrings.ToArray());

            //StringBuilder sb = new StringBuilder(headerString);
            //sb.Append("\n");

            //int yMax = 0;
            //for (int i = 0; i < contentStrings.Count; ++i)
            //{
            //    if (yMax < contentStrings[i].Length)
            //        yMax = contentStrings[i].Length;
            //}
            //int xMax = contentStrings.Count;

            //List<string> strings = new List<string>();
            //for (int y = 0; y < yMax; ++y)
            //{
            //    StringBuilder sbTemp = new StringBuilder("");
            //    int xCount = 0;
            //    for (int i = 0; i < xMax; ++i)
            //    {
            //        if (contentStrings[i].Length > y)
            //            ++xCount;
            //    }

            //    Debug.Log(xCount);
            //    for (int x = 0; x < xCount; ++x)
            //    {
            //        if (x < xCount - 1)
            //        {
            //            if (y < contentStrings[x].Length)
            //                sbTemp.Append(contentStrings[x][y] + ",");
            //        }
            //        else
            //        {
            //            if (y < contentStrings[x].Length)
            //                sbTemp.Append(contentStrings[x][y]);
            //        }
            //    }
            //    strings.Add(sbTemp.ToString());
            //}

            //for (int i = 0; i < strings.Count; ++i)
            //{
            //    sb.Append(strings[i]);
            //    if (i < strings.Count - 1)
            //        sb.Append("\n");
            //}

            List<string> content = new List<string>();
            for (int i = 0; i < fileObjects.Count; ++i)
            {
                CSVColumnObject colObj = (CSVColumnObject)fileObjects[i];
                content.Add(colObj.GetAllCellContent());
            }

            List<string> headerStrings = new List<string>();
            foreach (CSVHeaderObject headerObject in headers)
            {
                headerStrings.Add(headerObject.header);
            }

            string finalHeaderStrings = string.Join(",", headerStrings);
            string final = finalHeaderStrings + "\n" + string.Join("\n", content);

            //Debug.Log(final);
            StreamWriter writer = new StreamWriter(path, append);
            writer.WriteLine(final);
            writer.Close();
        }

        protected override FileObject GetFileObject(string headerName)
        {
            for (int i = 0; i < fileObjects.Count; ++i)
            {
                if (fileObjects[i].header.CompareTo(headerName) == 0)
                    return fileObjects[i];
            }

            //int header;
            //int.TryParse(headerName, out header);
            //CSVColumnObject headerObject = new CSVColumnObject(header);
            //headerObject.header = headerName;
            //fileObjects.Add(headerObject);

            return GetNewFileObject(headerName);
        }

        private FileObject GetNewFileObject(string headerName)
        {
            int header;
            int.TryParse(headerName, out header);
            CSVColumnObject columnObject = new CSVColumnObject(header);
            columnObject.header = headerName;
            fileObjects.Add(columnObject);

            for (int h = 0; h < headers.Count; ++h)
            {
                columnObject.InitializeCell(headers[h].header);
            }

            return columnObject;
        }
    }
}