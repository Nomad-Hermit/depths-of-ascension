using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager 
{
    public static Attributes attributes;
    static GameManager manager;
    /*public static PlayerHealth health;
    public static PlayerLevel level;*/

    static int baseAC = 12;
    public static int baseAttack = 0;
    public static int baseBonus = 0;

    public static bool isParalyzed = false;
    public static int paralizationTurns = 0;

    public static bool isPoisoned;
    public static int poisonTurns;
    public static int poisonStrength;

    public static void Initialize() {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        /*health = GameObject.Find("Player(Clone)").GetComponent<PlayerHealth>();
        level = GameObject.Find("Player(Clone)").GetComponent<PlayerLevel>();
        WeaponsHandler.AddWeapon(-1);
        ArmorHandler.AddArmor(-1);
        RingEffects.Initialize();
        RingsHandler.AddRing(-1);*/
        //attributes = new Attributes();

        PullArmorData();
        PullWeaponData();
        /*attributes.baseAttack = baseAttack;
        attributes.bonusDamage = baseBonus;*/

        isParalyzed = false;
        paralizationTurns = 0;

        isPoisoned = false;
        poisonTurns = 0;
        poisonStrength = 0;
    }

    public static void PullArmorData() {
        //attributes.ac = baseAC + ArmorHandler.armor.acBonus + RingEffects.status.acBonus;
    }

    public static void PullWeaponData() {
        /*attributes.baseAttack = baseAttack + WeaponsHandler.weapon.baseAttack + RingEffects.status.attackBonus;
        attributes.diceDamage = WeaponsHandler.weapon.diceDamage;
        attributes.rollDamage = WeaponsHandler.weapon.rollDamage;
        attributes.bonusDamage = baseBonus + WeaponsHandler.weapon.bonusDamage;*/
    }

    public static void Paralyze(int turns) {
        isParalyzed = true;
        paralizationTurns = turns;
    }

    public static void Poison(int turns, int power) {
        isPoisoned = true;
        poisonTurns = turns;
        poisonStrength = power;
    }
}
