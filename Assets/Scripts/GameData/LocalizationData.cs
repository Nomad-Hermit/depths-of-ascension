using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class LocalizationData : MonoBehaviour
{
    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingString = "string not found";

    public void LocalizeStart(int value) {
        if (value == 0) {
            LoadLocalizedText("localizedText_eng.json");
        }
        else if (value == 1) {
            LoadLocalizedText("localizedText_por.json");
        }
        else if (value == 2) {
            LoadLocalizedText("localizedText_esp.json");
        }
        else if (value == 3) {
            LoadLocalizedText("localizedText_fra.json");
        }
    }

    //Pull the array of strings from the json.
    public void LoadLocalizedText(string fileName) {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        string newPath = Path.Combine(Application.persistentDataPath, fileName);

        DoWWW_Android(fileName, filePath, newPath);

        if (File.Exists(newPath)) {
            string dataAsJson = File.ReadAllText(newPath);
            LocalizedData loadedData = JsonUtility.FromJson<LocalizedData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++) {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains " + localizedText.Count.ToString() + " entries");
        }
        else {
            //Returns error message if there is not a json file.
            Debug.LogError("Cannot Find Localization File!");
        }

        isReady = true;

        FeedManager.localization = this;
    }

    static void DoWWW_Android(string name, string origPath, string newPath) {
        using (UnityWebRequest reader = UnityWebRequest.Get(origPath)) {
            reader.SendWebRequest();
            while (!reader.isDone) {
            }
            if (!reader.isHttpError && !reader.isNetworkError) {
                File.WriteAllBytes(newPath, reader.downloadHandler.data);
            }
            else {
                Debug.LogError("Database " + name + " not found at " + origPath);
                Debug.LogError("Error: " + reader.error);
            }
        }
    }

    //Returns the correct string from the localization array.
    public string GetString(string key) {
        string result = missingString;

        if (localizedText.ContainsKey(key)) {
            result = localizedText[key];
        }

        return result;
    }
}

[System.Serializable]
public class LocalizedData {
    public LocalizationItem[] items;
}

[System.Serializable]
public class LocalizationItem {
    public string key;
    public string value;
}
