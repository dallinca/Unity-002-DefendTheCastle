using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProfileNames
{
    public List<ProfileName> profileNames;

    public ProfileNames(List<ProfileName> profileNames) {
        this.profileNames = profileNames;
    }
}
