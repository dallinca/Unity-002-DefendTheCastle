using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatInteractionDefault : StatInteraction<EDefaultStatType,EDefaultStatInteractionType>
{

    public StatInteractionDefault(EStatInteractionIntent statInteractionIntent, EDefaultStatType statToAffect, EDefaultStatInteractionType statInteractionType) : base(statInteractionIntent, statToAffect, statInteractionType) {

    }

}
