using System;

public class StatInteractionRemoveStatFilter<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> : StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>
    where E_STAT_TYPE : Enum
    where E_STAT_INTERACTION_TYPE : Enum {

    public StatInteractionRemoveStatFilter(string filterName, E_STAT_TYPE statToAffect, E_STAT_INTERACTION_TYPE statInteractionType)
        : base(EStatInteractionIntent.REMOVE_STAT_FILTER, statToAffect, statInteractionType) {
        this.filterName = filterName;
    }


}
