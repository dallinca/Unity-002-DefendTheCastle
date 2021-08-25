using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="E_STAT_TYPES">The Available Stats (ie. Health, Speed, etc)</typeparam>
/// <typeparam name="E_STAT_INTERACTION_TYPES">The Available Types of Interaction on Stats</typeparam>
public class StatFactory<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>
    where E_STAT_TYPES : Enum
    where E_STAT_INTERACTION_TYPES : Enum
{
    // Creates Stats and returns them
    public StatReference<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> NewStatReference(int statBase) {
        return new StatReference<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(statBase);
    }

    public StatDepletion<E_STAT_INTERACTION_TYPES, E_STAT_INTERACTION_TYPES> NewStatDepletion(int statBase) {
        return new StatDepletion<E_STAT_INTERACTION_TYPES, E_STAT_INTERACTION_TYPES>(statBase);
    }

    public StatDepletion<E_STAT_INTERACTION_TYPES, E_STAT_INTERACTION_TYPES> NewStatDepletion(int statBase, bool canDepletePastZero, bool canGainPastCeiling) {
        return new StatDepletion<E_STAT_INTERACTION_TYPES, E_STAT_INTERACTION_TYPES>(statBase, canDepletePastZero, canGainPastCeiling);
    }

    // Creates Stat Interaction Objects and returns them
    /// <summary>
    /// Creates and returns a StatInteraction Object. Returns Null if provided value is not valid.
    /// </summary>
    /// <param name="statInteractionIntent"></param>
    /// <returns></returns>
    public StatInteraction<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> NewStatInteractionObject(EStatInteractionIntent statInteractionIntent, E_STAT_TYPES statToAffect, E_STAT_INTERACTION_TYPES statInteractionType, int valueToAddDeplete) {
        switch (statInteractionIntent) {
            case EStatInteractionIntent.STAT_GAIN:
                return new StatInteractionStatGain<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(valueToAddDeplete, statToAffect, statInteractionType);
            case EStatInteractionIntent.STAT_DEPLETE:
                return new StatInteractionStatDeplete<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(valueToAddDeplete, statToAffect, statInteractionType);
            default:
                Debug.Log("StatFactory.StatInteraction: Provided Arguments for AddDeplete Interaction Object is not valid");
                break;
        }

        return null;
    }

    // Creates Stat Interaction Objects and returns them
    /// <summary>
    /// Creates and returns a StatInteraction Object. Returns Null if provided value is not valid.
    /// </summary>
    /// <param name="statInteractionIntent"></param>
    /// <returns></returns>
    public StatInteraction<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> NewStatInteractionObject(EStatInteractionIntent statInteractionIntent, E_STAT_TYPES statToAffect, E_STAT_INTERACTION_TYPES statInteractionType, string filterToRemove) {
        switch (statInteractionIntent) {
            case EStatInteractionIntent.REMOVE_STAT_FILTER:
                return new StatInteractionRemoveStatFilter<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(filterToRemove, statToAffect, statInteractionType);
            case EStatInteractionIntent.REMOVE_STAT_GAIN_FILTER:
                return new StatInteractionRemoveStatGainFilter<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(filterToRemove, statToAffect, statInteractionType);
            case EStatInteractionIntent.REMOVE_STAT_DEPLETE_FILTER:
                return new StatInteractionRemoveStatDepleteFilter<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(filterToRemove, statToAffect, statInteractionType);
            default:
                Debug.Log("StatFactory.StatInteraction: Provided Arguments for RemoveFilter Interaction Object is not valid");
                break;
        }

        return null;
    }

    // Creates Stat Interaction Objects and returns them
    /// <summary>
    /// Creates and returns a StatInteraction Object. Returns Null if provided value is not valid.
    /// </summary>
    /// <param name="statInteractionIntent"></param>
    /// <returns></returns>
    public StatInteraction<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> NewStatInteractionObject(EStatInteractionIntent statInteractionIntent, E_STAT_TYPES statToAffect, E_STAT_INTERACTION_TYPES statInteractionType, IntFilterSequence.Filter filterToAdd) {
        switch (statInteractionIntent) {
            case EStatInteractionIntent.ADD_STAT_FILTER:
                return new StatInteractionAddStatFilter<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(filterToAdd, statToAffect, statInteractionType);
            case EStatInteractionIntent.ADD_STAT_GAIN_FILTER:
                return new StatInteractionAddStatGainFilter<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(filterToAdd, statToAffect, statInteractionType);
            case EStatInteractionIntent.ADD_STAT_DEPLETE_FILTER:
                return new StatInteractionAddStatDepleteFilter<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(filterToAdd, statToAffect, statInteractionType);
            default:
                Debug.Log("StatFactory.StatInteraction: Provided Arguments for AddFilter Interaction Object is not valid");
                break;
        }

        return null;
    }
}
