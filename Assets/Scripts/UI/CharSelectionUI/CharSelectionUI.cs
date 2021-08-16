using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectionUI : MonoBehaviour
{
    public Toggle knightTgl;
    public Toggle swashTgl;
    public Toggle assassinTgl;

    public Button playBtn;

    public PlayableRaces races;
    
    // Start is called before the first frame update
    void Start()
    {
        knightTgl.isOn = false;
        swashTgl.isOn = false;
        assassinTgl.isOn = false;

        playBtn.interactable = false;
    }

    public void ToggleSelection(string name) {
        string race = "";
        switch (name) {
            case "knightTgl":


                if (knightTgl.isOn) {
                    swashTgl.isOn = false;
                    assassinTgl.isOn = false;
                    race = "dwarf";
                }
                break;
            case "swashTgl":
                if (swashTgl.isOn) {
                    knightTgl.isOn = false;
                    assassinTgl.isOn = false;
                    race = "elf";
                }
                break;
            case "assassinTgl":
                if (assassinTgl.isOn) {
                    knightTgl.isOn = false;
                    swashTgl.isOn = false;
                }
                break;
        }

        if (knightTgl.isOn || swashTgl.isOn || assassinTgl.isOn) {
            PlayerStats.body = races.GetRace(race);
            PlayerStats.baseBody = races.GetRace(race);
            PlayerStats.RollAttributes(name);
            PlayerStats.AddRacialAttributes(race);
            playBtn.interactable = true;
        }
        else {
            PlayerStats.body = null;
            PlayerStats.baseBody = null;
            playBtn.interactable = false;
        }
    }
}
