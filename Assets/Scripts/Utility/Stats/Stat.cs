using UnityEngine;
using System;
using System.Collections.Generic;

public class Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>
    where E_STAT_TYPES : Enum
    where E_STAT_INTERACTION_TYPES : Enum
{

    Classes statClass = Classes.NONE;

    public StatReference<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> asStatReference;
    public StatDepletion<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> asStatDepletion;
    public StatSequence<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> asStatSequence;
    public StatSequenceByInteractionType<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> asStatSequenceByInteractionType;

    /*
     * Only allow child classes be directly instantiated.
     */
    protected Stat(Classes statClass) {
        this.statClass = statClass;
    }

    public Classes getStatClass() {
        return statClass;
    }

    public enum Classes {
        NONE, DEPLETION, REFERENCE, SEQUENCE, SEQUENCE_BY_INTERACTION_TYPE
    }
}
