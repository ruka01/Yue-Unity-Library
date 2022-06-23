using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yue.FileIO;

public class FileTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string headers = "Name,Health,Damage,Defense";
        CSV csv = new CSV();
        csv.Read(Application.persistentDataPath + "/test.csv");
        FileObject fileObject = csv.WriteNewObject();
        csv.WriteOn(fileObject, "Name", "one");
        csv.WriteOn(fileObject, "Health", "four");
        csv.WriteOn(fileObject, "Defense", "nine");

        FileObject fileObject2 = csv.WriteNewObject();
        csv.WriteOn(fileObject2, "Name", "nine");
        csv.WriteOn(fileObject2, "Damage", "five");
        csv.WriteOn(fileObject2, "Defense", "two");

        csv.Save(Application.persistentDataPath + "/test.csv", false);
    }

}
