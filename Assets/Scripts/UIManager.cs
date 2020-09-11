using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startup;
    public Text paddingValueText;
    private int currentCase;
    private string[] cases;
    private int[] caseValues;

    // Start is called before the first frame update
    void Start()
    {
        // Padding toggle
        currentCase = 0;
        cases = new string[] {
            "1", "10", "100", "1k", "10k", "100k", "1M"
        };

        caseValues = new int[] {
            1, 10, 100, 1000, 10000, 100000, 1000000
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void paddingToggleLeft() {
        currentCase -= 1;
        updateText();
    }

    public void paddingToggleRight() {
        currentCase += 1;
        updateText();
    }

    private void updateText() {
        currentCase = Mathf.Clamp(currentCase, 0, cases.Length-1);
        paddingValueText.text = cases[currentCase];
        startup.GetComponent<Startup>().colorPadding = caseValues[currentCase];
    }
}
