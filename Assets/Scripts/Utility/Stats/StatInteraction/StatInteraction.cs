using System;

/// <summary>
/// 
/// </summary>
/// <typeparam name="E_STAT_TYPE">The Available Stats (ie. Health, Speed, etc)</typeparam>
/// <typeparam name="E_STAT_INTERACTION_TYPE">The Available Types of Interaction on Stats</typeparam>
public class StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>
    where E_STAT_TYPE : Enum
    where E_STAT_INTERACTION_TYPE : Enum
{
    
    protected IntFilterSequence.Filter filter;
    protected string filterName;
    protected int deplete;
    protected int gain;

    /// <summary>
    /// The Intent of the Interaction on the Statistic
    /// </summary>
    public readonly EStatInteractionIntent STAT_INTERACTION_INTENT;

    /// <summary>
    /// The Stat that is to be affected
    /// </summary>
    public readonly E_STAT_TYPE STAT_TO_AFFECT;

    /// <summary>
    /// The Type of Interaction
    /// </summary>
    public readonly E_STAT_INTERACTION_TYPE STAT_INTERACTION_TYPE;

    /// <summary>
    /// Create a new StatInteraction Object
    /// </summary>
    /// <param name="statInteractionIntent">The Intent of this Interaction Object on the Stat</param>
    /// <param name="statToAffect">The Stat to be affected</param>
    /// <param name="statInteractionType">The Type of interaction</param>
    protected StatInteraction(EStatInteractionIntent statInteractionIntent, E_STAT_TYPE statToAffect, E_STAT_INTERACTION_TYPE statInteractionType) {
        STAT_INTERACTION_INTENT = statInteractionIntent;
        STAT_TO_AFFECT = statToAffect;
        STAT_INTERACTION_TYPE = statInteractionType;
    }

    public IntFilterSequence.Filter GetFilter() {
        return filter;
    }

    public string GetFilterName() {
        return filterName;
    }

    public int GetDepleteValue() {
        return deplete;
    }

    public int GetGainValue() {
        return gain;
    }

    //public StatInteraction() {

    //}

    // Intent Objects
    // - Define a Stat to interact with
    // - Define depletion or gain (if a depletion stat)


}
