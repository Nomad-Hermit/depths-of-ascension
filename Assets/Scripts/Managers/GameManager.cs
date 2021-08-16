using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DungeonGenerator dungeon;
    
    // Start is called before the first frame update
    void Start()
    {
        dungeon.GoGenerate(false, true, this);
    }

    
}
