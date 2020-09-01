﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxesDropdown : MonoBehaviour
{
    public GameObject importManager;

    private string[] vars;
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Dropdown GameObject
        Dropdown dropdown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });

        // Get variable list
        vars = importManager.GetComponent<Startup>().variables;

        List<Dropdown.OptionData> entries = new List<Dropdown.OptionData>();

        foreach(string var in vars) {
            entries.Add(new Dropdown.OptionData(var));
        }

        dropdown.options = entries;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
        Debug.Log("Value changed");
    }
}
