using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxesDropdown : MonoBehaviour
{
    public GameObject importManager;
    public int axis;

    private string[] vars;
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Dropdown GameObject
        Dropdown dropdown = GetComponent<Dropdown>();

        // Get variable list
        vars = importManager.GetComponent<Startup>().variables;

        // Create empty list for dropdown entries
        List<Dropdown.OptionData> entries = new List<Dropdown.OptionData>();

        // Add each variable in the data as a dropdown option
        foreach(string var in vars) {
            entries.Add(new Dropdown.OptionData(var));
        }

        // Assign options
        dropdown.options = entries;

        // Set initial display
        dropdown.value=axis-1;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
