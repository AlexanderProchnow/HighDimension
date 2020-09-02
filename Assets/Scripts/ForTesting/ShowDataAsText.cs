using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;


public class ShowDataAsText : MonoBehaviour
{
    float[,] fdata;
    string[][] data;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        fdata = Startup.fdata;
        data = Startup.data;
        text = GetComponent<Text>();

        //text.text += "Dims: (" + fdata.GetLength(0).ToString() + ", " + fdata.GetLength(1).ToString() + ")");

        for(int i=1; i<150; i++) {
            for(int j=0; j<fdata.GetLength(1); j++) {
                text.text += fdata[i,j].ToString();
                text.text += " ";
            }
            text.text += "\n";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
