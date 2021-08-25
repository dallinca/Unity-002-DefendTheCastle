using UnityEngine;
using System;
using System.Collections.Generic;

public class Unit<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>
    where E_STAT_TYPES : Enum
    where E_STAT_INTERACTION_TYPES : Enum
{

    // Must set stat Factory for the unit
    protected StatFactory<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> statFactory;

    // Stat Interaction Filters
    protected Dictionary<string, StatInteractionFilter<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>> statInteractionFilters;

    // Unit Stats
    //protected Dictionary<E_STAT_TYPES, StatDepletion<E_STAT_INTERACTION_TYPES, E_STAT_INTERACTION_TYPES>> unitStats = new Dictionary<E_STAT_TYPES, StatDepletion<E_STAT_INTERACTION_TYPES, E_STAT_INTERACTION_TYPES>>();
    protected Dictionary<E_STAT_TYPES, Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>> unitStats = new Dictionary<E_STAT_TYPES, Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>>();

    // Has Depletion Stats

    // Has Reference Stats

    public Unit (StatFactory<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> statFactory) {
        SetStatFactory(statFactory);
        
        // Generate default StatDepletion instance for each Stat Type in E_STAT_TYPES
        /*foreach (E_STAT_TYPES statType in Enum.GetValues(typeof(E_STAT_TYPES))) {
            unitStats.Add(statType, new StatDepletion<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>(0));
        }*/
    }


    // Receives Interaction Intent Obects
    // - Passes them along
    // - May Modify Intent Objects according to settings in the Unit
    public virtual void InteractWithStat(StatInteraction<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> statInteraction) {

        // Find the desired Stat to affect
        Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> statToAffect;
        if (!unitStats.TryGetValue(statInteraction.STAT_TO_AFFECT, out statToAffect)) {
            // Return if the Stat object was not found
            Debug.Log("Tried to affect non-existent stat: Ensure all stats are being generated in constructor");
            return;
        }

        // Pass to StatInteractionFilter mechanism
        // TODO

        // Return if the Stat object was not found

        switch(statToAffect.getStatClass()) {
            case Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>.Classes.DEPLETION:

                switch (statInteraction.STAT_INTERACTION_INTENT) {
                    case EStatInteractionIntent.ADD_STAT_FILTER:
                        statToAffect.asStatDepletion.AddStatFilter(statInteraction.GetFilter());
                        break;
                    case EStatInteractionIntent.REMOVE_STAT_FILTER:
                        statToAffect.asStatDepletion.RemoveStatFilter(statInteraction.GetFilterName());
                        break;
                    case EStatInteractionIntent.STAT_GAIN:
                        statToAffect.asStatDepletion.GainStat(statInteraction.GetGainValue(), statInteraction.STAT_INTERACTION_TYPE);
                        break;
                    case EStatInteractionIntent.ADD_STAT_GAIN_FILTER:
                        statToAffect.asStatDepletion.AddGainFilter(statInteraction.GetFilter(), statInteraction.STAT_INTERACTION_TYPE);
                        break;
                    case EStatInteractionIntent.ADD_STAT_DEPLETE_FILTER:
                        statToAffect.asStatDepletion.AddDepletionFilter(statInteraction.GetFilter(), statInteraction.STAT_INTERACTION_TYPE);
                        break;
                    case EStatInteractionIntent.STAT_DEPLETE:
                        statToAffect.asStatDepletion.DepleteStat(statInteraction.GetDepleteValue(), statInteraction.STAT_INTERACTION_TYPE);
                        break;
                    case EStatInteractionIntent.REMOVE_STAT_GAIN_FILTER:
                        statToAffect.asStatDepletion.RemoveGainFilter(statInteraction.GetFilterName(), statInteraction.STAT_INTERACTION_TYPE);
                        break;
                    case EStatInteractionIntent.REMOVE_STAT_DEPLETE_FILTER:
                        statToAffect.asStatDepletion.RemoveDepletionFilter(statInteraction.GetFilterName(), statInteraction.STAT_INTERACTION_TYPE);
                        break;
                }

                break;
            case Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>.Classes.REFERENCE:

                switch (statInteraction.STAT_INTERACTION_INTENT) {
                    case EStatInteractionIntent.ADD_STAT_FILTER:
                        statToAffect.asStatReference.AddStatFilter(statInteraction.GetFilter());
                        break;
                    case EStatInteractionIntent.REMOVE_STAT_FILTER:
                        statToAffect.asStatReference.RemoveStatFilter(statInteraction.GetFilterName());
                        break;
                    default:
                        Debug.Log("Attempting to interact with Reference Stat in invalid way");
                        break;
                }

                break;
            case Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>.Classes.SEQUENCE:

                switch (statInteraction.STAT_INTERACTION_INTENT) {
                    case EStatInteractionIntent.ADD_STAT_FILTER:
                        statToAffect.asStatReference.AddStatFilter(statInteraction.GetFilter());
                        break;
                    case EStatInteractionIntent.REMOVE_STAT_FILTER:
                        statToAffect.asStatReference.RemoveStatFilter(statInteraction.GetFilterName());
                        break;
                    default:
                        Debug.Log("Attempting to interact with Reference Stat in invalid way");
                        break;
                }

                break;
            case Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>.Classes.SEQUENCE_BY_INTERACTION_TYPE:

                switch (statInteraction.STAT_INTERACTION_INTENT) {
                    case EStatInteractionIntent.ADD_STAT_FILTER:
                        statToAffect.asStatReference.AddStatFilter(statInteraction.GetFilter());
                        break;
                    case EStatInteractionIntent.REMOVE_STAT_FILTER:
                        statToAffect.asStatReference.RemoveStatFilter(statInteraction.GetFilterName());
                        break;
                    default:
                        Debug.Log("Attempting to interact with Reference Stat in invalid way");
                        break;
                }

                break;
            default:
                break;
        }
    }

    public void SetStatFactory(StatFactory<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> statFactory) {
        this.statFactory = statFactory;
    }

    /// <summary>
    /// Checks if the Unit has the named statInteractionFilter
    /// </summary>
    /// <param name="filterName"></param>
    /// <returns>Whether the filterName already exists</returns>
    public bool HasStatInteractionFilter(string filterName) {
        if (statInteractionFilters.ContainsKey(filterName)) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes the named statInteractionFilter if it exists
    /// </summary>
    /// <param name="filterName"></param>
    public void RemoveStatInteractionFilter(string filterName) {
        if (HasStatInteractionFilter(filterName)) {
            statInteractionFilters.Remove(filterName);
        }
    }

    /// <summary>
    /// Adds the name statInteractionFilter if the name doesn't already exist
    /// </summary>
    /// <param name="filterName"></param>
    /// <param name="statInteractionFilter"></param>
    /// <returns>Whether the filter was added</returns>
    public bool AddStatInteractionFilter(string filterName, StatInteractionFilter<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> statInteractionFilter) {
        if (HasStatInteractionFilter(filterName)) {
            return false;
        }

        statInteractionFilters.Add(filterName, statInteractionFilter);
        return true;
    }

    /// <summary>
    /// Adds the name statInteractionFilter or replaces it if is already exists
    /// </summary>
    /// <param name="filterName"></param>
    /// <param name="statInteractionFilter"></param>
    public void UpdateStatInteractionFilter(string filterName, StatInteractionFilter<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> statInteractionFilter) {
        RemoveStatInteractionFilter(filterName);
        statInteractionFilters.Add(filterName, statInteractionFilter);
    }




    public Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> GetStat(E_STAT_TYPES statToGet) {
        return unitStats[statToGet];
    }
}
