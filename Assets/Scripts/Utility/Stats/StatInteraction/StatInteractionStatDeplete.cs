using System;

public class StatInteractionStatDeplete<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> : StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>
    where E_STAT_TYPE : Enum
    where E_STAT_INTERACTION_TYPE : Enum {

    public StatInteractionStatDeplete(int deplete, E_STAT_TYPE statToAffect, E_STAT_INTERACTION_TYPE statInteractionType)
        : base(EStatInteractionIntent.STAT_DEPLETE, statToAffect, statInteractionType) {
        this.deplete = deplete;
    }


}
