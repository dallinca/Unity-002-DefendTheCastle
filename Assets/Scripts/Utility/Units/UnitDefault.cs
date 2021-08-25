using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitDefault : Unit<EDefaultStatType, EDefaultStatInteractionType> {

    public static readonly int UNIT_BASE_HEALTH = 1000;

    // -- CONSTRUCTORS -- //

    /// <summary>
    /// 
    /// </summary>
    public UnitDefault(StatFactoryDefault statFactory) : base(statFactory) {
        unitStats.Add(EDefaultStatType.HEALTH, new StatDepletionDefault(UNIT_BASE_HEALTH));
        unitStats.Add(EDefaultStatType.MOVEMENT_SPEED, new StatReferenceDefault(10));
    }

    // -- METHODS -- //


}
