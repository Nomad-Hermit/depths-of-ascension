using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats 
{
    public static Body body;
    public static Body baseBody;
    public static Weapon weapon;
    public static Attributes attributes;

    public void EquipArmor(Armor newArmor) {
        for (int i = 0; i < baseBody.bodyParts.Length; i++) {
            if (newArmor.bodyPart == baseBody.bodyParts[i].type) {
                body.bodyParts[i].naturalArmor += newArmor.armor;
            }
        }
    }

    public static void RollAttributes(string name) {
        //attributes = new Attributes();

        int[] newAttributes = new int[6];
        for (int i = 0; i < newAttributes.Length; i++) {
            newAttributes[i] = DiceRoller.RollDice(3, 6);
        }

        for (int i = 0; i < newAttributes.Length - 1; i++) {
            for (int j = i + 1; j < newAttributes.Length; j++) {
                if (j > i) {
                    int temp = j;
                    i = temp;
                    j = i;
                }
            }
        }

        switch (name) {
            case "knightTgl":
                attributes.strength = newAttributes[1];
                attributes.dexterity = newAttributes[3];
                attributes.constitution = newAttributes[2];
                attributes.intelligence = newAttributes[4];
                attributes.wisdom = newAttributes[5];
                attributes.charisma = newAttributes[6];
                break;
            case "swashTgl":
                attributes.strength = newAttributes[3];
                attributes.dexterity = newAttributes[1];
                attributes.constitution = newAttributes[2];
                attributes.intelligence = newAttributes[4];
                attributes.wisdom = newAttributes[5];
                attributes.charisma = newAttributes[6];
                break;
        }
    }

    public static void AddRacialAttributes(string playerRace) {
        foreach (Race race in PlayableRaces.races) {
            if (race.name == playerRace) {
                attributes += race.attributes;
            }
        }
    }

}

[System.Serializable]
public class Attributes {
    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;

    public Attributes (int rStrength, int rDexterity, int rConstitution, int rIntelligence, int rWisdom, int rCharisma) {
        strength = rStrength;
        dexterity = rDexterity;
        constitution = rConstitution;
        intelligence = rIntelligence;
        wisdom = rWisdom;
        charisma = rCharisma;

    }

    public static Attributes operator + (Attributes attributes1, Attributes attributes2) {
        return new Attributes(attributes1.strength + attributes2.strength,
            attributes1.dexterity + attributes2.dexterity,
            attributes1.constitution + attributes2.constitution,
            attributes1.intelligence + attributes2.intelligence,
            attributes1.wisdom + attributes2.wisdom,
            attributes1.charisma + attributes2.charisma);
    }
}

[System.Serializable]
public class Stats {
    public float speed;

    public int baseAttack;
    public int diceDamage;
    public int rollDamage;
    public int bonusDamage;
}

[System.Serializable]
public class Body {
    public string race;
    public BodyPart[] bodyParts;
}

[System.Serializable]
public class BodyPart {
    public string type;
    public string position;
    public int naturalArmor;
    public float hitChance;
    public int health;
    public int maxHealth;
    public string status;
    public bool isDeadly = false;
}

[System.Serializable]
public class Weapon {
    public string name;
    public string type;
    public int baseDamage;
    public int diceDamage;
    public int bonusDamage;
    public int attackBonus;
}

[System.Serializable]
public class Armor {
    public string name;
    public string bodyPart;
    public int armor;
    public int agilityPenalty;
}