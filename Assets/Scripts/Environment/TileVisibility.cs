using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisibility : MonoBehaviour
{
    //****************************************************************************************|
    //                                                                                        |
    // Get if the tile is visible or not, and then updates its color accordingly.             |
    //                                                                                        |
    //****************************************************************************************|

    public Vector2Int position;

    TileInfo info;
    SpriteRenderer sprite;

    public int turn = 0;

    // When starts, the script finds the references for the scripts that will be accessed by this one.
    public void Initialize()
    {
        sprite = GetComponent<SpriteRenderer>();
        //sprite.color = Color.clear;
        info = GetComponent<TileInfo>();
        position = info.position;
    }

    // Checks if the tile is visible or not, and then updates its color accordingly.
    public void VisibilityCheck()
    {
        info.UpdateData();
        if (MapManager.map[position.x, position.y].isVisible) {
            sprite.color = Color.white;
        }
        else if (MapManager.map[position.x, position.y].isExplored && !MapManager.map[position.x, position.y].isVisible) {
            sprite.color = Color.grey;
        }
        else {
            sprite.color = Color.clear;
        }
        turn++;
    }
}
