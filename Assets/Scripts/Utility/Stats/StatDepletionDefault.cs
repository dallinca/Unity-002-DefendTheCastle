using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDepletionDefault : StatDepletion<EDefaultStatType, EDefaultStatInteractionType> {

    public StatDepletionDefault(int statBase) : base(statBase) {}

    public StatDepletionDefault(int statBase, bool canDepletePastZero, bool canGainPastCeiling) : base(statBase, canDepletePastZero, canGainPastCeiling) {}

}
