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
    float xmax, xmin, ymax, ymin, zmax, zmin = 0;

    private bool colorGroupingEnabled;

    List<GameObject> colored_point_prefabs;

    delegate void DropdownEvent(int x1);
    
    // Start is called before the first frame update
    void Start()
    {               
        // Import data
        data = CSVReader.IntoJaggedArray("data/iris");
        fdata = CSVReader.IntoFloatArray("data/iris"); // CHANGE TO PATH

        // Get column names
        variables = data[0];

        // Initialize colored point prefabs list
        colored_point_prefabs = new List<GameObject>() {
            pointColor1, pointColor2, pointColor3, pointColor4
        };

        // Spawn data points by variables (0=firstVariable)
        x1 = 0;
        x2 = 1;
        x3 = 2;
        color = 4;
        
        populatePoints(); 
               
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
        if (colorGroupingEnabled) {
            color = change.value;
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

        // iterate over each row in the data
        for (int line = 1; line < fdata.GetLength(0); line++)
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

    // populate points and scale axes
    private void populatePointsOLD(int x1, int x2, int x3) { // TODO add int color
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
            
            GameObject point = Instantiate(pointDefault, plotbase.transform);

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
