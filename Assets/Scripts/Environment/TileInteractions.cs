using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractions
{
    //****************************************************************************************|
    //                                                                                        |
    // Calculates the path from the player to the target tile and then moves the player       |
    // through that path.                                                                     |
    //                                                                                        |
    //****************************************************************************************|

    private static GameManager manager;
    private static DungeonGenerator dungeon;
    private static List<Vector2Int> path;
    //private static PlayerMovement movement;
    private static Vector2Int tile;
    private static int lengthPath;
    private static int walkedPath;
    private static Tile target;


    // Clears the path and pulls all references.
    public static void Initialize()
    {
        //Pulls references
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dungeon = GameObject.Find("DungeonManager").GetComponent<DungeonGenerator>();
        //movement = GameObject.Find("Player(Clone)").GetComponent<PlayerMovement>();

        //Clears the path
        path = null;
        lengthPath = 0;
        walkedPath = 0;
    }


    //Generates a new path from the player position to the target tile.
    public static void GeneratePath(GameObject clickedTile) {
        //Update what is the target tile.
        tile = clickedTile.GetComponent<TileInfo>().position;

        //If the player clicked the tile on its turn, calculates the path and toggles that the player has a path for the next turns.
        /*if (manager.isPlayerTurn) {
            path = CalculatePath();
            manager.hasPath = true;
        }*/
    }

    //Calculates the new path.
    /*static List<Vector2Int> CalculatePath() {
        List<Vector2Int> nextStep = AStar.CalculatePath(true, movement.position, tile, dungeon.mapWidth,dungeon.mapHeight);
        
        //Gets the length of the path.
        foreach(Vector2Int tile in nextStep) {
            lengthPath++;
        }

        //Returns the path.
        return nextStep;
    }*/

    //Makes the player walk the calculated path if there is no enemy visible.
    public static void TryToWalk(int move) {
        //Counts the length of the path so the correct step is called when movint the player around.
        int count = 0;

        //Checks if there is an enemy visible if the path is longer than one tile.
        //If the path is only one tile long there is no need of checking for enemies around, so the player can move with the mouse on the presence of the enemy.
        /*if (lengthPath > 1) {
            foreach (Vector2Int position in FoV.tilesInFieldOfView) {
                if (MapManager.map[position.x, position.y].hasEnemy) {
                    //If there is an enemy visible, cancels the path.
                    Debug.Log("Saw an enemy, cancelled path!");
                    CancelPath();
                    return;
                }
            }
        }*/

        //Debug.Log(path.Count);

        //Check in which step of the path the player is now.
        foreach (Vector2Int position in path) {
            
            if (count >= walkedPath) {

                walkedPath++;

                //Moves the player.
                //movement.Move(position);

                if (walkedPath == lengthPath) {
                    //if the player reached the end of the path, clear the path.
                    CancelPath();
                }
                else {
                    foreach (Vector2Int pos in FoV.tilesInFieldOfView) {
                        if (MapManager.map[pos.x, pos.y].hasEnemy) {
                            //If there is an enemy visible, cancels the path.
                            Debug.Log("Saw an enemy, cancelled path!");
                            CancelPath();
                            return;
                        }
                    }
                }
                return;
            }

            //Increase the count of the path, for each step walked.
            count++;


        }
    }

    //Clear the path when it is canceled.
    public static void CancelPath() {
        //manager.hasPath = false;
        walkedPath = 0;
        lengthPath = 0;
        path = null;
    }
}
