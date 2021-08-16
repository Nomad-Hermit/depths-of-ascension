using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorsTurnManager 
{
    public static List<Actor> actors;
    public static int tick;

    public static void AddActor(GameObject actor, int actionBar, string name, string type) {
        actors.Add(new Actor (actor, actionBar, name, type));
    }

    public static void RemoveActor() {

    }

    public static void Tick() {
        foreach (Actor actor in actors) {
            actor.actionBar--;
        }
        tick++;
    }

    public static bool GetNextActor(out Actor nextActor) {
        foreach (Actor actor in actors) {
            if (actor.actionBar <= 0) {
                nextActor = actor;
                return true;
            }
        }
        nextActor = null;
        return false;
    }
}

public class Actor {
    public string name;
    public string type;
    public GameObject actor;
    public int actionBar;

    public Actor (GameObject actor, int actionBar, string name, string type) {
        this.name = name;
        this.type = type;
        this.actor = actor;
        this.actionBar = actionBar;
    }
}
