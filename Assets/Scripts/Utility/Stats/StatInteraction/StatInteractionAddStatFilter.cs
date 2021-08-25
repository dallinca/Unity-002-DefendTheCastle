using System;

public class StatInteractionAddStatFilter<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> : StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>
    where E_STAT_TYPE : Enum
    where E_STAT_INTERACTION_TYPE : Enum {

    public StatInteractionAddStatFilter(IntFilterSequence.Filter filter, E_STAT_TYPE statToAffect, E_STAT_INTERACTION_TYPE statInteractionType)
        : base(EStatInteractionIntent.ADD_STAT_FILTER, statToAffect, statInteractionType) {
        this.filter = filter;
    }

}
