using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startup;
    public static Slider minSlider;

    public Text paddingValueText;
    private int currentCase;
    private string[] cases;
    private float[] caseValues;

    // Start is called before the first frame update
    void Start()
    {
        //minSlider = this.transform.Find("FilterPanel").Find("MinSlider").GetComponent<Slider>();

        // Padding toggle
        currentCase = 2; // starting case is "1"
        cases = new string[] {
            "0.01", "0.1", "1", "10", "100", "1k", "10k", "100k", "1M"
        };

        caseValues = new float[] {
            0.01f, 0.1f, 1, 10, 100, 1000, 10000, 100000, 1000000
        };

    }

    /* Set sliders to min and max values of axes
    public static void updateSliders() {
        minSlider.value = Startup.xmin;
        minSlider.minValue = Startup.xmin;
        minSlider.maxValue = Startup.xmax;
    }

    public static float getMinSliderValue() {
        return minSlider.value;
    } */

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
        startup.GetComponent<Startup>().populatePoints();
    }
}
