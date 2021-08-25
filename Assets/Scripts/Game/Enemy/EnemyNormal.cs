using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyNormal : Unit<EGameStatTypes, EDefaultStatInteractionType> {

    // -- CONSTRUCTORS -- //

    /// <summary>
    /// 
    /// </summary>
    public EnemyNormal(StatFactory<EGameStatTypes, EDefaultStatInteractionType> statFactory) : base(statFactory) {


        // Set the Stat types
        foreach (EGameStatTypes statType in Enum.GetValues(typeof(EDefaultStatType))) {
            unitStats.Add(statType, new StatDepletion<EGameStatTypes, EDefaultStatInteractionType>(0));
        }

    }

    public override void InteractWithStat(StatInteraction<EGameStatTypes, EDefaultStatInteractionType> statInteraction) {
        base.InteractWithStat(statInteraction);
    }






    /// <summary>
    /// The base Health of the enemy.
    /// Must be set through the child class constructor.
    /// </summary>
    
    
    public int GetBaseHealth() {
        return unitStats[EGameStatTypes.HEALTH].asStatDepletion.GetStatBase();
    }

    public int GetFilteredBaseHealth() {
        return unitStats[EGameStatTypes.HEALTH].asStatDepletion.GetStatFiltered();
    }

    public int GetCurrentHealth() {
        return unitStats[EGameStatTypes.HEALTH].asStatDepletion.GetRemainingStatValue();
    }

    /*public int TakeDamage(int newDamage) {
        return health.DepleteStat(newDamage);
    }*/
}
