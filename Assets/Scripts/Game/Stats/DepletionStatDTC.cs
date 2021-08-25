using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepletionStatDTC : StatDepletion<EDTCStatType, EDTCStatInteractionType> {

    public DepletionStatDTC(int statBase) : base(statBase) { }

    public DepletionStatDTC(int statBase, bool canDepletePastZero, bool canGainPastCeiling) : base(statBase, canDepletePastZero, canGainPastCeiling) { }

}
