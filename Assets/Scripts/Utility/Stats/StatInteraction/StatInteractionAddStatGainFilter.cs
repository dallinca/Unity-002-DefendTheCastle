using System;

public class StatInteractionAddStatGainFilter<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> : StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>
    where E_STAT_TYPE : Enum
    where E_STAT_INTERACTION_TYPE : Enum {

    public StatInteractionAddStatGainFilter(IntFilterSequence.Filter filter, E_STAT_TYPE statToAffect, E_STAT_INTERACTION_TYPE statInteractionType)
        : base(EStatInteractionIntent.ADD_STAT_GAIN_FILTER, statToAffect, statInteractionType) {
        this.filter = filter;
    }


}
