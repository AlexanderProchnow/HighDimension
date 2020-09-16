using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using SFB;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class FileBrowser : MonoBehaviour {
    public static string path = "";
    public static string fileName;
    
    public Text pathText;

    private static List<TextAsset> datasets;

    void Start() {
        datasets = new List<TextAsset>();
        // Read all files in Resources folder
        Object[] datasetObjects = Resources.LoadAll("data", typeof(TextAsset));
        foreach(Object set in datasetObjects) {
            datasets.Append((TextAsset) set);
            Debug.Log(s.text);
        }
        // Make a widget for each file that contains name, (num rows), delete, enter VR
    }

    // Called when import button is clicked
    public void SetPath() {
        path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "csv", false)[0];

        // Extract file name from path
        string[] pathArray = Regex.Split(path, @"\\");
        string fileNameWithEnding = pathArray[pathArray.Length-1];
        fileName = Regex.Split(fileNameWithEnding, @"\.")[0];

        // Display imported path message
        pathText.text = path + "\n" + Directory.GetCurrentDirectory() + "\n" + fileNameWithEnding;

        // copy file to resource folder
        string copyToPath = "Assets/Resources/" + fileNameWithEnding;
        File.Copy(path, copyToPath, true);
    }

    public void EnterVR() {
        SceneManager.LoadScene("VRApp");
    }
}

// D:\Alex\Unity\HighDimension\Assets\Resources\data\iris.csv