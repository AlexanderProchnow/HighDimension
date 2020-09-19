using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerDesktop : MonoBehaviour
{
    public static string separator;

    public GameObject projectManager;
    public Text separatorValueText;
    private int currentCase;
    private string[] cases;

    // Start is called before the first frame update
    void Start()
    {
        currentCase = 0; // starting case is ","
        cases = new string[] {
            ",", ";"
        };

        separator = cases[currentCase];
    }

    public void separatorToggleLeft() {
        currentCase -= 1;
        updateText();
    }

    public void separatorToggleRight() {
        currentCase += 1;
        updateText();
    }

    private void updateText() {
        currentCase = Mathf.Clamp(currentCase, 0, cases.Length-1);
        separatorValueText.text = cases[currentCase];
        separator = cases[currentCase];
    }
}
