using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableRaces : MonoBehaviour
{
    public Body[] bodies;
    public static Race[] races;

    public Body GetRace(string name) {
        foreach (Race race in races) {
            if (name == race.name) return race.body;
        }

        return null;
    }
}

[System.Serializable]
public class Race {
    public string name;
    public Body body;
    public Attributes attributes;
}