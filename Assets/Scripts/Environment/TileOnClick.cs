using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileOnClick : MonoBehaviour
{
    //****************************************************************************************|
    //                                                                                        |
    // Checks when the tile is clicked, so the player may move to that tile.                  |
    //                                                                                        |
    //****************************************************************************************|

    private GameManager manager;
    private TileInfo info;

    // When starts, the script finds the references for the scripts that will be accessed by this one.
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        info = GetComponent<TileInfo>();
    }

    //Calls the method to generate the path to that tile for the player.
    private void OnMouseUp() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            Debug.Log("clicked on " + info.position + (MapManager.map[info.position.x, info.position.y].isWalkable ? "Walkable, " : "Non Walkable, ") + (MapManager.map[info.position.x, info.position.y].hasEnemy ? "Has Enemy." : "Don't Have Enemy."));
            TileInteractions.GeneratePath(gameObject);
        }
    }
}
