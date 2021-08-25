using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IntFilterSequence {

    // -- TYPE DECLARATIONS -- //

    // Format of filtering functions
    public delegate float DFilter(float fInt);

    // Type of Rounding After Filters
    public enum Rounding {
        DOWN,
        UP,
        MIDDLE
    }
    private const Rounding DEFAULT_ROUNDING = Rounding.DOWN;

    // Type of Filter
    public enum Type {
        CONSTANT,
        SCALAR,
        CUSTOM
    };

    // Order of Filter Application
    public enum Order {
        CONSTANT,
        SCALAR,
        CUSTOM,

        CONSTANT_SCALAR,
        SCALAR_CONSTANT,

        CONSTANT_CUSTOM,
        CUSTOM_CONSTANT,

        SCALAR_CUSTOM,
        CUSTOM_SCALAR,

        CONSTANT_SCALAR_CUSTOM,
        SCALAR_CONSTANT_CUSTOM,
        CONSTANT_CUSTOM_SCALAR,
        SCALAR_CUSTOM_CONSTANT,
        CUSTOM_CONSTANT_SCALAR,
        CUSTOM_SCALAR_CONSTANT

    };
    private static List<List<int>> applicationOrders = new List<List<int>> {
        new List<int> { (int)Type.CONSTANT },
        new List<int> { (int)Type.SCALAR },
        new List<int> { (int)Type.CUSTOM },

        new List<int> { (int)Type.CONSTANT, (int)Type.SCALAR },
        new List<int> { (int)Type.SCALAR, (int)Type.CONSTANT },

        new List<int> { (int)Type.CONSTANT, (int)Type.CUSTOM },
        new List<int> { (int)Type.CUSTOM, (int)Type.CONSTANT },

        new List<int> { (int)Type.SCALAR, (int)Type.CUSTOM },
        new List<int> { (int)Type.CUSTOM, (int)Type.SCALAR },

        new List<int> { (int)Type.CONSTANT, (int)Type.SCALAR, (int)Type.CUSTOM },
        new List<int> { (int)Type.SCALAR, (int)Type.CONSTANT, (int)Type.CUSTOM },
        new List<int> { (int)Type.CONSTANT, (int)Type.CUSTOM, (int)Type.SCALAR },
        new List<int> { (int)Type.SCALAR, (int)Type.CUSTOM, (int)Type.CONSTANT },
        new List<int> { (int)Type.CUSTOM, (int)Type.CONSTANT, (int)Type.SCALAR },
        new List<int> { (int)Type.CUSTOM, (int)Type.SCALAR, (int)Type.CONSTANT }
    };
    private const Order DEFAULT_ORDER = Order.CONSTANT_SCALAR_CUSTOM;

    // -- INSTANCE MEMBERS -- //

    // Stored Filters
    private Dictionary<string, Filter>[] filters = new Dictionary<string, Filter>[Enum.GetNames(typeof(Type)).Length];
    private Dictionary<string, int> filterLocation = new Dictionary<string, int>();

    // Filter application order
    private Order order = DEFAULT_ORDER;

    // After Filters Rounding
    private Rounding rounding = DEFAULT_ROUNDING;

    // Observer Function
    private Action observerFunction;

    // Utility Helper Monobehaviour
    UtilityHelperMonoBehaviour monoHelper;

    // -- CONTRUCTORS -- //

    public IntFilterSequence() {

        // Init empty dictionaries for filters storage
        for (int i = 0; i < Enum.GetNames(typeof(Type)).Length; i++) {
            filters[i] = new Dictionary<string, Filter>();
        }

        GameObject gameObject = GameObject.Find("MonoHelper");
        if (gameObject) {
            monoHelper = gameObject.GetComponent<UtilityHelperMonoBehaviour>();
        } else {
            Debug.Log("Warning: Could not find 'MonoHelper' GameObject");
        }

        if (!monoHelper) {
            Debug.Log("Warning: Could not find 'UtilityHelperMonoBehaviour' Component on 'MonoHelper' GameObject");
        }

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
    /// Sets a new order of application for the filters on the passed in integers
    /// </summary>
    /// <param name="newOrder">The new Order of application</param>
    public void SetFilterOrder(Order newOrder) {
        order = newOrder;
        AlertObserverFunction();
    }

    /// <summary>
    /// Gets the currently set order to apply filters to the passed in integers
    /// </summary>
    /// <returns>Order of application</returns>
    public Order GetFilterOrder() {
        return order;
    }

    /// <summary>
    /// Sets a new rounding method for after all filters are applied to the passed in integers
    /// </summary>
    /// <param name="newRounding">The new After Filters Rounding Strategy</param>
    public void SetAfterFilterRounding(Rounding newRounding) {
        rounding = newRounding;
        AlertObserverFunction();
    }

    /// <summary>
    /// Gets the currently set rounding method for after all filters are applied to the passed in integers
    /// </summary>
    /// <returns>After Filters Rounding Strategy</returns>
    public Rounding GetAfterFilterRounding() {
        return rounding;
    }

    /// <summary>
    /// Gets the value of the base integer after having passed through
    /// all the filters specified by the set order.
    /// </summary>
    /// <param name="IntToFilter">The Integer to apply the stored filters on</param>
    /// <returns>The filtered Integer</returns>
    public int ApplyFilters(int IntToFilter) {
        float newFilteredFloat = IntToFilter;

        // Retrieve the current order of filtering
        List<int> filterOrder = applicationOrders[(int)order];

        float scalarTotal = 1;

        // Iterate over each filter type
        foreach (var index in filterOrder) {

            // Iterate over each filter within the filter type
            foreach (KeyValuePair<string, Filter> filter in filters[index]) {
                if (index == (int)Type.SCALAR) {
                    scalarTotal = filter.Value.DoFilter(scalarTotal);
                } else {
                    newFilteredFloat = filter.Value.DoFilter(newFilteredFloat);
                }
            }

            if (index == (int)Type.SCALAR) {
                newFilteredFloat = scalarTotal * newFilteredFloat;
            }

        }

        int filteredInt = 0;

        if (Rounding.DOWN == rounding) {
            filteredInt = (int) Math.Floor(newFilteredFloat);
        } else if (Rounding.UP == rounding) {
            filteredInt = (int) Math.Ceiling(newFilteredFloat);
        } else if (Rounding.MIDDLE == rounding) {
            filteredInt = (int)Math.Round(newFilteredFloat, MidpointRounding.AwayFromZero);
        }

        // Set new FilteredInt
        return filteredInt;
    }

    /// <summary>
    /// <para>Adds a new filter to apply to the passed in integers</para>
    /// <para>Returns whether the filter was added.</para>
    /// </summary>
    /// <param name="filter">The filter to add</param>
    /// <returns>If the filter was added</returns>
    public bool AddFilter(Filter filter) {
        return AddFilter(filter, 0);
    }

    /// <summary>
    /// <para>Adds a new filter to apply to the passed in integers</para>
    /// <para>Returns whether the filter was added.</para>
    /// </summary>
    /// <param name="filter">The filter to add</param>
    /// <param name="secondsUntilRemoval">The number of seconds until the filter is automatically removed. 0 for never auto remove.</param>
    /// <returns>If the filter was added</returns>
    public bool AddFilter(Filter filter, float secondsUntilRemoval) {
        if (null == filter) {
            return false;
        }

        if (HasFilter(filter.GetName())) {
            return false;
        }

        // Add the filter to the correct Dictionary
        filters[(int)filter.GetType()].Add(filter.GetName(), filter);

        // Add the filter to the registry
        filterLocation.Add(filter.GetName(), (int)filter.GetType());

        // Start process to remove filter if necessary
        if (secondsUntilRemoval > 0 && monoHelper) {
            monoHelper.StartCoroutineWaitToRunFunction(secondsUntilRemoval, RemoveFilter, filter.GetName());
        }

        AlertObserverFunction();
        return true;
    }

    /// <summary>
    /// <para>Removes a filter that was being applied to the passed in integers</para>
    /// <para>Returns whether the filter was removed.</para>
    /// </summary>
    /// <param name="name">The name of the filter</param>
    /// <returns>If the filter was removed</returns>
    public bool RemoveFilter(string name) {
        // Return if filter does not exist
        if (!HasFilter(name)) {
            return false;
        }

        // find which dictionary the filter is in
        int location = -1;
        if (!filterLocation.TryGetValue(name, out location)) {
            return false;
        }

        // Remove the filter from the dictionary
        filters[location].Remove(name);

        // Remove the registration of the filter
        filterLocation.Remove(name);

        AlertObserverFunction();
        return true;
    }

    /// <summary>
    /// Checks if the specified filter name already exists, and therefore
    /// cannot be used for another filter.
    /// </summary>
    /// <param name="name">The name to check</param>
    /// <returns>Whether the name already exists</returns>
    public bool HasFilter(string name) {
        if (filterLocation.ContainsKey(name)) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Base Helper class to IntFilterSequence. 
    /// The acting filters on IntFilterSequence.
    /// </summary>
    public class Filter {

        protected string name;
        protected float value;
        protected Type type;
        protected DFilter filter;

        // Nobody should instantiate this class directly
        protected Filter() { }

        /// <summary>
        /// Gets the value currently set for the filter
        /// </summary>
        /// <returns>The Unique Identifier of the filter</returns>
        public string GetName() {
            return name;
        }

        /// <summary>
        /// Gets the value currently set for the filter
        /// </summary>
        /// <returns>Value of change to the filtered Values</returns>
        public float GetValue() {
            return value;
        }

        /// <summary>
        /// Gets the value currently set for the filter
        /// </summary>
        /// <returns>The Type of filter this object is</returns>
        public Type GetType() {
            return type;
        }

        /// <summary>
        /// Perform the filtering on a given value
        /// </summary>
        /// <param name="valueToFilter"></param>
        /// <returns>The value post-filter</returns>
        public float DoFilter(float valueToFilter) {
            if (null != filter) {
                return filter(valueToFilter);
            }

            return valueToFilter;
        }
    }

    public class ConstantFilter : Filter {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Unique Identifier for the filter</param>
        /// <param name="value">Value of change to the filtered Values</param>
        public ConstantFilter(string name, int value) {
            // Given values
            this.name = name;
            this.value = value;

            // Implied Values
            type = Type.CONSTANT;
            filter = (float valueToFilter) => { return valueToFilter + value; };
        }

    }

    public class ScalarFilter : Filter {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Unique Identifier for the filter</param>
        /// <param name="value">Value of change to the filtered Values</param>
        public ScalarFilter(string name, float value) {
            // Given Values
            this.name = name;
            this.value = value;

            // Implied Values
            type = Type.SCALAR;
            filter = (float valueToFilter) => { return valueToFilter * value; };
        }

    }

    public class CustomFilter : Filter {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Unique Identifier for the filter</param>
        /// <param name="value">Value of change to the filtered Values</param>
        public CustomFilter(string name, DFilter filter) {
            // Given Values
            this.name = name;

            // Implied Values
            type = Type.CUSTOM;
            this.filter = filter;
        }

    }
}
