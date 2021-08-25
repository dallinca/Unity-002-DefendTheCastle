using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*public class EnemyMan<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> : EnemyNormal<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>
    where E_STAT_TYPES : Enum
    where E_STAT_INTERACTION_TYPES : Enum {

    public static readonly int ENEMY_MAN_BASE_HEALTH = 10;

    public EnemyMan (StatFactory<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> statFactory) : base(statFactory, ENEMY_MAN_BASE_HEALTH) {

    }
}*/

public class EnemyMan : EnemyNormal {

    public static readonly int ENEMY_MAN_BASE_HEALTH = 10;

    public EnemyMan(StatFactory<EGameStatTypes, EDefaultStatInteractionType> statFactory) : base(statFactory) {
        this.statFactory = statFactory;

        GetStat(EGameStatTypes.HEALTH).asStatDepletion.SetStatBase(ENEMY_MAN_BASE_HEALTH);
    }
}
