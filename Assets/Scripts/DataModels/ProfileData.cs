using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the information of a specific Profile. Used to
/// restore gamestate for a specific Profile.
/// </summary>
[System.Serializable]
public class ProfileData
{
    public string name;
    public List<EStageName> stagesUnlocked;
    public GameData currentGame;

    public ProfileData(string name, List<EStageName> stagesUnlocked, GameData currentGame) {
        this.name = name;
        this.stagesUnlocked = stagesUnlocked;
        this.currentGame = currentGame;
    }

    public ProfileData(string name) {
        this.name = name;
        stagesUnlocked = new List<EStageName>() {
            EStageName.HELLO_WORLD
        };
        currentGame = null;
    }
}
