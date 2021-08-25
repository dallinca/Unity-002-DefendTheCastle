using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO - ADD Recurring depletion - with optional TimeToRemoveEffect
// TODO - ADD Recurring gain - with optional TimeToRemoveEffect
// TODO - ADD Object with multiple Depletions and Gains to be entered with one function call

/// <summary>
/// 
/// </summary>
/// <typeparam name="E_STAT_INTERACTION_TYPES">The Interaction Types for Depletion</typeparam>
public class StatDepletion<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> : Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>
    where E_STAT_INTERACTION_TYPES : Enum
    where E_STAT_TYPES : Enum {

    // Base Stat
    private IntFiltered stat;

    // Depletion Rules
    private int totalDepletion;
    private bool canDepletePastZero;
    private bool canGainPastCeiling;

    // Gain and Depletion Filter Sequences
    private E_STAT_INTERACTION_TYPES depletionInteractionType;
    private IntFilterSequence[] depletionFilterSequences;
    
    private E_STAT_INTERACTION_TYPES gainInteractionType;
    private IntFilterSequence[] gainFilterSequences;

    // Recurring Gains and Depletions
    // Dictionary<name, Tuple<valueOfGain, frequency>>
    //private Dictionary<string, Tuple<int, float>> recurringGains;

    // Stat Observer Function
    private Action statObserverFunction;

    // -- CONSTRUCTOR -- //

    public StatDepletion(int statBase) : base(Classes.DEPLETION) {
        InitFromConstructor(statBase, false, false);

        asStatDepletion = this;
    }

    public StatDepletion(int statBase, bool canDepletePastZero, bool canGainPastCeiling) : base(Classes.DEPLETION) {
        InitFromConstructor(statBase, canDepletePastZero, canGainPastCeiling);

        asStatDepletion = this;
    }

    private void InitFromConstructor(int statBase, bool canDepletePastZero, bool canGainPastCeiling) {
        // Stat
        stat = new IntFiltered(statBase);

        // Depletion General
        totalDepletion = 0;
        this.canDepletePastZero = canDepletePastZero;
        this.canGainPastCeiling = canGainPastCeiling;

        // Depletion
        depletionFilterSequences = new IntFilterSequence[Enum.GetValues(this.depletionInteractionType.GetType()).Length];

        for (int i = 0; i < depletionFilterSequences.Length; i++) {
            depletionFilterSequences[i] = new IntFilterSequence();
        }

        // Gain
        gainFilterSequences = new IntFilterSequence[Enum.GetValues(this.gainInteractionType.GetType()).Length];

        for (int i = 0; i < gainFilterSequences.Length; i++) {
            gainFilterSequences[i] = new IntFilterSequence();
        }

        // Observe Relevant Changes.
        stat.SetObserverFunction(StatWasUpdated);

    }

    // -- INSTANCE METHODS - Depletion -- //

    public bool GetCanDepletePastZeroSetting() {
        return canDepletePastZero;
    }

    /// <summary>
    /// Can be called to manually set the current depletion amount of the character. Cannot be less than 0 or more than the current filteredStat value.
    /// </summary>
    /// <param name="newDepletionAmount"></param>
    public void ResetDepletionAmount(int newDepletionAmount) {
        if (newDepletionAmount < 0) {
            newDepletionAmount = 0;
        } else if (newDepletionAmount > stat.GetFilteredInt()) {
            newDepletionAmount = stat.GetFilteredInt();
        }

        totalDepletion = newDepletionAmount;
    }

    /// <summary>
    /// Called internally to ensure the Depletion amount is within the desired bounds.
    /// </summary>
    private void ClampDepletionAmount() {
        // Check Lower Bound
        if (!canDepletePastZero && totalDepletion > stat.GetFilteredInt()) {
            totalDepletion = stat.GetFilteredInt();
        }

        // Check Upper Bound
        if (!canGainPastCeiling && totalDepletion < 0) {
            totalDepletion = 0;
        }
    }

    private E_STAT_INTERACTION_TYPES GetDefaultDepletionInteractionTypeValue() {
        return (E_STAT_INTERACTION_TYPES)Enum.Parse(typeof(E_STAT_INTERACTION_TYPES), 0.ToString());
    }

    /// <summary>
    /// Sets a new order of application for the depletion filters on the passed in depletion values
    /// </summary>
    /// <param name="newOrder">The new Order of application</param>
    public void SetDepletionFilterOrder(IntFilterSequence.Order newOrder) {
        SetDepletionFilterOrder(newOrder, GetDefaultDepletionInteractionTypeValue());
    }

    /// <summary>
    /// Sets a new order of application for the depletion filters on the passed in depletion values
    /// </summary>
    /// <param name="newOrder">The new Order of application</param>
    /// <param name="depletionInteractionType">The Specific Depletion Interaction Type</param>
    public void SetDepletionFilterOrder(IntFilterSequence.Order newOrder, E_STAT_INTERACTION_TYPES depletionInteractionType) {
        depletionFilterSequences[Convert.ToInt32(depletionInteractionType)].SetFilterOrder(newOrder);
    }

    /// <summary>
    /// Gets the currently set order to apply depletion filters to the passed in depletion values
    /// </summary>
    /// <returns>Order of application</returns>
    public IntFilterSequence.Order GetDepletionFilterOrder() {
        return GetDepletionFilterOrder();
    }

    /// <summary>
    /// Gets the currently set order to apply depletion filters to the passed in depletion values
    /// </summary>
    /// <param name="depletionInteractionType">The Specific Depletion Interaction Type</param>
    /// <returns>Order of application</returns>
    public IntFilterSequence.Order GetDepletionFilterOrder(E_STAT_INTERACTION_TYPES depletionInteractionType) {
        return depletionFilterSequences[Convert.ToInt32(depletionInteractionType)].GetFilterOrder();
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the passed in depletion values
    /// </summary>
    /// <param name="newRounding">The new Depletion After Filters Rounding Strategy</param>
    public void SetDepletionAfterFilterRounding(IntFilterSequence.Rounding newRounding) {
        SetDepletionAfterFilterRounding(newRounding, GetDefaultDepletionInteractionTypeValue());
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the passed in depletion values
    /// </summary>
    /// <param name="newRounding">The new Depletion After Filters Rounding Strategy</param>
    /// <param name="AllInteractionTypes">If all Interaction Type Filter Lists should be updated</param>
    public void SetDepletionAfterFilterRounding(IntFilterSequence.Rounding newRounding, bool AllInteractionTypes) {
        if (!AllInteractionTypes) {
            SetDepletionAfterFilterRounding(newRounding, GetDefaultDepletionInteractionTypeValue());
        } else {
            foreach (var filterSequence in depletionFilterSequences) {
                filterSequence.SetAfterFilterRounding(newRounding);
            }
        }
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the passed in depletion values
    /// </summary>
    /// <param name="newRounding">The new Depletion After Filters Rounding Strategy</param>
    public void SetDepletionAfterFilterRounding(IntFilterSequence.Rounding newRounding, E_STAT_INTERACTION_TYPES depletionInteractionType) {
        depletionFilterSequences[Convert.ToInt32(depletionInteractionType)].SetAfterFilterRounding(newRounding);
    }

    /// <summary>
    /// Gets the currently set rounding method for after all filters are applied to the passed in depletion values
    /// </summary>
    /// <returns>Depletion After Filters Rounding Strategy</returns>
    public IntFilterSequence.Rounding GetDepletionAfterFilterRounding() {
        return GetDepletionAfterFilterRounding(GetDefaultDepletionInteractionTypeValue());
    }

    /// <summary>
    /// Gets the currently set rounding method for after all filters are applied to the passed in depletion values
    /// </summary>
    /// <returns>Depletion After Filters Rounding Strategy</returns>
    public IntFilterSequence.Rounding GetDepletionAfterFilterRounding(E_STAT_INTERACTION_TYPES depletionInteractionType) {
        return depletionFilterSequences[Convert.ToInt32(depletionInteractionType)].GetAfterFilterRounding();
    }

    /// <summary>
    /// <para>Adds a new depletion filter to apply to the passed in depletion values</para>
    /// <para>Returns whether the depletion filter was added.</para>
    /// </summary>
    /// <param name="filter">The depletion filter to add</param>
    /// <returns>If the depletion filter was added</returns>
    public bool AddDepletionFilter(IntFilterSequence.Filter filter) {
        return AddDepletionFilter(filter, GetDefaultDepletionInteractionTypeValue());
    }

    /// <summary>
    /// <para>Adds a new depletion filter to apply to the passed in depletion values</para>
    /// <para>Returns whether the depletion filter was added.</para>
    /// </summary>
    /// <param name="filter">The depletion filter to add</param>
    /// <param name="depletionInteractionType">The Specific Depletion Interaction Type</param>
    /// <returns>If the depletion filter was added</returns>
    public bool AddDepletionFilter(IntFilterSequence.Filter filter, E_STAT_INTERACTION_TYPES depletionInteractionType) {
        return depletionFilterSequences[Convert.ToInt32(depletionInteractionType)].AddFilter(filter);
    }

    /// <summary>
    /// <para>Removes a depletion filter that was being applied to the passed in depletion values</para>
    /// <para>Returns whether the depletion filter was removed.</para>
    /// </summary>
    /// <param name="name">The name of the depletion filter</param>
    /// <returns>If the depletion filter was removed</returns>
    public bool RemoveDepletionFilter(string name) {
        return RemoveDepletionFilter(name, GetDefaultDepletionInteractionTypeValue());
    }

    /// <summary>
    /// <para>Removes a depletion filter that was being applied to the passed in depletion values</para>
    /// <para>Returns whether the depletion filter was removed.</para>
    /// </summary>
    /// <param name="name">The name of the depletion filter</param>
    /// <param name="depletionInteractionType">The Specific Depletion Interaction Type</param>
    /// <returns>If the depletion filter was removed</returns>
    public bool RemoveDepletionFilter(string name, E_STAT_INTERACTION_TYPES depletionInteractionType) {
        return depletionFilterSequences[Convert.ToInt32(depletionInteractionType)].RemoveFilter(name);
    }

    /// <summary>
    /// Checks if the specified depletion filter name already exists, and therefore
    /// cannot be used for another depletion filter.
    /// </summary>
    /// <param name="name">The name to check</param>
    /// <returns>Whether the name already exists</returns>
    public bool HasDepletionFilter(string name) {
        return HasDepletionFilter(name, GetDefaultDepletionInteractionTypeValue());
    }

    /// <summary>
    /// Checks if the specified depletion filter name already exists, and therefore
    /// cannot be used for another depletion filter.
    /// </summary>
    /// <param name="name">The name to check</param>
    /// <param name="depletionInteractionType">The Specific Depletion Interaction Type</param>
    /// <returns>Whether the name already exists</returns>
    public bool HasDepletionFilter(string name, E_STAT_INTERACTION_TYPES depletionInteractionType) {
        return depletionFilterSequences[Convert.ToInt32(depletionInteractionType)].HasFilter(name);
    }

    /// <summary>
    /// <para>Depletes the stat by the specified depletionValue. The depletion value will first have all the depletionFilters applied.</para>
    /// <para>Return the new value of the stat</para>
    /// </summary>
    /// <param name="depletionValue">Amount to deplete by</param>
    /// <returns>The new value of the stat</returns>
    public int DepleteStat(int depletionValue) {
        return DepleteStat(depletionValue, GetDefaultDepletionInteractionTypeValue());
    }

    /// <summary>
    /// <para>Depletes the stat by the specified depletionValue. The depletion value will first have all the depletionFilters applied.</para>
    /// <para>Return the new value of the stat</para>
    /// </summary>
    /// <param name="depletionValue">Amount to deplete by</param>
    /// <param name="depletionInteractionType">The Specific Depletion Interaction Type</param>
    /// <returns>The new value of the stat</returns>
    public int DepleteStat(int depletionValue, E_STAT_INTERACTION_TYPES depletionInteractionType) {
        // Get filtered Depletion Value
        int depletionValueAfterFilters = depletionFilterSequences[Convert.ToInt32(depletionInteractionType)].ApplyFilters(depletionValue);

        // Keep Depletions Positive
        if (depletionValueAfterFilters < 0) {
            depletionValueAfterFilters = 0;
        }

        // update total depletion
        totalDepletion += depletionValueAfterFilters;

        ClampDepletionAmount();

        int remainingStatValue = GetRemainingStatValue();

        StatWasUpdated();
        return remainingStatValue;
    }

    // -- INSTANCE METHODS - Gain -- //

    public bool GetCanGainPastCeilingSetting() {
        return canGainPastCeiling;
    }

    private E_STAT_INTERACTION_TYPES GetDefaultGainInteractionTypeValue() {
        return (E_STAT_INTERACTION_TYPES)Enum.Parse(typeof(E_STAT_INTERACTION_TYPES), 0.ToString());
    }

    /// <summary>
    /// Sets a new order of application for the gain filters on the passed in gain values
    /// </summary>
    /// <param name="newOrder">The new Order of application</param>
    public void SetGainFilterOrder(IntFilterSequence.Order newOrder) {
        SetGainFilterOrder(newOrder, GetDefaultGainInteractionTypeValue());
    }

    /// <summary>
    /// Sets a new order of application for the gain filters on the passed in gain values
    /// </summary>
    /// <param name="newOrder">The new Order of application</param>
    /// <param name="gainInteractionType">The Specific Gain Interaction Type</param>
    public void SetGainFilterOrder(IntFilterSequence.Order newOrder, E_STAT_INTERACTION_TYPES gainInteractionType) {
        gainFilterSequences[Convert.ToInt32(gainInteractionType)].SetFilterOrder(newOrder);
    }

    /// <summary>
    /// Gets the currently set order to apply gain filters to the passed in gain values
    /// </summary>
    /// <returns>Order of application</returns>
    public IntFilterSequence.Order GetGainFilterOrder() {
        return GetGainFilterOrder(GetDefaultGainInteractionTypeValue());
    }

    /// <summary>
    /// Gets the currently set order to apply gain filters to the passed in gain values
    /// </summary>
    /// <param name="gainInteractionType">The Specific Gain Interaction Type</param>
    /// <returns>Order of application</returns>
    public IntFilterSequence.Order GetGainFilterOrder(E_STAT_INTERACTION_TYPES gainInteractionType) {
        return gainFilterSequences[Convert.ToInt32(gainInteractionType)].GetFilterOrder();
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the passed in gain values
    /// </summary>
    /// <param name="newRounding">The new Gain After Filters Rounding Strategy</param>
    public void SetGainAfterFilterRounding(IntFilterSequence.Rounding newRounding) {
        SetGainAfterFilterRounding(newRounding, GetDefaultGainInteractionTypeValue());
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the passed in gain values
    /// </summary>
    /// <param name="newRounding">The new Gain After Filters Rounding Strategy</param>
    /// <param name="AllInteractionTypes">If all Interaction Type Filter Lists should be updated</param>
    public void SetGainAfterFilterRounding(IntFilterSequence.Rounding newRounding, bool AllInteractionTypes) {
        if (!AllInteractionTypes) {
            SetGainAfterFilterRounding(newRounding, GetDefaultGainInteractionTypeValue());
        } else {
            foreach (var filterSequence in gainFilterSequences) {
                filterSequence.SetAfterFilterRounding(newRounding);
            }
        }
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the passed in gain values
    /// </summary>
    /// <param name="newRounding">The new Gain After Filters Rounding Strategy</param>
    public void SetGainAfterFilterRounding(IntFilterSequence.Rounding newRounding, E_STAT_INTERACTION_TYPES gainInteractionType) {
        gainFilterSequences[Convert.ToInt32(gainInteractionType)].SetAfterFilterRounding(newRounding);
    }

    /// <summary>
    /// Gets the currently set rounding method for after all filters are applied to the passed in gain values
    /// </summary>
    /// <returns>Gain After Filters Rounding Strategy</returns>
    public IntFilterSequence.Rounding GetGainAfterFilterRounding() {
        return GetGainAfterFilterRounding(GetDefaultGainInteractionTypeValue());
    }

    /// <summary>
    /// Gets the currently set rounding method for after all filters are applied to the passed in gain values
    /// </summary>
    /// <returns>Gain After Filters Rounding Strategy</returns>
    public IntFilterSequence.Rounding GetGainAfterFilterRounding(E_STAT_INTERACTION_TYPES gainInteractionType) {
        return gainFilterSequences[Convert.ToInt32(gainInteractionType)].GetAfterFilterRounding();
    }

    /// <summary>
    /// <para>Adds a new gain filter to apply to the passed in gain values</para>
    /// <para>Returns whether the gain filter was added.</para>
    /// </summary>
    /// <param name="filter">The gain filter to add</param>
    /// <returns>If the gain filter was added</returns>
    public bool AddGainFilter(IntFilterSequence.Filter filter) {
        return AddGainFilter(filter, GetDefaultGainInteractionTypeValue());
    }

    /// <summary>
    /// <para>Adds a new gain filter to apply to the passed in gain values</para>
    /// <para>Returns whether the gain filter was added.</para>
    /// </summary>
    /// <param name="filter">The gain filter to add</param>
    /// <param name="gainInteractionType">The Specific Gain Interaction Type</param>
    /// <returns>If the gain filter was added</returns>
    public bool AddGainFilter(IntFilterSequence.Filter filter, E_STAT_INTERACTION_TYPES gainInteractionType) {
        return gainFilterSequences[Convert.ToInt32(gainInteractionType)].AddFilter(filter);
    }

    /// <summary>
    /// <para>Removes a gain filter that was being applied to the passed in gain values</para>
    /// <para>Returns whether the gain filter was removed.</para>
    /// </summary>
    /// <param name="name">The name of the gain filter</param>
    /// <returns>If the v filter was removed</returns>
    public bool RemoveGainFilter(string name) {
        return RemoveGainFilter(name, GetDefaultGainInteractionTypeValue());
    }

    /// <summary>
    /// <para>Removes a gain filter that was being applied to the passed in gain values</para>
    /// <para>Returns whether the gain filter was removed.</para>
    /// </summary>
    /// <param name="name">The name of the gain filter</param>
    /// <param name="gainInteractionType">The Specific Gain Interaction Type</param>
    /// <returns>If the v filter was removed</returns>
    public bool RemoveGainFilter(string name, E_STAT_INTERACTION_TYPES gainInteractionType) {
        return gainFilterSequences[Convert.ToInt32(gainInteractionType)].RemoveFilter(name);
    }

    /// <summary>
    /// Checks if the specified gain filter name already exists, and therefore
    /// cannot be used for another gain filter.
    /// </summary>
    /// <param name="name">The name to check</param>
    /// <returns>Whether the name already exists</returns>
    public bool HasGainFilter(string name) {
        return HasGainFilter(name, GetDefaultGainInteractionTypeValue());
    }

    /// <summary>
    /// Checks if the specified gain filter name already exists, and therefore
    /// cannot be used for another gain filter.
    /// </summary>
    /// <param name="name">The name to check</param>
    /// <param name="gainInteractionType">The Specific Gain Interaction Type</param>
    /// <returns>Whether the name already exists</returns>
    public bool HasGainFilter(string name, E_STAT_INTERACTION_TYPES gainInteractionType) {
        return gainFilterSequences[Convert.ToInt32(gainInteractionType)].HasFilter(name);
    }

    /// <summary>
    /// <para>Gains the stat by the specified gainValue. The gain value will first have all the gainFilters applied.</para>
    /// <para>Return the new value of the stat</para>
    /// </summary>
    /// <param name="depletionValue">Amount to gain by</param>
    /// <returns>The new value of the stat</returns>
    public int GainStat(int gainValue) {
        return GainStat(gainValue, GetDefaultGainInteractionTypeValue());
    }

    /// <summary>
    /// <para>Gains the stat by the specified gainValue. The gain value will first have all the gainFilters applied.</para>
    /// <para>Return the new value of the stat</para>
    /// </summary>
    /// <param name="depletionValue">Amount to gain by</param>
    /// <param name="gainInteractionType">The Specific Gain Interaction Type</param>
    /// <returns>The new value of the stat</returns>
    public int GainStat(int gainValue, E_STAT_INTERACTION_TYPES gainInteractionType) {
        // Get filtered gain Value
        int gainValueAfterFilters = gainFilterSequences[Convert.ToInt32(gainInteractionType)].ApplyFilters(gainValue);

        // Keep Gains Positive
        if (gainValueAfterFilters < 0) {
            gainValueAfterFilters = 0;
        }

        // update total depletion
        totalDepletion -= gainValueAfterFilters;

        ClampDepletionAmount();

        int remainingStatValue = GetRemainingStatValue();

        StatWasUpdated();
        return remainingStatValue;
    }


    // -- INSTANCE METHODS - Stat -- //

    /// <summary>
    /// Get the remaining value of the stat.
    /// </summary>
    /// <returns>The remaining value</returns>
    public int GetRemainingStatValue() {
        int remainingStatValue = stat.GetFilteredInt() - totalDepletion;

        if (!canDepletePastZero && remainingStatValue < 0) {
            remainingStatValue = 0;
        }

        return remainingStatValue;
    }

    /// <summary>
    /// Gets the stat base that the filters will be applied to
    /// </summary>
    /// <returns></returns>
    public int GetStatBase() {
        return stat.GetBaseInt();
    }

    /// <summary>
    /// <para>Sets the new Stat Base Integer.</para>
    /// <para>Returns the new value of the Stat Filtered.</para>
    /// </summary>
    /// <param name="newBaseStat"></param>
    /// <returns>The new value of the Filtered Stat</returns>
    public int SetStatBase(int newBaseStat) {
        return stat.SetBaseInt(newBaseStat);
    }

    /// <summary>
    /// Gets the value of the stat base after having passed through
    /// all the stat filters specified by the set stat filter order.
    /// </summary>
    /// <returns>The Stat base after filtering</returns>
    public int GetStatFiltered() {
        return stat.GetFilteredInt();
    }

    /// <summary>
    /// Gets the currently set stat filter order
    /// </summary>
    /// <returns>stat filter Order of application</returns>
    public IntFilterSequence.Order GetStatFilterOrder() {
        return stat.GetFilterOrder();
    }

    /// <summary>
    /// Sets a new stat filter order
    /// </summary>
    /// <param name="newOrder">The new stat filter Order of application</param>
    public void SetStatFilterOrder(IntFilterSequence.Order newOrder) {
        stat.SetFilterOrder(newOrder);
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the base stat
    /// </summary>
    /// <param name="newRounding">The new After Filters Rounding Strategy</param>
    public void SetStatAfterFilterRounding(IntFilterSequence.Rounding newRounding) {
        stat.SetAfterFilterRounding(newRounding);
    }

    /// <summary>
    /// Gets the currently set rounding method for after all filters are applied to the base stat
    /// </summary>
    /// <returns>After Filters Rounding Strategy</returns>
    public IntFilterSequence.Rounding GetStatAfterFilterRounding() {
        return stat.GetAfterFilterRounding();
    }

    /// <summary>
    /// <para>Adds a new stat filter to apply to the stat base</para>
    /// <para>Returns whether the stat filter was added.</para>
    /// </summary>
    /// <param name="filter">The stat filter to add</param>
    /// <returns>If the stat filter was added</returns>
    public bool AddStatFilter(IntFilterSequence.Filter filter) {
        return AddStatFilter(filter, 0);
    }

    /// <summary>
    /// <para>Adds a new stat filter to apply to the stat base</para>
    /// <para>Returns whether the stat filter was added.</para>
    /// </summary>
    /// <param name="filter">The stat filter to add</param>
    /// <param name="secondsUntilRemove">The number of seconds until the filter is automatically removed. 0 for never auto remove.</param>
    /// <returns>If the stat filter was added</returns>
    public bool AddStatFilter(IntFilterSequence.Filter filter, float secondsUntilRemove) {
        return stat.AddFilter(filter, secondsUntilRemove);
    }

    /// <summary>
    /// <para>Removes a stat filter that was being applied to the stat base</para>
    /// <para>Returns whether the stat filter was removed.</para>
    /// </summary>
    /// <param name="name">The name of the stat filter</param>
    /// <returns>If the stat filter was removed</returns>
    public bool RemoveStatFilter(string name) {
        return stat.RemoveFilter(name);
    }

    /// <summary>
    /// Checks if the specified stat filter name already exists, and therefore
    /// cannot be used for another stat filter.
    /// </summary>
    /// <param name="name">The name to check</param>
    /// <returns>Whether the name already exists</returns>
    public bool HasStatFilter(string name) {
        return stat.HasFilter(name);
    }

    /// <summary>
    /// Sets a function to call every time a change is made that will affect the value of the stat int.
    /// </summary>
    /// <param name="observerFunction"></param>
    public void SetStatObserverFunction(Action statObserverFunction) {
        this.statObserverFunction = statObserverFunction;
    }

    /// <summary>
    /// Called when any change is made to the filter sequence of the stat. Notifies the ObserverFunction
    /// when changes are made. Does not specify what changed.
    /// </summary>
    private void AlertStatObserverFunction() {
        statObserverFunction?.Invoke();
    }

    /// <summary>
    /// <para>Called by the IntFiltered whenever a change is made that affects the filter sequence.</para>
    /// <para>Retrieves the new filteredInt value from the filterSequence</para>
    /// </summary>
    public void StatWasUpdated() {
        AlertStatObserverFunction();
    }

}
