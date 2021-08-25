using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CastleData
{
    public List<CastleGateData> castleGatesData;
    // TODO - public List<CastleTowerData> castleTowersData;
    // TODO - public List<CastleBuildingsData> castleBuildingsData;

    public CastleData(List<CastleGateData> castleGatesData) {
        this.castleGatesData = castleGatesData;
    }
}
