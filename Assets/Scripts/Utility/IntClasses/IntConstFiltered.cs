using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntConstFiltered {

    // -- INSTANCE MEMBERS -- //

    private IntFilterSequence filterSequence = new IntFilterSequence();

    // Stored Integer
    public readonly int baseInt;
    private int filteredInt;

    // Observer Function
    private Action observerFunction;

    // -- CONTRUCTORS -- //

    public IntConstFiltered(int baseInt) {
        this.baseInt = baseInt;
        filteredInt = baseInt;
        filterSequence.SetObserverFunction(UpdateFilteredConstInt);
    }

    // -- INSTANCE METHODS -- //

    /// <summary>
    /// Sets a function to call every time a change is made that will affect the value of the filtered int.
    /// </summary>
    /// <param name="observerFunction"></param>
    public void SetObserverFunction(Action observerFunction) {
        this.observerFunction = observerFunction;
    }

    /// <summary>
    /// Called when any change is made to the filter sequence. Notifies the ObserverFunction
    /// when changes are made. Does not specify what changed.
    /// </summary>
    private void AlertObserverFunction() {
        observerFunction?.Invoke();
    }

    /// <summary>
    /// Gets the value of the base integer after having passed through
    /// all the filters specified by the set order.
    /// </summary>
    /// <returns>The filtered Integer</returns>
    public int GetFilteredInt() {
        return filteredInt;
    }

    /// <summary>
    /// Sets a new order of application for the filters on the base integer
    /// </summary>
    /// <param name="newOrder">The new Order of application</param>
    public void SetFilterOrder(IntFilterSequence.Order newOrder) {
        filterSequence.SetFilterOrder(newOrder);
    }

    /// <summary>
    /// Gets the currently set order to apply filters to the base integer
    /// </summary>
    /// <returns>Order of application</returns>
    public IntFilterSequence.Order GetFilterOrder() {
        return filterSequence.GetFilterOrder();
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the base integer
    /// </summary>
    /// <param name="newRounding">The new After Filters Rounding Strategy</param>
    public void SetAfterFilterRounding(IntFilterSequence.Rounding newRounding) {
        filterSequence.SetAfterFilterRounding(newRounding);
    }

    /// <summary>
    /// Gets the currently set rounding method for after all filters are applied to the base integer
    /// </summary>
    /// <returns>After Filters Rounding Strategy</returns>
    public IntFilterSequence.Rounding GetAfterFilterRounding() {
        return filterSequence.GetAfterFilterRounding();
    }

    /// <summary>
    /// Gets the base integer that the filters are applied to
    /// </summary>
    /// <returns></returns>
    public int GetBaseInt() {
        return baseInt;
    }

    /// <summary>
    /// <para>Adds a new filter to apply to the base integer</para>
    /// <para>Returns whether the filter was added.</para>
    /// </summary>
    /// <param name="filter">The filter to add</param>
    /// <returns>If the filter was added</returns>
    public bool AddFilter(IntFilterSequence.Filter filter) {
        return AddFilter(filter, 0);
    }

    /// <summary>
    /// <para>Adds a new filter to apply to the base integer</para>
    /// <para>Returns whether the filter was added.</para>
    /// </summary>
    /// <param name="filter">The filter to add</param>
    /// <param name="secondsUntilRemove">The number of seconds until the filter is automatically removed. 0 for never auto remove.</param>
    /// <returns>If the filter was added</returns>
    public bool AddFilter(IntFilterSequence.Filter filter, float secondsUntilRemove) {
        return filterSequence.AddFilter(filter, secondsUntilRemove);
    }
    
    /// <summary>
    /// <para>Removes a filter that was being applied to the base integer</para>
    /// <para>Returns whether the filter was removed.</para>
    /// </summary>
    /// <param name="name">The name of the filter</param>
    /// <returns>If the filter was removed</returns>
    public bool RemoveFilter(string name) {
        return filterSequence.RemoveFilter(name);
    }

    /// <summary>
    /// Checks if the specified filter name already exists, and therefore
    /// cannot be used for another filter.
    /// </summary>
    /// <param name="name">The name to check</param>
    /// <returns>Whether the name already exists</returns>
    public bool HasFilter(string name) {
        return filterSequence.HasFilter(name);
    }

    /// <summary>
    /// <para>Called by the filterSequence whenever a change is made that affects the filter sequence.</para>
    /// <para>Retrieves the new filteredInt value from the filterSequence</para>
    /// </summary>
    public void UpdateFilteredConstInt() {
        filteredInt = filterSequence.ApplyFilters(baseInt);
        AlertObserverFunction();
    }
}
