using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSequenceByInteractionType<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> : Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>
    where E_STAT_TYPES : Enum
    where E_STAT_INTERACTION_TYPES : Enum {

    // private IntFilterSequence stat;
    private Dictionary<E_STAT_INTERACTION_TYPES, IntFilterSequence> stat = new Dictionary<E_STAT_INTERACTION_TYPES, IntFilterSequence>();

    // Stat Observer Function
    private Action statObserverFunction;


    // -- CONSTRUCTOR -- //

    public StatSequenceByInteractionType() : base(Classes.SEQUENCE_BY_INTERACTION_TYPE) {

        this.asStatSequenceByInteractionType = this;

        // Fill in a blank FilterSequence for each type.
        foreach (E_STAT_INTERACTION_TYPES type in Enum.GetValues(typeof(E_STAT_INTERACTION_TYPES))) {
            stat.Add(type, new IntFilterSequence());
        }

    }

    // -- INSTANCE METHODS -- //

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
    /// Gets the value of the stat base after having passed through
    /// all the stat filters specified by the set stat filter order.
    /// </summary>
    /// <returns>The Stat base after filtering</returns>
    public int GetStatFiltered(int valueToFilter, E_STAT_INTERACTION_TYPES interactionType) {
        return stat[interactionType].ApplyFilters(valueToFilter);
    }

    /// <summary>
    /// Gets the currently set stat filter order
    /// </summary>
    /// <returns>stat filter Order of application</returns>
    public IntFilterSequence.Order GetStatFilterOrder(E_STAT_INTERACTION_TYPES interactionType) {
        return stat[interactionType].GetFilterOrder();
    }

    /// <summary>
    /// Sets a new stat filter order
    /// </summary>
    /// <param name="newOrder">The new stat filter Order of application</param>
    public void SetStatFilterOrder(IntFilterSequence.Order newOrder, E_STAT_INTERACTION_TYPES interactionType) {
        stat[interactionType].SetFilterOrder(newOrder);
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the base stat
    /// </summary>
    /// <param name="newRounding">The new After Filters Rounding Strategy</param>
    public void SetAfterFilterRounding(IntFilterSequence.Rounding newRounding, E_STAT_INTERACTION_TYPES interactionType) {
        stat[interactionType].SetAfterFilterRounding(newRounding);
    }

    /// <summary>
    /// Gets the currently set rounding method for after all filters are applied to the base stat
    /// </summary>
    /// <returns>After Filters Rounding Strategy</returns>
    public IntFilterSequence.Rounding GetAfterFilterRounding(E_STAT_INTERACTION_TYPES interactionType) {
        return stat[interactionType].GetAfterFilterRounding();
    }

    /// <summary>
    /// <para>Adds a new stat filter to apply to the stat base</para>
    /// <para>Returns whether the stat filter was added.</para>
    /// </summary>
    /// <param name="filter">The stat filter to add</param>
    /// <returns>If the stat filter was added</returns>
    public bool AddStatFilter(IntFilterSequence.Filter filter, E_STAT_INTERACTION_TYPES interactionType) {
        return AddStatFilter(filter, 0, interactionType);
    }

    /// <summary>
    /// <para>Adds a new stat filter to apply to the stat base</para>
    /// <para>Returns whether the stat filter was added.</para>
    /// </summary>
    /// <param name="filter">The stat filter to add</param>
    /// <param name="secondsUntilRemove">The number of seconds until the filter is automatically removed. 0 for never auto remove.</param>
    /// <returns>If the stat filter was added</returns>
    public bool AddStatFilter(IntFilterSequence.Filter filter, float secondsUntilRemove, E_STAT_INTERACTION_TYPES interactionType) {
        return stat[interactionType].AddFilter(filter, secondsUntilRemove);
    }

    /// <summary>
    /// <para>Removes a stat filter that was being applied to the stat base</para>
    /// <para>Returns whether the stat filter was removed.</para>
    /// </summary>
    /// <param name="name">The name of the stat filter</param>
    /// <returns>If the stat filter was removed</returns>
    public bool RemoveStatFilter(string name, E_STAT_INTERACTION_TYPES interactionType) {
        return stat[interactionType].RemoveFilter(name);
    }

    /// <summary>
    /// Checks if the specified stat filter name already exists, and therefore
    /// cannot be used for another stat filter.
    /// </summary>
    /// <param name="name">The name to check</param>
    /// <returns>Whether the name already exists</returns>
    public bool HasStatFilter(string name, E_STAT_INTERACTION_TYPES interactionType) {
        return stat[interactionType].HasFilter(name);
    }

    /// <summary>
    /// <para>Called by the IntFiltered whenever a change is made that affects the filter sequence.</para>
    /// <para>Retrieves the new filteredInt value from the filterSequence</para>
    /// </summary>
    public void StatWasUpdated() {
        AlertStatObserverFunction();
    }

}
