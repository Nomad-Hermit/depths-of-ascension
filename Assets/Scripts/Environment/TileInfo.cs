using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public Vector2Int position;
    TileVisibility visibility;

    public bool isVisible;
    public bool isExplored;


    public void GetData(Vector2Int pos) {
        position = pos;

        visibility = GetComponent<TileVisibility>();
        visibility.Initialize();
    }

    public void UpdateData() {
        isVisible = MapManager.map[position.x, position.y].isVisible;
        isExplored = MapManager.map[position.x, position.y].isExplored;
    }
}
