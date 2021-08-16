using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //****************************************************************************************|
    //                                                                                        |
    // Loads a scene.                                                                         |
    //                                                                                        |
    //****************************************************************************************|

    // Load new scene.
    public void Load(string scene) {
        SceneManager.LoadScene(scene);
    }
}
