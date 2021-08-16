using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int level;
    public int BossLevel;

    DungeonGenerator dungeon;

    public void newLevel(GameManager gameManager) {
        if (dungeon == null) dungeon = GetComponent<DungeonGenerator>();

        Debug.Log("Level " + level + ", Boss Level: " + BossLevel);

        dungeon.GoGenerate(level == BossLevel, level == 0, gameManager);

        //GameObject.Find("UIManager").GetComponent<TipUI>().OpenPanel();

        level++;
    }

    public void ClearLevel() {
        for (int y = 0; y < dungeon.mapHeight; y++) {
            for (int x = 0; x < dungeon.mapWidth; x++) {
                if (MapManager.map[x,y] != null) {
                    Destroy(MapManager.map[x, y].baseObject);
                    if (MapManager.map[x, y].enemyObject != null) Destroy(MapManager.map[x, y].enemyObject);
                    if (MapManager.map[x, y].extraObject != null) Destroy(MapManager.map[x, y].extraObject);
                    if (MapManager.map[x, y].cosmeticObject != null) Destroy(MapManager.map[x, y].cosmeticObject);

                    MapManager.map[x, y] = null;
                }
            }
        }

        MapManager.enemies.Clear();
    }
}
