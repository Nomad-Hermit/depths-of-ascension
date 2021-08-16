using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2Int position;

    GameManager manager;
    DungeonGenerator dungeon;

    private void Start() {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dungeon = GameObject.Find("DungeonManager").GetComponent<DungeonGenerator>();
    }

    public void Initialize(Vector2Int newPosition) {
        position = newPosition;
    }

    private void Update() {
        /*if (!manager.isPlayerTurn) return;

        if (PlayerManager.isParalyzed) {
            PlayerManager.paralizationTurns--;
            if (PlayerManager.paralizationTurns <= 0) {
                PlayerManager.isParalyzed = false;
            }
            manager.FinishPlayersTurn();
        }

        if (manager.hasPath) {
            TileInteractions.TryToWalk(1);
        }*/
    }

    Vector2Int InputToVector(int x, int y) {
        Vector2Int target = new Vector2Int(position.x + x, position.y + y);

        return target;
    }

    public void Move(Vector2Int target) {
        //Debug.Log(target);
        if (MapManager.map[target.x, target.y].isWalkable && !MapManager.map[target.x, target.y].hasEnemy) {
            MapManager.map[position.x, position.y].hasPlayer = false;
            position = target;
            gameObject.transform.position = new Vector3(position.x * dungeon.tileScale, position.y * dungeon.tileScale, -1);
            MapManager.map[position.x, position.y].hasPlayer = true;
            //Debug.Log("Moved to " + position);
        }
        if (!MapManager.map[target.x, target.y].isWalkable && MapManager.map[target.x, target.y].isDoor) {
            MapManager.map[target.x, target.y].extraObject.GetComponent<DoorHandler>().OpenDoor();
        }
        if (MapManager.map[target.x, target.y].hasEnemy) {
            Attack(MapManager.enemies[MapManager.map[target.x, target.y].enemyID]);
        }
        if (MapManager.map[target.x, target.y].isWalkable && !MapManager.map[target.x, target.y].hasEnemy && MapManager.map[target.x, target.y].isExit) {
            //manager.DownALevel();
        }
        if (MapManager.map[target.x, target.y].hasItem) {
            //InventoryManager.AddItem(MapManager.map[target.x, target.y].itemObject.GetComponent<ItemInfo>().itemData);
            Destroy(MapManager.map[target.x, target.y].itemObject);
            MapManager.map[target.x, target.y].hasItem = false;
            MapManager.map[target.x, target.y].itemID = -1;
        }

        //manager.FinishPlayersTurn();
    }

    void Attack(Enemy enemy) {
        /*// Rolls the dice for the attack.
        int roll = RollDice(1, 20);

        //If the attack roll plus the baseAttack from the enemy is bigger than the enemy's armor class, deal damage.
        if ((PlayerManager.attributes.baseAttack + roll) > enemy.ac) {
            int damage = RollDice(WeaponsHandler.weapon.diceDamage, WeaponsHandler.weapon.rollDamage) + PlayerManager.attributes.bonusDamage;
            FeedManager.PlayerAttack(enemy.enemyClass, true);
            enemy.enemyObject.GetComponent<EnemyHealth>().DealDamage(damage);
        }
        else {
            FeedManager.PlayerAttack(enemy.enemyClass, false);
            StartCoroutine(enemy.enemyObject.GetComponent<EnemyHealth>().DisplayMiss());
        }
        Debug.Log("Attack!");*/
    }

    // Roll the damage of the attack.
    int RollDice(int quantity, int dice) {
        int roll = 0;
        for (int i = 0; i < quantity; i++) {
            roll += (int)Random.Range(1, dice);
        }

        return roll;
    }
}
