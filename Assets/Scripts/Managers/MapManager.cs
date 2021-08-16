using System.Collections;
using System; // So the script can use the serialization commands
using System.Collections.Generic;
using UnityEngine;

public class MapManager {
    public static Tile[,] map; // the 2-dimensional map with the information for all the tiles
    public static List<Enemy> enemies;
}

[Serializable] // Makes the class serializable so it can be saved out to a file
public class Tile { // Holds all the information for each tile on the map
    public int xPosition; // the position on the x axis
    public int yPosition; // the position on the y axis
    [NonSerialized]
    public GameObject baseObject; // the map game object attached to that position: a floor, a wall, etc.
    public string type; // The type of the tile, if it is wall, floor, etc
    public bool hasPlayer = false;
    public bool isWalkable = false;
    public bool isOpaque = false;
    public bool isVisible = false;
    public bool isExplored = false;
    public string color = "black";
    public bool isDoor = false;
    public bool doorIsOpen = false;
    [NonSerialized]
    public GameObject extraObject;
    public bool isExit = false;
    public bool hasCosmetic = false;
    [NonSerialized]
    public GameObject cosmeticObject;
    public bool hasEnemy = false;
    public int enemyID;
    [NonSerialized]
    public GameObject enemyObject;
    public bool hasItem = false;
    public int itemID;
    [NonSerialized]
    public GameObject itemObject;
}

[Serializable]
public class Wall { // A class for saving the wall information, for the dungeon generation algorithm
    public List<Vector2Int> positions;
    public string direction;
    public int length;
    public bool hasFeature = false;
    public Feature parent;
}

[Serializable]
public class Feature { // A class for saving the feature (corridor or room) information, for the dungeon generation algorithm
    public List<Vector2Int> positions;
    public Wall[] walls;
    public string type;
    public int width;
    public int height;
    public int id;
    public bool hasPlayer = false;
}

[Serializable]
public class Enemy {
    public Vector2Int position;
    public string enemyClass;
    public int hp;
    public int baseAttack;
    public int diceDamage;
    public int rollDamage;
    public int bonusDamage;
    public int ac;
    public int xp;
    public string aiType;
    public bool isAlive = true;
    [NonSerialized]
    public GameObject enemyObject;
}
