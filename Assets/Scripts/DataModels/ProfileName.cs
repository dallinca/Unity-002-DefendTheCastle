using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains only the profile name. Intended for storing a list
/// of profile names that can be used to before choosing a specific
/// profile to load into the game.
/// </summary>
[System.Serializable]
public class ProfileName {
    public string name;

    public ProfileName(string name) {
        this.name = name;
    }
}
