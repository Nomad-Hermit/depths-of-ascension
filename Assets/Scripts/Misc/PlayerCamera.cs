using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //****************************************************************************************|
    //                                                                                        |
    // Makes the camera follow the player.                                                    |
    //                                                                                        |
    //****************************************************************************************|

    public GameObject player;

    private Vector3 offset;
    
    // Get the reference to the player GameObject.
    void LookForPlayer()
    {
        player = GameObject.Find("Player(Clone)");
        offset = transform.position - player.transform.position;
    }

    // If there is a player reference, makes the camera follow it. If not, look for a player.
    void LateUpdate()
    {
        if (player != null) {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        } else {
            LookForPlayer();
        }
    }

    
}
