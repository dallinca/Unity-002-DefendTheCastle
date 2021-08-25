using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitDTC : Unit<EDTCStatType, EDTCStatInteractionType> {

    private int UNIT_BASE_HEALTH = 1000;
    private int UNIT_BASE_SPEED = 1;

    // -- CONSTRUCTORS -- //

    /// <summary>
    /// 
    /// </summary>
    public UnitDTC() : base(StatFactoryDTC.Instance()) {
        unitStats.Add(EDTCStatType.HEALTH, new DepletionStatDTC(UNIT_BASE_HEALTH));
        unitStats.Add(EDTCStatType.MOVEMENT_SPEED, new ReferenceStatDTC(UNIT_BASE_SPEED));
    }

    public UnitDTC(int baseHealth, int baseSpeed) : base(StatFactoryDTC.Instance()) {
        unitStats.Add(EDTCStatType.HEALTH, new DepletionStatDTC(baseHealth));
        unitStats.Add(EDTCStatType.MOVEMENT_SPEED, new ReferenceStatDTC(baseSpeed));
    }

    // -- METHODS -- //


}
