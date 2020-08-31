using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxesDropdown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Dropdown GameObject
        Dropdown dropdown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });

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
