using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupManager : MonoBehaviour
{
    public static StartupManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("localization")) {
            PlayerPrefs.SetInt("localization", 0);
        }

        GetComponent<LocalizationData>().LocalizeStart(PlayerPrefs.GetInt("localization"));
    }
}
