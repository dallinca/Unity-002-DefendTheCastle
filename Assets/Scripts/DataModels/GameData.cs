using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int gameSeed; // GameSeed of 0 means no game data
    public EStageName currentStage;
    public int currentLevel;
    public int currentBank;
    public List<CastleData> currentCastlesData;

    public GameData(int gameSeed, EStageName currentStage, int currentLevel, int currentBank, List<CastleData> currentCastlesData) {
        this.gameSeed = gameSeed;
        this.currentStage = currentStage;
        this.currentLevel = currentLevel;
        this.currentBank = currentBank;
        this.currentCastlesData = currentCastlesData;
    }
}
