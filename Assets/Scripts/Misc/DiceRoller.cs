using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller
{
    public static int RollDice (int dices, int faces, int bonus = 0) {
        int roll = 0;

        for (int i = 0; i < dices; i++) {
            int tempRoll = Random.Range(1, faces);
            roll += tempRoll;
        }

        roll += bonus;

        return roll;
    }
}
