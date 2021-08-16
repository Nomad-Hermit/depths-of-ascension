using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public Sprite openDoor;
    public Sprite closedDoor;

    public Vector2Int position;

    TileVisibility visibility;

    public void Initialize() {
        visibility = GetComponent<TileVisibility>();
        position = visibility.position;
    }

    public void OpenDoor() {
        GetComponent<SpriteRenderer>().sprite = openDoor;
        MapManager.map[position.x, position.y].isOpaque = false;
        MapManager.map[position.x, position.y].isWalkable = true;
        MapManager.map[position.x, position.y].doorIsOpen = true;
    }

    public void CloseDoor() {
        GetComponent<SpriteRenderer>().sprite = closedDoor;
        MapManager.map[position.x, position.y].isOpaque = true;
        MapManager.map[position.x, position.y].isWalkable = false;
        MapManager.map[position.x, position.y].doorIsOpen = false;
    }
}
