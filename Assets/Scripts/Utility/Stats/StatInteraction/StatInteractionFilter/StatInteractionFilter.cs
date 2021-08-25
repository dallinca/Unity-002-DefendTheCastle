using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatInteractionFilter<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>
    where E_STAT_TYPE : Enum
    where E_STAT_INTERACTION_TYPE : Enum
{
    // -- INSTANCE MEMBERS -- //

    List<Qualifier> qualifiers; // What qualifies the Filter to fire
    List<FilterAction> filterActions; // All the actions to fire upon qualification

    // -- CONSTRUCTORS --//

    public StatInteractionFilter() {
        qualifiers = new List<Qualifier>();
        filterActions = new List<FilterAction>();
    }

    public StatInteractionFilter(Qualifier qualifier, FilterAction filterAction) {
        qualifiers = new List<Qualifier>();
        filterActions = new List<FilterAction>();

        if (null != qualifier) {
            qualifiers.Add(qualifier);
        }

        if (null != filterAction) {
            filterActions.Add(filterAction);
        }
    }

    public StatInteractionFilter(List<Qualifier> qualifiers, List<FilterAction> filterActions) {
        qualifiers = new List<Qualifier>();
        filterActions = new List<FilterAction>();

        foreach (var qualifier in qualifiers) {
            if (null != qualifier) {
                this.qualifiers.Add(qualifier);
            }
        }

        foreach (var filterAction in filterActions) {
            if (null != filterAction) {
                this.filterActions.Add(filterAction);
            }
        }
    }

    // -- INSTANCE METHODS -- //

    public void AddQualifier(Qualifier qualifier) {
        if (null != qualifier) {
            qualifiers.Add(qualifier);
        }
    }

    public bool Qualifies(E_STAT_TYPE statType, E_STAT_INTERACTION_TYPE statInteractionType, EStatInteractionIntent statInteractionIntent) {
        foreach (var qualifier in qualifiers) {
            if (Convert.ToInt32(qualifier.statType) != Convert.ToInt32(statType)) {
                continue;
            }

            if (Convert.ToInt32(qualifier.statInteractionType) != Convert.ToInt32(statInteractionType)) {
                continue;
            }

            if (Convert.ToInt32(qualifier.statInteractionIntent) != Convert.ToInt32(statInteractionIntent)) {
                continue;
            }

            // Found a matching qualifier
            return true;
        }

        // Could not find a matching qualifier
        return false;
    }

    public void AddFilterAction(FilterAction filterAction) {
        if (null != filterAction) {
            filterActions.Add(filterAction);
        }
    }

    public bool HasBlockFilterAction() {

        foreach (var filterAction in filterActions) {
            if (EFilterAction.BLOCK_INTERACTION == filterAction.filterAction) {
                return true;
            }
        }

        return false;
    }

    public List<StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>> GetResultingStatInteractions(StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> originalStatInteraction) {
        List<StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>> resultingStatInteractions = new List<StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>>();
        
        foreach (var filterAction in filterActions) {
            if (EFilterAction.ADD_INTERACTION == filterAction.filterAction) {
                resultingStatInteractions.Add(filterAction.GetNewInteraction(originalStatInteraction));
            }
        }

        return resultingStatInteractions;
    }

    // -- INNER ENUMS -- //
    public enum EFilterAction {
        BLOCK_INTERACTION, ADD_INTERACTION
    }

    public enum ENewValueMethod {
        USE_GIVEN, // Doesn't care what the original interaction value was
        BASE_ORIGINAL_WITH_CONSTANT, BASE_ORIGINAL_WITH_SCALAR // Bases the value on the original interaction
    }

    // -- INNER CLASSES -- //

    /// <summary>
    /// Defines the settings of the interaction required to fire off this StatInteractionFilter
    /// </summary>
    public class Qualifier {
        public readonly E_STAT_TYPE statType;
        public readonly E_STAT_INTERACTION_TYPE statInteractionType;
        public readonly EStatInteractionIntent statInteractionIntent;

        public Qualifier(E_STAT_TYPE statType, E_STAT_INTERACTION_TYPE statInteractionType, EStatInteractionIntent statInteractionIntent) {
            this.statType = statType;
            this.statInteractionType = statInteractionType;
            this.statInteractionIntent = statInteractionIntent;
        }
    }

    /// <summary>
    /// Base class for the action to be taken when the StatInteractionFilter qualifies to affect a given interaction.
    /// </summary>
    public class FilterAction {
        public readonly EFilterAction filterAction;

        protected FilterAction(EFilterAction filterAction) {
            this.filterAction = filterAction;
        }

        public StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> GetNewInteraction(StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> originalInteraction) {
            return null;
        }
    }

    /// <summary>
    /// Blocks the Interaction from taking effect
    /// </summary>
    public class FilterActionBlockInteraction : FilterAction {

        public FilterActionBlockInteraction() : base(EFilterAction.BLOCK_INTERACTION) {

        }
    }

    /// <summary>
    /// Adds an additional Interaction that is not affected by other StatInteractionFilters
    /// </summary>
    public class FilterActionAddInteraction : FilterAction {

        private StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> statInteraction;

        // Modifier defaults to being the exact same value
        private ENewValueMethod newValueMethod = ENewValueMethod.USE_GIVEN;
        private float modifierValue = 1;

        public FilterActionAddInteraction(StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> statInteraction) : base(EFilterAction.ADD_INTERACTION) {
            this.statInteraction = statInteraction;
        }

        public FilterActionAddInteraction(StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> statInteraction, ENewValueMethod newValueMethod, float modifierValue) : base(EFilterAction.ADD_INTERACTION) {
            this.statInteraction = statInteraction;
            this.newValueMethod = newValueMethod;
            this.modifierValue = modifierValue;
        }

        /// <summary>
        /// Gets the new Interaction to Add. Must supply the original Interaction that triggered this AddInteraction. 
        /// </summary>
        /// <param name="originalInteraction">The original Interaction that triggered this AddInteraction</param>
        /// <returns></returns>
        public new StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> GetNewInteraction(StatInteraction<E_STAT_TYPE, E_STAT_INTERACTION_TYPE> originalInteraction) {
            // If using the original value, just return the statInteraction
            if (ENewValueMethod.USE_GIVEN == newValueMethod) {
                return statInteraction;
            }

            // If removing a Stat, a value is irrelevant
            if (EStatInteractionIntent.REMOVE_STAT_DEPLETE_FILTER == statInteraction.STAT_INTERACTION_INTENT ||
                EStatInteractionIntent.REMOVE_STAT_GAIN_FILTER == statInteraction.STAT_INTERACTION_INTENT ||
                EStatInteractionIntent.REMOVE_STAT_FILTER == statInteraction.STAT_INTERACTION_INTENT
            ) {
                return statInteraction;
            }

            // continue on to find the new value to set
            float originalValue = 1;

            // Retrieve the original value of the deplete/gain/filter in the original interaction
            switch(originalInteraction.STAT_INTERACTION_INTENT) {
                case EStatInteractionIntent.STAT_DEPLETE:
                    originalValue = originalInteraction.GetDepleteValue();
                    break;
                case EStatInteractionIntent.STAT_GAIN:
                    originalValue = originalInteraction.GetGainValue();
                    break;
                case EStatInteractionIntent.ADD_STAT_DEPLETE_FILTER:
                    originalValue = originalInteraction.GetFilter().GetValue();
                    break;
                case EStatInteractionIntent.ADD_STAT_GAIN_FILTER:
                    originalValue = originalInteraction.GetFilter().GetValue();
                    break;
                case EStatInteractionIntent.ADD_STAT_FILTER:
                    originalValue = originalInteraction.GetFilter().GetValue();
                    break;
                default:
                    break;
            }

            // Prep the new value
            int newValue = (int)Math.Round(modifierValue * originalValue, MidpointRounding.AwayFromZero);
            IntFilterSequence.Filter newFilter = statInteraction.GetFilter();

            // If adding a filter, prep the new inner filter;
            if (EStatInteractionIntent.ADD_STAT_DEPLETE_FILTER == statInteraction.STAT_INTERACTION_INTENT ||
                EStatInteractionIntent.ADD_STAT_GAIN_FILTER == statInteraction.STAT_INTERACTION_INTENT ||
                EStatInteractionIntent.ADD_STAT_FILTER == statInteraction.STAT_INTERACTION_INTENT
            ) {
                switch (statInteraction.GetFilter().GetType()) {
                    case IntFilterSequence.Type.CONSTANT:
                        newFilter = new IntFilterSequence.ConstantFilter(statInteraction.GetFilter().GetName(), newValue);
                        break;
                    case IntFilterSequence.Type.SCALAR:
                        newFilter = new IntFilterSequence.ScalarFilter(statInteraction.GetFilter().GetName(), newValue);
                        break;
                    case IntFilterSequence.Type.CUSTOM:
                        newFilter = statInteraction.GetFilter();
                        break;
                }

            }

            // Create and return the appropriate resulting StatInteraction
            switch(statInteraction.STAT_INTERACTION_INTENT) {
                case EStatInteractionIntent.STAT_DEPLETE:
                    return new StatInteractionStatDeplete<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>(newValue, statInteraction.STAT_TO_AFFECT, statInteraction.STAT_INTERACTION_TYPE);
                case EStatInteractionIntent.STAT_GAIN:
                    return new StatInteractionStatGain<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>(newValue, statInteraction.STAT_TO_AFFECT, statInteraction.STAT_INTERACTION_TYPE);
                case EStatInteractionIntent.ADD_STAT_DEPLETE_FILTER:
                    return new StatInteractionAddStatDepleteFilter<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>(newFilter, statInteraction.STAT_TO_AFFECT, statInteraction.STAT_INTERACTION_TYPE);
                case EStatInteractionIntent.ADD_STAT_GAIN_FILTER:
                    return new StatInteractionAddStatGainFilter<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>(newFilter, statInteraction.STAT_TO_AFFECT, statInteraction.STAT_INTERACTION_TYPE);
                case EStatInteractionIntent.ADD_STAT_FILTER:
                    return new StatInteractionAddStatFilter<E_STAT_TYPE, E_STAT_INTERACTION_TYPE>(newFilter, statInteraction.STAT_TO_AFFECT, statInteraction.STAT_INTERACTION_TYPE);
                default:
                    break;
            }

            // Could not create and return the new Stat Interaction
            return null;
        }
    }
}
