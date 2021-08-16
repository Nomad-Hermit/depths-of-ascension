using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float tileScale;

    public int widthMinRoom;
    public int widthMaxRoom;
    public int heightMinRoom;
    public int heightMaxRoom;

    public int minCorridorLength;
    public int maxCorridorLength;
    public int maxFeatures;
    int countFeatures;
    public int maxTries;

    public int enemyQuantity;
    public int cosmeticsQuantity;

    public List<Feature> allFeatures;
    List<Vector2Int> corridors;

    public GameObject playerPrefab;
    public GameObject doorPrefab;
    public GameObject cosmeticPrefab;
    GameManager manager;

    public Sprite[] floors;
    public Sprite[] walls;
    public Sprite[] doors;
    public Sprite[] statues;
    public Sprite[] fountains;
    public Sprite[] bloodStains;
    public Sprite exit;

    public int[] weaponSpawnQuantity;
    public int[] armorSpawnQuantity;
    public int[] potionSpawnQuantity;
    public int[] ringSpawnQuantity;
    public int[] scrollSpawnQuantity;
    public int[] foodSpawnQuantity;
    public GameObject itemPrefab;
    public GameObject itemParent;

    public int level;

    public void GoGenerate(bool isBoss, bool isNewGame, GameManager gameManager) {
        manager = gameManager;
        InitializeDungeon(isNewGame);
        GenerateDungeon();
        //SpawnPlayer(isNewGame);
        //SpawnEnemies(isBoss ? 1 : enemyQuantity, true);
        SpawnCosmetics();
        SpawnExit();
        //SpawnEquipment();
        DrawMap(false);
    }

    public void InitializeDungeon(bool isFirst) {
        MapManager.map = new Tile[mapWidth, mapHeight];
        MapManager.enemies = new List<Enemy>();
        allFeatures = new List<Feature>();
        allFeatures.Clear();
        countFeatures = 0;
        if (isFirst) level = 0;
        else level++;
    }

    void GenerateDungeon() {
        int tries = 0;

        while(countFeatures < maxFeatures) {
            GenerateFeature("Room", Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1));

            tries++;
            if (tries >= maxTries) break;
        }

        GenerateCorridors();
        FillWalls();
        GenerateDoors();
    }

    void GenerateFeature(string type, int xStartingPoint, int yStartingPoint) {
        Feature room = new Feature();
        room.positions = new List<Vector2Int>();

        int roomWidth = Random.Range(widthMinRoom, widthMaxRoom);
        int roomHeight = Random.Range(heightMinRoom, heightMaxRoom);

        if (!CheckIfHasSpace(new Vector2Int(xStartingPoint, yStartingPoint), new Vector2Int(xStartingPoint + roomWidth - 1, yStartingPoint + roomHeight - 1))) return;

        room.walls = new Wall[4];

        for (int i = 0; i < room.walls.Length; i++) {
            room.walls[i] = new Wall();
            room.walls[i].positions = new List<Vector2Int>();
            room.walls[i].length = 0;
            room.walls[i].parent = room;

            switch (i) {
                case 0:
                    room.walls[i].direction = "South";
                    break;
                case 1:
                    room.walls[i].direction = "North";
                    break;
                case 2:
                    room.walls[i].direction = "West";
                    break;
                case 3:
                    room.walls[i].direction = "East";
                    break;
            }
        }

        for (int y = 0; y < roomHeight; y++) {
            for (int x = 0; x < roomWidth; x++) {
                Vector2Int position = new Vector2Int();
                position.x = xStartingPoint + x;
                position.y = yStartingPoint + y;

                room.positions.Add(position);

                MapManager.map[position.x, position.y] = new Tile();
                //Debug.Log(position);
                MapManager.map[position.x, position.y].xPosition = position.x;
                MapManager.map[position.x, position.y].yPosition = position.y;

                if (y == 0) {
                    room.walls[0].positions.Add(position);
                    room.walls[0].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                    MapManager.map[position.x, position.y].isOpaque = true;
                }
                if (y == (roomHeight - 1)) {
                    room.walls[1].positions.Add(position);
                    room.walls[1].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                    MapManager.map[position.x, position.y].isOpaque = true;
                }
                if (x == 0) {
                    room.walls[2].positions.Add(position);
                    room.walls[2].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                    MapManager.map[position.x, position.y].isOpaque = true;
                }
                if (x == (roomWidth - 1)) {
                    room.walls[3].positions.Add(position);
                    room.walls[3].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                    MapManager.map[position.x, position.y].isOpaque = true;
                }
                if (MapManager.map[position.x, position.y].type != "Wall") {
                    MapManager.map[position.x, position.y].type = "Floor";
                    MapManager.map[position.x, position.y].isWalkable = true;
                }
            }
        }

        room.width = roomWidth;
        room.height = roomHeight;
        room.type = type;
        room.id = countFeatures;
        allFeatures.Add(room);
        countFeatures++;
    }

    void GenerateCorridors() {
        corridors = new List<Vector2Int>();
        int seed = UnityEngine.Random.Range(0, 100000);
        float[,] rockMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, 24, 0, 0.9f, 5f, new Vector2(0f,0f));

        foreach (Feature room in allFeatures) {
            List<Vector2Int> validPositions = new List<Vector2Int>();
            foreach (Vector2Int position in room.positions) {
                //Debug.Log(position);
                if (MapManager.map[position.x, position.y] == null) {
                    Debug.Log("Map Tile is Null");
                }
                //Debug.Log(MapManager.map[position.x, position.y].type);
                if (MapManager.map[position.x, position.y].type == "Floor") validPositions.Add(position);
            }
            Vector2Int origin = validPositions[Random.Range(0, validPositions.Count - 1)];

            int maxRuns = countFeatures * 2;
            int runs = 0;
            Feature targetRoom = allFeatures[Random.Range(0, allFeatures.Count - 1)];
            while(targetRoom.id == room.id) {
                targetRoom = allFeatures[Random.Range(0, allFeatures.Count - 1)];
                runs++;
                if (runs >= maxRuns) break;
            }
            validPositions = new List<Vector2Int>();
            foreach (Vector2Int position in targetRoom.positions) {
                if (MapManager.map[position.x, position.y].type == "Floor") validPositions.Add(position);
            }
            Vector2Int target = validPositions[Random.Range(0, validPositions.Count - 1)];

            foreach (Vector2Int position in AStar.CalculatePath(false, origin, target, mapWidth, mapHeight, rockMap)) {
                MapManager.map[position.x, position.y] = new Tile();
                MapManager.map[position.x, position.y].xPosition = position.x;
                MapManager.map[position.x, position.y].yPosition = position.y;
                MapManager.map[position.x, position.y].type = "Floor";
                MapManager.map[position.x, position.y].isWalkable = true;
                MapManager.map[position.x, position.y].isOpaque = false;
                corridors.Add(position);
            }
        }
    }

    bool CheckIfHasSpace(Vector2Int start, Vector2Int end) {
        bool hasSpace = true;

        for (int y = start.y; y <= end.y; y++) {
            for (int x = start.x; x <= end.x; x++) {
                if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight) return false;
                if (MapManager.map[x, y] != null) {
                    return false;
                }
            }
        }

        return hasSpace;
    }

    void FillWalls() {
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                if (MapManager.map[x, y] != null && MapManager.map[x, y].type == "Floor") {

                    for (int i = y - 1; i <= y + 1; i++) {
                        for (int j = x - 1; j <= x + 1; j++) {
                            if (j >= 0 && i >= 0 && j < mapWidth && i < mapHeight) {
                                if (MapManager.map[j, i] == null) {
                                    MapManager.map[j, i] = new Tile() { type = "Wall", isWalkable = false, isOpaque = true };
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void GenerateDoors() {
        foreach (Feature room in allFeatures) {
            foreach (Wall wall in room.walls) {
                int holes = 0;
                Vector2Int holePosition = new Vector2Int(); ;
                foreach (Vector2Int position in wall.positions) {
                    if (MapManager.map[position.x, position.y].type == "Floor") {
                        holes++;
                        holePosition = position;
                    }
                }
                if (holes == 1) {
                    MapManager.map[holePosition.x, holePosition.y].isDoor = true;
                    MapManager.map[holePosition.x, holePosition.y].isOpaque = true;
                    MapManager.map[holePosition.x, holePosition.y].isWalkable = false;
                }
            }
        }
    }

    /*void SpawnPlayer(bool isFirst) {
        Feature startingRoom = allFeatures[Random.Range(0, allFeatures.Count - 1)];

        List<Vector2Int> validPositions = new List<Vector2Int>();
        foreach (Vector2Int position in startingRoom.positions) {
            if (MapManager.map[position.x, position.y].isWalkable) validPositions.Add(position);
        }
        int positionPlayerID = Random.Range(0, validPositions.Count - 1);
        Vector2Int playerPosition = validPositions[positionPlayerID];
        startingRoom.hasPlayer = true;

        GameObject player;

        if (isFirst) {
            player = GameObject.Instantiate(playerPrefab, new Vector3(playerPosition.x * tileScale, playerPosition.y * tileScale, -1), Quaternion.identity);
            player.GetComponent<PlayerMovement>().Initialize(playerPosition);
        }
        else {
            player = GameObject.Find("Player(Clone)");
            player.GetComponent<PlayerMovement>().position = playerPosition;
            player.transform.position = new Vector3(playerPosition.x * tileScale, playerPosition.y * tileScale, -1);
            //player.GetComponent<PlayerMovement>().PositionOnNewLevel(playerPosition);
        }
        MapManager.map[playerPosition.x, playerPosition.y].hasPlayer = true;

        if (manager.player == null) manager.player = player.GetComponent<PlayerMovement>();
    }*/

    /*public void SpawnEnemies(int quantity, bool isNewLevel, Vector2Int player = new Vector2Int()) {
        List<Vector2Int> tiles = new List<Vector2Int>();
        if (isNewLevel) tiles = GetSpawnableRoomTiles();
        else tiles = GetAllSpawnableTiles(true, player);

        for (int i = 0; i < quantity; i++) {
            Vector2Int target = tiles[Random.Range(0, tiles.Count - 1)];

            while (MapManager.map[target.x, target.y].hasEnemy) {
                target = tiles[Random.Range(0, tiles.Count - 1)];
            }

            manager.gameObject.GetComponent<EnemySpawn>().SpawnEnemy(target, tileScale, level, true);
        }
    }*/

    public List<Vector2Int> GetSpawnableRoomTiles() {
        List<Vector2Int> spawnableTiles = new List<Vector2Int>();

        foreach (Feature room in allFeatures) {
            if (!room.hasPlayer) {
                foreach (Vector2Int position in room.positions) {
                    if (MapManager.map[position.x, position.y].isWalkable && !MapManager.map[position.x, position.y].hasEnemy && !MapManager.map[position.x, position.y].hasCosmetic) {
                        spawnableTiles.Add(position);
                    }
                }
            }
        }

        return spawnableTiles;
    }

    public List<Vector2Int> GetAllSpawnableTiles(bool isRelativeToPlayer, Vector2Int player = new Vector2Int()) {
        List<Vector2Int> tiles = new List<Vector2Int>();

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                if (MapManager.map[x,y] != null) {
                    if (!MapManager.map[x, y].hasEnemy && !MapManager.map[x, y].hasCosmetic && !MapManager.map[x, y].isDoor && MapManager.map[x, y].isWalkable) {
                        if (isRelativeToPlayer) {
                            if (Mathf.Abs(x - player.x) > 6 && Mathf.Abs(y - player.y) > 6) {
                                tiles.Add(new Vector2Int(x, y));
                            }
                        }
                        else {
                            tiles.Add(new Vector2Int(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    void SpawnCosmetics() {
        List<Vector2Int> tiles = GetSpawnableRoomTiles();

        for (int i = 0; i < cosmeticsQuantity; i++) {
            Vector2Int target = tiles[Random.Range(0, (tiles.Count * 100) - 1) / 100];

            while (MapManager.map[target.x, target.y].hasCosmetic) {
                target = tiles[Random.Range(0, (tiles.Count * 100) - 1) / 100];
            }
            GameObject newCosmetic = Instantiate(cosmeticPrefab, new Vector3(target.x * tileScale, target.y * tileScale, -0.01f), Quaternion.identity);
            MapManager.map[target.x, target.y].hasCosmetic = true;
            MapManager.map[target.x, target.y].cosmeticObject = newCosmetic;

            int rand = Random.Range(0, 300);
            if (rand < 200) {
                //blood
                newCosmetic.GetComponent<SpriteRenderer>().sprite = bloodStains[0];
            }
            else if (rand < 280) {
                //statue
                newCosmetic.GetComponent<SpriteRenderer>().sprite = statues[Random.Range(0, (statues.Length * 100) - 1) / 100];
                MapManager.map[target.x, target.y].isWalkable = false;
                MapManager.map[target.x, target.y].isOpaque = true;
            }
            else {
                //fountain
                newCosmetic.GetComponent<SpriteRenderer>().sprite = fountains[Random.Range(0, (fountains.Length * 100) - 1) / 100];
                MapManager.map[target.x, target.y].isWalkable = false;
                MapManager.map[target.x, target.y].isOpaque = true;
            }

            newCosmetic.GetComponent<TileInfo>().GetData(target);
        }

        for (int i = 0; i < 10; i++) {
            Vector2Int target = corridors[Random.Range(0, (corridors.Count * 100) - 1) / 100];

            while (MapManager.map[target.x, target.y].hasCosmetic || MapManager.map[target.x, target.y].isDoor) {
                target = corridors[Random.Range(0, (corridors.Count * 100) - 1) / 100];
            }
            GameObject newCosmetic = Instantiate(cosmeticPrefab, new Vector3(target.x * tileScale, target.y * tileScale, -0.01f), Quaternion.identity);
            MapManager.map[target.x, target.y].hasCosmetic = true;
            MapManager.map[target.x, target.y].cosmeticObject = newCosmetic;
            newCosmetic.GetComponent<SpriteRenderer>().sprite = bloodStains[0];
            newCosmetic.GetComponent<TileInfo>().GetData(target);
        }
    }

    void SpawnExit() {
        List<Vector2Int> tiles = GetSpawnableRoomTiles();

        Vector2Int target = tiles[Random.Range(0, (tiles.Count * 100) - 1) / 100];

        GameObject newExit = Instantiate(cosmeticPrefab, new Vector3(target.x * tileScale, target.y * tileScale, -0.01f), Quaternion.identity);
        MapManager.map[target.x, target.y].isExit = true;
        MapManager.map[target.x, target.y].extraObject = newExit;
        newExit.GetComponent<SpriteRenderer>().sprite = exit;
        newExit.GetComponent<TileInfo>().GetData(target);
    }

    /*void SpawnEquipment() {
        int quantity = 0;
        int roll = Random.Range(0, 100);

        for (int i = weaponSpawnQuantity.Length - 1; i >= 0; i--) {
            if (roll <= weaponSpawnQuantity[i]) {
                quantity = i;
                break;
            }
        }
        if (quantity > 0) GenerateItem(quantity, "Weapon");

        roll = Random.Range(0, 100);

        for (int i = armorSpawnQuantity.Length - 1; i >= 0; i--) {
            if (roll <= armorSpawnQuantity[i]) {
                quantity = i;
                break;
            }
        }
        if (quantity > 0) GenerateItem(quantity, "Armor");

        roll = Random.Range(0, 100);

        for (int i = ringSpawnQuantity.Length - 1; i >= 0; i--) {
            if (roll <= ringSpawnQuantity[i]) {
                quantity = i;
                break;
            }
        }
        if (quantity > 0) GenerateItem(quantity, "Ring");

        roll = Random.Range(0, 100);

        for (int i = potionSpawnQuantity.Length - 1; i >= 0; i--) {
            if (roll <= potionSpawnQuantity[i]) {
                quantity = i;
                break;
            }
        }
        if (quantity > 0) GenerateItem(quantity, "Potion");

        roll = Random.Range(0, 100);

        for (int i = scrollSpawnQuantity.Length - 1; i >= 0; i--) {
            if (roll <= scrollSpawnQuantity[i]) {
                quantity = i;
                break;
            }
        }
        if (quantity > 0) GenerateItem(quantity, "Scroll");

        roll = Random.Range(0, 100);

        for (int i = foodSpawnQuantity.Length - 1; i >= 0; i--) {
            if (roll <= foodSpawnQuantity[i]) {
                quantity = i;
                break;
            }
        }
        if (quantity > 0) GenerateItem(quantity, "Food");
    }*/

    /*void GenerateItem(int quantity, string classOfItem) {
        GameObject inventory = GameObject.Find("InventoryManager");
        for (int i = 0; i < quantity; i++) {
            List<Vector2Int> positions = GetSpawnableRoomTiles();
            Vector2Int position = positions[Random.Range(0, positions.Count - 1)];
            GameObject item = Instantiate(itemPrefab, new Vector3(position.x * tileScale, position.y * tileScale, -0.5f), Quaternion.identity);
            item.transform.parent = itemParent.transform;

            Sprite sprite = null;
            string name = "";
            switch (classOfItem) {
                case "Weapon":
                    ItemInfo infoW = item.GetComponent<ItemInfo>();
                    infoW.id = GetWeapon(inventory.GetComponent<WeaponsData>(), out sprite, out name);
                    infoW.classOfItem = classOfItem;
                    infoW.itemData = inventory.GetComponent<InventoryData>().inventories[infoW.id];
                    break;
                case "Armor":
                    ItemInfo infoA = item.GetComponent<ItemInfo>();
                    infoA.id = GetArmor(inventory.GetComponent<ArmorsData>(), out sprite, out name);
                    infoA.classOfItem = classOfItem;
                    infoA.itemData = inventory.GetComponent<InventoryData>().inventories[infoA.id];
                    break;
                case "Ring":
                    ItemInfo infoR = item.GetComponent<ItemInfo>();
                    infoR.id = GetRing(inventory.GetComponent<RingsData>(), out sprite, out name);
                    infoR.classOfItem = classOfItem;
                    infoR.itemData = inventory.GetComponent<InventoryData>().inventories[infoR.id];
                    break;
                case "Potion":
                    ItemInfo infoP = item.GetComponent<ItemInfo>();
                    infoP.id = GetPotion(inventory.GetComponent<PotionsData>(), out sprite, out name);
                    infoP.classOfItem = classOfItem;
                    infoP.itemData = inventory.GetComponent<InventoryData>().inventories[infoP.id];
                    break;
                case "Scroll":
                    ItemInfo infoS = item.GetComponent<ItemInfo>();
                    infoS.id = GetScroll(inventory.GetComponent<ScrollsData>(), out sprite, out name);
                    infoS.classOfItem = classOfItem;
                    infoS.itemData = inventory.GetComponent<InventoryData>().inventories[infoS.id];
                    break;
                case "Food":
                    ItemInfo infoF = item.GetComponent<ItemInfo>();
                    infoF.id = GetFood(inventory.GetComponent<FoodData>(), out sprite, out name);
                    infoF.classOfItem = classOfItem;
                    infoF.itemData = inventory.GetComponent<InventoryData>().inventories[infoF.id];
                    break;
            }

            item.GetComponent<SpriteRenderer>().sprite = sprite;
            item.name = name;

            MapManager.map[position.x, position.y].hasItem = true;
            MapManager.map[position.x, position.y].itemID = item.GetComponent<ItemInfo>().id;
            MapManager.map[position.x, position.y].itemObject = item;
        }
    }*/

    /*int GetWeapon(WeaponsData data, out Sprite sprite, out string name) {
        int chance = Random.Range(0, data.weapons[0].chance);
        sprite = null;
        name = "";

        for (int i = data.weapons.Length - 1; i >= 0; i--) {
            if (chance <= data.weapons[i].chance) {
                sprite = data.weapons[i].inventorySprite;
                name = data.weapons[i].name;
                return data.weapons[i].itemID;
            }
        }
        return 0;
    }*/

    /*int GetArmor(ArmorsData data, out Sprite sprite, out string name) {
        int chance = Random.Range(0, data.armors[0].chance);
        sprite = null;
        name = "";

        for (int i = data.armors.Length - 1; i >= 0; i--) {
            if (chance <= data.armors[i].chance) {
                sprite = data.armors[i].inventorySprite;
                name = data.armors[i].name;
                return data.armors[i].itemID;
            }
        }
        return 0;
    }*/

    /*int GetRing(RingsData data, out Sprite sprite, out string name) {
        int chance = Random.Range(0, data.rings[0].chance);
        sprite = null;
        name = "";

        for (int i = data.rings.Length - 1; i >= 0; i--) {
            if (chance <= data.rings[i].chance) {
                sprite = data.rings[i].sprite;
                name = data.rings[i].name;
                return data.rings[i].itemID;
            }
        }
        return 0;
    }*/

    /*int GetPotion(PotionsData data, out Sprite sprite, out string name) {
        int chance = Random.Range(0, data.potions[0].chance);
        sprite = null;
        name = "";

        for (int i = data.potions.Length - 1; i >= 0; i--) {
            if (chance <= data.potions[i].chance) {
                sprite = data.potions[i].sprite;
                name = data.potions[i].name;
                return data.potions[i].itemID;
            }
        }
        return 0;
    }*/

    /*int GetScroll(ScrollsData data, out Sprite sprite, out string name) {
        int chance = Random.Range(0, data.scrolls[0].chance);
        Debug.Log(chance);
        sprite = null;
        name = "";

        for (int i = data.scrolls.Length - 1; i >= 0; i--) {
            if (chance <= data.scrolls[i].chance) {
                sprite = data.scrolls[i].sprite;
                name = data.scrolls[i].name;
                return data.scrolls[i].itemID;
            }
        }
        return 0;
    }*/

    /*int GetFood(FoodData data, out Sprite sprite, out string name) {
        int chance = Random.Range(0, data.foods[0].chance);
        sprite = null;
        name = "";

        for (int i = data.foods.Length - 1; i >= 0; i--) {
            if (chance <= data.foods[i].chance) {
                sprite = data.foods[i].sprite;
                name = data.foods[i].name;
                return data.foods[i].itemID;
            }
        }
        return 0;
    }*/

    public void DrawMap(bool isASCII) {
        if (isASCII) {
            Text screen = GameObject.Find("ASCIITest").GetComponent<Text>();

            string asciiMap = "";

            for (int y = (mapHeight - 1); y >= 0; y--) {
                for (int x = 0; x < mapWidth; x++) {
                    if (MapManager.map[x, y] != null) {
                        switch (MapManager.map[x, y].type) {
                            case "Wall":
                                asciiMap += "#";
                                break;
                            case "Floor":
                                asciiMap += ".";
                                break;
                        }
                    }
                    else {
                        asciiMap += " ";
                    }

                    if (x == (mapWidth - 1)) {
                        asciiMap += "\n";
                    }
                }
            }

            screen.text = asciiMap;
        }
        else {
            GameObject parent = GameObject.Find("DungeonManager");
            for (int y = (mapHeight - 1); y >= 0; y--) {
                for (int x = 0; x < mapWidth; x++) {
                    if (MapManager.map[x, y] != null) {
                        GameObject newTile = new GameObject();
                        newTile.AddComponent<SpriteRenderer>();
                        newTile.AddComponent<TileInfo>();
                        newTile.AddComponent<TileVisibility>();
                        newTile.AddComponent<TileOnClick>();
                        newTile.AddComponent<BoxCollider2D>();
                        newTile.GetComponent<BoxCollider2D>().size = new Vector2(tileScale, tileScale);

                        switch (MapManager.map[x, y].type) {
                            case "Wall":
                                newTile.GetComponent<SpriteRenderer>().sprite = walls[Random.Range(0, walls.Length - 1)];
                                break;
                            case "Floor":
                                newTile.GetComponent<SpriteRenderer>().sprite = floors[Random.Range(0, floors.Length - 1)]; ;
                                break;
                        }

                        newTile.transform.position = new Vector3(x * tileScale, y * tileScale, 0);
                        newTile.transform.parent = parent.transform;
                        newTile.name = MapManager.map[x, y].type;
                        MapManager.map[x, y].baseObject = newTile;
                        newTile.GetComponent<TileInfo>().GetData(new Vector2Int(x, y));

                        if (MapManager.map[x, y].isDoor) {
                            GameObject newDoor = GameObject.Instantiate(doorPrefab);

                            newDoor.transform.position = new Vector3(x * tileScale, y * tileScale, -0.1f);
                            newDoor.transform.parent = newTile.transform;

                            newDoor.GetComponent<SpriteRenderer>().sprite = doors[0];
                            newDoor.name = "Door";
                            MapManager.map[x, y].extraObject = newDoor;
                            newDoor.GetComponent<TileInfo>().GetData(new Vector2Int(x, y));
                            newDoor.GetComponent<DoorHandler>().Initialize();
                        }
                    }
                }
            }
        }
    }
}
