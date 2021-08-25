using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CastleGateData
{
    public int currentHealth;

    public CastleGateData(int currentHealth) {
        this.currentHealth = currentHealth;
    }
}
