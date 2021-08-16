using System.Collections.Generic;
using UnityEngine;

public class AStar 
{
    static List<PathNode> openList;
    static List<PathNode> closedList;
    static PathNode[,] allNodes;
    
    public static List<Vector2Int> CalculatePath(bool isMovement, Vector2Int start, Vector2Int target, int mapWidth, int mapHeight, float[,] rockMap = null) {
        List<Vector2Int> path = new List<Vector2Int>();

        openList = new List<PathNode>();
        closedList = new List<PathNode>();
        allNodes = new PathNode[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                allNodes[x, y] = new PathNode();
            }
        }

        PathNode firstNode = new PathNode();
        firstNode.position = start;
        firstNode.gCost = 0;

        //closedList.Add(firstNode);

        path.Add(target);

        foreach (Vector2Int position in GetNeighbours(isMovement, firstNode.position, mapWidth, mapHeight, rockMap)) {
            PathNode node = new PathNode() { position = position, parent = firstNode, gCost = CalculateGCost(firstNode.gCost, isMovement ? (CheckIfDiagonal(firstNode.position, position) ? 14 : 10) : (int)rockMap[position.x, position.y]), hCost = CalculateHCost(position, target, rockMap) };
            node.fCost = CalculateFCost(node);
            allNodes[node.position.x, node.position.y].isInOpenList = true;
            openList.Add(node);
        }

        int loopCount = 0;
        PathNode lastNode = new PathNode();

        while (openList.Count > 0) {
            loopCount++;

            PathNode node = openList[0];

            foreach (PathNode pathNode in openList) {
                if (node.fCost > pathNode.fCost || (node.fCost == pathNode.fCost && node.hCost > pathNode.hCost)) {
                    node = pathNode;
                }
            }

            closedList.Add(node);
            allNodes[node.position.x, node.position.y].isInClosedList = true;
            openList.Remove(node);
            allNodes[node.position.x, node.position.y].isInOpenList = false;

            bool foundTarget = false;

            //Debug.Log("Closed List: " + closedList.Count);
            //Debug.Log("Open List: " + openList.Count);
            foreach (Vector2Int position in GetNeighbours(isMovement, node.position, mapWidth, mapHeight, rockMap)) {
                if (position == target) {
                    foundTarget = true;
                    //Debug.Log("Found Target");
                    //Debug.Log("Closed List: " + closedList.Count);
                    break;
                }

                if (!allNodes[position.x, position.y].isInOpenList && !allNodes[position.x, position.y].isInClosedList) {
                    PathNode neighbour = new PathNode() { position = position, parent = node, gCost = CalculateGCost(node.gCost, isMovement ? (CheckIfDiagonal(node.position, position) ? 14 : 10) : (int)rockMap[position.x, position.y]), hCost = CalculateHCost(position, target, rockMap) };
                    neighbour.fCost = CalculateFCost(neighbour);
                    openList.Add(neighbour);
                    allNodes[position.x, position.y].isInOpenList = true;
                }
            }


            if (foundTarget) {
                openList.Clear();
                lastNode = node;
                break;
            }

        }

        RetracePath(path, start, lastNode);

        return path;
    }

    static void RetracePath(List<Vector2Int> path, Vector2Int start, PathNode node) {
        PathNode currentNode = node;

        while (currentNode.position != start) {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
            //Debug.Log(currentNode.position + ", " + start);
        }

        /*foreach (Vector2Int position in path) {
            Debug.Log(position);
        }*/

        if (path[path.Count - 1] == path[path.Count - 2]) {
            path.Remove(path[path.Count - 1]);
        }
        //Debug.Log(path.Count);
        path.Reverse();
    }

    static List<Vector2Int> GetNeighbours(bool isMovement, Vector2Int parent, int mapWidth, int mapHeight, float [,] rockMap) {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (isMovement) {
            for (int y = parent.y - 1; y <= parent.y + 1; y++) {
                for (int x = parent.x - 1; x <= parent.x + 1; x++) {
                    if (x >= 0 && y >= 0 && x < mapWidth && y < mapHeight) {
                        if (MapManager.map[x, y] != null) {
                            if (MapManager.map[x, y].isWalkable || MapManager.map[x, y].isDoor) {
                                neighbours.Add(new Vector2Int() { x = x, y = y });
                            }
                        }
                    }
                }
            }
        }
        else {
            if (parent.x - 2 >= 0) neighbours.Add(new Vector2Int() { x = parent.x - 1, y = parent.y });
            if (parent.x + 2 < mapWidth) neighbours.Add(new Vector2Int() { x = parent.x + 1, y = parent.y });
            if (parent.y - 2 >= 0) neighbours.Add(new Vector2Int() { x = parent.x, y = parent.y - 1 });
            if (parent.y + 2 < mapHeight) neighbours.Add(new Vector2Int() { x = parent.x, y = parent.y + 1 });
        }

        return neighbours;
    }

    static int CalculateFCost(PathNode node) {
        int fCost = node.hCost + node.gCost;

        return fCost;
    }

    static int CalculateHCost(Vector2Int start, Vector2Int target, float[,] rockMap) {
        int hCost = 0;

        if (start.x < target.x) {
            for (int x = start.x; x <= target.x; x++) {
                hCost += rockMap == null ? 10 : (int)rockMap[x, start.y];
            }
        }
        else if (start.x > target.x) {
            for (int x = start.x; x >= target.x; x--) {
                hCost += rockMap == null ? 10 : (int)rockMap[x, start.y];
            }
        }
        else {
            hCost += 0;
        }

        if (start.y < target.y) {
            for (int y = start.y; y <= target.y; y++) {
                hCost += rockMap == null ? 10 : (int)rockMap[target.x, y];
            }
        }
        else if (start.y > target.y) {
            for (int y = start.y; y >= target.y; y--) {
                hCost += rockMap == null ? 10 : (int)rockMap[target.x, y];
            }
        }
        else {
            hCost += 0;
        }

        return hCost;
    }

    static int CalculateGCost(int parent, int localG) {
        int gCost = parent + localG;

        return gCost;
    }

    static bool CheckIfDiagonal(Vector2Int origin, Vector2Int neighbour) {
        bool isDiagonal = false;

        if (origin.x != neighbour.x && origin.y != neighbour.y) isDiagonal = true;

        return isDiagonal;
    }
}

public class PathNode {
    public Vector2Int position;
    public int gCost;
    public int hCost;
    public int fCost;
    public PathNode parent;
    public bool isInOpenList = false;
    public bool isInClosedList = false;
}
