using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

    //****************************************************************************************|
    //                                                                                        |
    // Updates the string of the text from the Localization.                                  |
    //                                                                                        |
    //****************************************************************************************|

    public string key;

    LocalizationData localization;

    //Updates the string of the text from the Localization when the GameObject is created.
    void Start () {
        localization = GameObject.Find("StartupManager").GetComponent<LocalizationData>();
		this.gameObject.GetComponent<Text> ().text = localization.GetString (key);
	}

    //Updates the string of the text from the Localization in realtime.
    public void UpdateLocalization() {
        this.gameObject.GetComponent<Text>().text = localization.GetString(key);
    }
	

}
