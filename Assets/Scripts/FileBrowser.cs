using System.Collections;
using System.Collections.Generic;
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
    public GameObject datasetPanel;
    public GameObject datasetWidgetPrefab;

    private static List<string> datasets;

    void Start() {
        // Create the local directory for saving dataset files
        if (!Directory.Exists(Application.persistentDataPath + "/data/"))
        {
            DirectoryInfo di = Directory.CreateDirectory(Application.persistentDataPath + "/data/");
        }
        
        updateDatasetPanel();
    }

    public void updateDatasetPanel() {
        // Clear all dataset widgets
        foreach (Transform child in datasetPanel.transform) {
            GameObject.Destroy(child.gameObject);
        }
        datasets = new List<string>();
        // Read all files in data folder
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/data/", "*.csv");
        // Object[] datasetObjects = Resources.LoadAll("data", typeof(TextAsset)); // OLD

        // Cast objects to text assets
        foreach(string setPath in files) {
            datasets.Add(setPath);
            Debug.Log(datasets[datasets.Count-1]);
        }

        // Make a widget for each file that contains name, (num rows), delete, enter VR:
        for(int i = 0; i<datasets.Count; i++) {
            CreateDatasetWidget(i);
        }
    }

    private void CreateDatasetWidget(int index) {
        // Create new dataset widget
        GameObject datasetWidget = Instantiate(datasetWidgetPrefab, datasetPanel.transform, false);
        // Move widget to be within panel (0,0 is set to top left corner), space widgets out over x-axis
        datasetWidget.transform.localPosition = new Vector3(100*(index+1), -100, 0);
        // Set widget header to dataset name
        Transform datasetName = datasetWidget.transform.Find("Header");
        datasetName.GetComponent<Text>().text = ExtractFileName(datasets[index]);
    }

    // Called when import button is clicked
    public void SetPath() {
        path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "csv", false)[0];

        // Extract file name from path
        fileName = ExtractFileName(path);

        // Display imported path message
        pathText.text = path + "\n" +  Application.persistentDataPath + "/data/" + fileName + ".csv" + "\n" + fileName;

        // copy file to resource folder
        string copyToPath = Application.persistentDataPath + "/data/" + fileName + ".csv";
        // string copyToPath = "Assets/Resources/data/" + fileNameWithEnding; // OLD
        File.Copy(path, copyToPath, true);

        updateDatasetPanel();
        /* Add dataset to panel
        TextAsset importedFile = Resources.Load<TextAsset>("Assets/Resources/data/" + fileName);
        datasets.Add(importedFile);
        Debug.Log(importedFile);
        CreateDatasetWidget(datasets.Count-1);
        */
    }

    public void EnterVR() {
        SceneManager.LoadScene("VRApp");
    }

    // Helper function
    private string ExtractFileName(string path) {
        string[] pathArray = Regex.Split(path, @"(\\|/)");
        string fileNameWithEnding = pathArray[pathArray.Length-1];
        string fileName = Regex.Split(fileNameWithEnding, @"\.")[0];
        return fileName;
    }
}

// D:\Alex\Unity\HighDimension\Assets\Resources\data\iris.csv