using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using SFB;
using UnityEngine.UI;
using System.IO;

public class FileBrowser : MonoBehaviour {
    public static string path = "";
    
    public Text pathText;

    // Called when import button is clicked
    public void SetPath() {
        path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "csv", false)[0];

        pathText.text = path + "\n" + Directory.GetCurrentDirectory();
        File.Copy(path, "Assets/Resources/test.csv", true);
    }

    public void EnterVR() {
        SceneManager.LoadScene("VRApp");
    }
}

// D:\Alex\Unity\HighDimension\Assets\Resources\data\iris.csv