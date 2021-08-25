using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatReference<E_STAT_TYPES, E_STAT_INTERACTION_TYPES> : Stat<E_STAT_TYPES, E_STAT_INTERACTION_TYPES>
    where E_STAT_TYPES : Enum
    where E_STAT_INTERACTION_TYPES : Enum
{
    private IntFiltered stat;

    // Stat Observer Function
    private Action statObserverFunction;

    // -- CONSTRUCTOR -- //

    public StatReference(int statBase) : base(Classes.REFERENCE) {
        stat = new IntFiltered(statBase);
        stat.SetObserverFunction(StatWasUpdated);

        this.asStatReference = this;
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
    public void SetAfterFilterRounding(IntFilterSequence.Rounding newRounding) {
        stat.SetAfterFilterRounding(newRounding);
    }

    /// <summary>
    /// Gets the currently set rounding method for after all filters are applied to the base stat
    /// </summary>
    /// <returns>After Filters Rounding Strategy</returns>
    public IntFilterSequence.Rounding GetAfterFilterRounding() {
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
    /// <para>Called by the IntFiltered whenever a change is made that affects the filter sequence.</para>
    /// <para>Retrieves the new filteredInt value from the filterSequence</para>
    /// </summary>
    public void StatWasUpdated() {
        AlertStatObserverFunction();
    }

}


// TODO, remove class
public class StatReference : StatReference<EDefaultStatType, EDefaultStatInteractionType> {

    // -- CONSTRUCTOR -- //

    public StatReference(int statBase) : base(statBase) {    }
}