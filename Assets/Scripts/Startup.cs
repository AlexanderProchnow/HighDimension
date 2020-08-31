using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Globalization;


public class Startup : MonoBehaviour
{
    // basic point prefab
    public GameObject pointPrefab;
    public GameObject plotbase;

    // set axis variables
    public enum axis1_values {sepal_length, sepal_width, petal_length, petal_width, species};
    public enum axis2_values {sepal_length, sepal_width, petal_length, petal_width, species};
    public enum axis3_values {sepal_length, sepal_width, petal_length, petal_width, species};
    public axis1_values x1_variable;
    public axis2_values x2_variable;
    public axis3_values x3_variable; 

    // get plot axes objects
    public GameObject axis1_object;
    public GameObject axis2_object;
    public GameObject axis3_object;

    // make imported data available (unused)
    public static string[][] data; 
    public static string[] variables;    

    // Record maximum and minimum value on each axis
    float xmax, xmin, ymax, ymin, zmax, zmin = 0;

    delegate void DropdownEvent(int x1);
    
    // Start is called before the first frame update
    void Start()
    {               
        // Import data
        data = CSVReader.IntoJaggedArray("data/iris");

        // Get column names
        variables  = data[0];

        /* Spawn data points
        int x1 = (int)x1_variable;
        int x2 = (int)x2_variable;
        int x3 = (int)x3_variable;
        */
        populatePoints((int)x1_variable, (int)x2_variable, (int)x2_variable); 
               
    }

    public void menuUpdate(int x1, int x2, int x3) {
        populatePoints(x1, x2, x3);
    }

    // populate points and scale axes
    void populatePoints(int x1, int x2, int x3) {
        //xmax, xmin, ymax, ymin, zmax, zmin = 0;

        for (int line = 1; line < data.Length-1; line++)
        {
            // x1
            float value_x1;
            float.TryParse(data[line][x1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out value_x1);
            // x2
            float value_x2;
            float.TryParse(data[line][x2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out value_x2);
            // x3
            float value_x3;
            float.TryParse(data[line][x3], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out value_x3);            
            
            GameObject point = Instantiate(pointPrefab, plotbase.transform);

            //point.transform.parent = plotbase.transform;
            point.transform.position += new Vector3(value_x1, value_x2, value_x3);

            // Find x1 min and max
            if (point.transform.position.x<xmin) { xmin = point.transform.position.x; }
            if (point.transform.position.x>xmax) { xmax = point.transform.position.x; }

            // Find x2 min and max
            if (point.transform.position.y<ymin) { ymin = point.transform.position.y; }
            if (point.transform.position.y>ymax) { ymax = point.transform.position.y; }

            // Find x3 min and max
            if (point.transform.position.z<zmin) { zmin = point.transform.position.z; }
            if (point.transform.position.z>zmax) { zmax = point.transform.position.z; }
        }

        // Scale plot axes to reach up until the maximum and minium values for each axis
        axis1_object.transform.localScale = new Vector3(xmax+xmax/3, 1, 1);
        axis2_object.transform.localScale = new Vector3(1, ymax+ymax/3, 1);
        axis3_object.transform.localScale = new Vector3(1, 1, zmax+zmax/3);
    }

    void Awake() {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


}
