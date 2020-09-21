using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine.SceneManagement;


public class Startup : MonoBehaviour
{
    [Header("Point Prefabs:")]

    // basic point prefab
    public GameObject pointDefault;
    public GameObject pointColor1;
    public GameObject pointColor2;
    public GameObject pointColor3;
    public GameObject pointColor4;

    [Tooltip("Range in which values are assigned the same color.")]
    public float colorPadding = 1;


    [Header("Axes and variables:")]
    [Space(10)]
    public GameObject plotbase;

    
    // axis variables (0=first variable)
    public int x1, x2, x3, color;

    // get plot axes objects
    public GameObject axis1_object;
    public GameObject axis2_object;
    public GameObject axis3_object;

    // make imported data available
    public static string[][] data;
    public static float[,] fdata;
    public string[] variables;    

    // Record maximum and minimum value on each axis
    public static float xmax, xmin, ymax, ymin, zmax, zmin = 0;

    private bool colorGroupingEnabled;

    List<GameObject> colored_point_prefabs;

    delegate void DropdownEvent(int x1);

    private float initial_scale;
    
    // Start is called before the first frame update
    void Start()
    {               
        // Import data
        var importedData = CSVReader.IntoFloatArray(Application.persistentDataPath + "/data/" + FileBrowser.fileName + ".csv");

        // Get data values
        fdata = importedData.Item1;

        // Get column names
        variables = importedData.Item2;


        // Initialize colored point prefabs list
        colored_point_prefabs = new List<GameObject>() {
            pointColor1, pointColor2, pointColor3, pointColor4
        };

        // Spawn data points by variables (0=firstVariable)
        x1 = 0;
        x2 = 1;
        x3 = 2;
        color = 4;
        
        
        //plotbase.transform.parent.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        populatePoints();      

        // Set initial scaling of plotbase
        //initial_scale = (fdata[1, x1] + fdata[1, x2] + fdata[1, x3]) / 60;
         
    }

    // called to update the axes (called when menu item changes)
    public void x1Update(Dropdown change) {
        x1 = change.value;
        populatePoints();
    }

    public void x2Update(Dropdown change) {
        x2 = change.value;  
        populatePoints();
    }

    public void x3Update(Dropdown change) {
        x3 = change.value;
        populatePoints();
    }

    public void colorUpdate(Dropdown change) {
        color = change.value;
        if (colorGroupingEnabled) {
            populatePoints();
        }
    }

    public void enableColorGrouping(Toggle toggle) {
        colorGroupingEnabled = toggle.isOn;
        populatePoints();
    }

    public void clearData() {
        foreach (Transform child in plotbase.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

     // populate points and scale axes
    public void populatePoints(bool clearCurrentData=true) {
        if (clearCurrentData) { clearData(); }
        resetMinMaxTrackers();

        // create list to keep track of color values
        List<float> color_values = new List<float>();

        // Set values for initial point size
        initial_scale = (fdata[1, x1] + fdata[1, x2] + fdata[1, x3]) / 150;

        // iterate over each row in the data
        for (int line = 1; line < fdata.GetLength(0)-1; line++)
        {
            // Prefab to instantiate for current data point
            GameObject pointPrefab = pointDefault;
            // x1
            float value_x1 = fdata[line, x1];
            // x2
            float value_x2 = fdata[line, x2];
            // x3
            float value_x3 = fdata[line, x3];

            // color
            if (colorGroupingEnabled) {
                float value_color = fdata[line, color];
                // Rounds value up to color padding digits (e.g. padding=1 rounds to nearest integer)
                value_color = Mathf.Round(value_color * colorPadding) / colorPadding;
                // Get index of value
                int index_of_color_value = color_values.IndexOf(value_color);
                // Check if index is -1, i.e. value is not present in list
                if (index_of_color_value==-1) {
                    // add value to list
                    color_values.Add(value_color);
                    // set index variable to newly added value's index
                    index_of_color_value = color_values.IndexOf(value_color);
                }
                
                // Check if prefab list must be expanded
                if (index_of_color_value >= colored_point_prefabs.Count) {
                    // expand list to accomodate new index
                    colored_point_prefabs.Add(pointDefault); // TODO Make list extend to new colors
                }
                
                // override pointPrefab to instantiate so that it includes the appropriate color
                pointPrefab = colored_point_prefabs[index_of_color_value];
            }

            // Instantiate point with the plotbase as parent and instantiating in world space is false
            GameObject point = Instantiate(pointPrefab, plotbase.transform, false);

            //point.transform.parent = plotbase.transform;
            //point.transform.localPosition = new Vector3(0,0,0);
            point.transform.localPosition = new Vector3(value_x1, value_x2, value_x3);
            point.transform.localScale = new Vector3(initial_scale, initial_scale, initial_scale);

            // Find x1 min and max
            if (value_x1<xmin) { xmin = value_x1; }
            if (value_x1>xmax) { xmax = value_x1; }

            // Find x2 min and max
            if (value_x2<ymin) { ymin = value_x2; }
            if (value_x2>ymax) { ymax = value_x2; }

            // Find x3 min and max
            if (value_x3<zmin) { zmin = value_x3; }
            if (value_x3>zmax) { zmax = value_x3; }

            
        }

        Debug.Log("X: " + xmin.ToString() + " - " + xmax.ToString());

        // Scale plot axes to reach up until the maximum and minium values for each axis
        initial_scale *= 100;
        axis1_object.transform.localScale = new Vector3(xmax, initial_scale, initial_scale);
        axis2_object.transform.localScale = new Vector3(initial_scale, ymax, initial_scale);
        axis3_object.transform.localScale = new Vector3(initial_scale, initial_scale, zmax);

        // UIManager.updateSliders();
    }

    /* Change axis length and move points acordingly UNUSED
    public void moveAxes() {
        axis1_object.transform.localScale = new Vector3(xmax - UIManager.getMinSliderValue(), 1, 1); 
        plotbase.transform.position = new Vector3(-UIManager.getMinSliderValue(), 0, 0);
    } */


    // Helper function
    private void resetMinMaxTrackers() {
        xmax = 0;
        xmin = 0;
        ymax = 0;
        ymin = 0;
        zmax = 0;
        zmin = 0;
    }

    public void ResetScene() {
        SceneManager.LoadScene("VRApp");
    }

    public void BackToMainMenu() {
        //Camera.main.clearFlags = CameraClearFlags.SolidColor;
        SceneManager.LoadScene("DesktopApp");
    }
}
