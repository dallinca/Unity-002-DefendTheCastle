using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace Tests {

    /// <summary>
    /// TEST TODO
    /// Timed Filter Removal
    /// </summary>
    [TestFixture]
    public class IntFilterSequenceTest {

        [Test]
        public void Filter_Order_Default_Setting() {
            // Create new filter int
            IntFilterSequence intFS = new IntFilterSequence();

            IntFilterSequence.Order filterOrder = intFS.GetFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM, filterOrder);

        }

        [Test]
        public void Can_Set_And_Retrieve_Filter_Order() {
            // Create new filter int
            IntFilterSequence intFS = new IntFilterSequence();

            intFS.SetFilterOrder(IntFilterSequence.Order.CUSTOM);
            IntFilterSequence.Order filterOrder = intFS.GetFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CUSTOM, filterOrder);

            intFS.SetFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filterOrder = intFS.GetFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM, filterOrder);

        }

        [Test]
        public void Add_And_Remove_Constant_Filters_Works() {
            const int BASE_INT = 20;

            // Create new filter int
            IntFilterSequence intFS = new IntFilterSequence();

            // Add and test filters
            intFS.AddFilter(new IntFilterSequence.ConstantFilter("filter1", 5));
            int filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual(BASE_INT + 5, filteredInt);

            // Add and test filters
            intFS.AddFilter(new IntFilterSequence.ConstantFilter("filter2", -10));
            filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual(BASE_INT + 5 - 10, filteredInt);

            // Remove and test filters
            intFS.RemoveFilter("filter1");
            filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual(BASE_INT - 10, filteredInt);

            // Remove and test filters
            intFS.RemoveFilter("filter2");
            filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual(BASE_INT, filteredInt);
        }

        [Test]
        public void Add_And_Remove_Scalar_Filters_Works() {
            const int BASE_INT = 20;

            // Create new filter int
            IntFilterSequence intFS = new IntFilterSequence();

            // Add and test filters
            intFS.AddFilter(new IntFilterSequence.ScalarFilter("filter1", 2));
            int filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual(BASE_INT * 2, filteredInt);

            // Add and test filters
            intFS.AddFilter(new IntFilterSequence.ScalarFilter("filter2", .5f));
            filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual(BASE_INT * 2 * .5f, filteredInt);

            // Remove and test filters
            intFS.RemoveFilter("filter1");
            filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual(BASE_INT * .5f, filteredInt);

            // Remove and test filters
            intFS.RemoveFilter("filter2");
            filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual(BASE_INT, filteredInt);
        }

        [Test]
        public void Add_And_Remove_Custom_Filters_Work() {
            const int BASE_INT = 20;

            // Create new filter int
            IntFilterSequence intFS = new IntFilterSequence();

            // Add and test filters
            intFS.AddFilter(new IntFilterSequence.CustomFilter("filter1",
                (float value) => {
                    return (value + 8) / 2;
                }
            ));
            int filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual((BASE_INT + 8) / 2, filteredInt);

            // Remove and test filters
            intFS.RemoveFilter("filter1");
            filteredInt = intFS.ApplyFilters(BASE_INT);
            Assert.AreEqual(BASE_INT, filteredInt);
        }


        [Test]
        public void Can_Restrict_And_Order_Filters() {
            const int BASE_INT = 20;
            const int CONSTANT_VAL = 6;
            const int SCALAR_VAL = 10;
            IntFilterSequence.DFilter customVal = (float value) => {
                return (value + 8) / 2;
            };

            // Create new filter int
            IntFilterSequence intFS = new IntFilterSequence();

            // Add and test filters
            intFS.AddFilter(new IntFilterSequence.CustomFilter("filter3", customVal));
            int filteredInt = intFS.ApplyFilters(BASE_INT);
            int afterCustom = (int)customVal(BASE_INT);
            Assert.AreEqual(afterCustom, filteredInt);

            // Add and test filters
            intFS.AddFilter(new IntFilterSequence.ConstantFilter("filter1", CONSTANT_VAL));
            filteredInt = intFS.ApplyFilters(BASE_INT);
            int afterConstant = BASE_INT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            // Add and test filters
            intFS.AddFilter(new IntFilterSequence.ScalarFilter("filter2", SCALAR_VAL));
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterConstant = BASE_INT + CONSTANT_VAL;
            int afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test Single Restrictions -- //

            // Restrict to CONSTANT only
            intFS.SetFilterOrder(IntFilterSequence.Order.CONSTANT);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterConstant = BASE_INT + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

            // Restrict to SCALAR only
            intFS.SetFilterOrder(IntFilterSequence.Order.SCALAR);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterScalar = BASE_INT * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            // Restrict to CUSTOM only
            intFS.SetFilterOrder(IntFilterSequence.Order.CUSTOM);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterCustom = (int)customVal(BASE_INT);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test that filters are still present, just not used. -- //

            // Reset to default filtering Order
            intFS.SetFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test Double Restrictions/Orderings -- //

            intFS.SetFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            intFS.SetFilterOrder(IntFilterSequence.Order.SCALAR_CONSTANT);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterScalar = BASE_INT * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);


            intFS.SetFilterOrder(IntFilterSequence.Order.CONSTANT_CUSTOM);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            intFS.SetFilterOrder(IntFilterSequence.Order.CUSTOM_CONSTANT);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterCustom = (int)customVal(BASE_INT);
            afterConstant = afterCustom + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);


            intFS.SetFilterOrder(IntFilterSequence.Order.SCALAR_CUSTOM);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterScalar = BASE_INT * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            intFS.SetFilterOrder(IntFilterSequence.Order.CUSTOM_SCALAR);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterCustom = (int)customVal(BASE_INT);
            afterScalar = afterCustom * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            // -- Test Triple Restrictions/Orderings -- //

            intFS.SetFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            intFS.SetFilterOrder(IntFilterSequence.Order.SCALAR_CONSTANT_CUSTOM);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterScalar = BASE_INT * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            intFS.SetFilterOrder(IntFilterSequence.Order.CONSTANT_CUSTOM_SCALAR);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            afterScalar = afterCustom * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            intFS.SetFilterOrder(IntFilterSequence.Order.SCALAR_CUSTOM_CONSTANT);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterScalar = BASE_INT * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            afterConstant = afterCustom + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

            intFS.SetFilterOrder(IntFilterSequence.Order.CUSTOM_CONSTANT_SCALAR);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterCustom = (int)customVal(BASE_INT);
            afterConstant = afterCustom + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            intFS.SetFilterOrder(IntFilterSequence.Order.CUSTOM_SCALAR_CONSTANT);
            filteredInt = intFS.ApplyFilters(BASE_INT);
            afterCustom = (int)customVal(BASE_INT);
            afterScalar = afterCustom * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

        }

        [Test]
        public void Rounding_Default_Setting() {
            IntFilterSequence fSeq = new IntFilterSequence();

            IntFilterSequence.Rounding defaultRounding = fSeq.GetAfterFilterRounding();
            Assert.AreEqual(IntFilterSequence.Rounding.DOWN, defaultRounding);
            
        }

        [Test]
        public void Rounding_Options_Behave_Correctly() {
            const int BASE_INT = 1;

            IntFilterSequence fSeq = new IntFilterSequence();

            // Test the Rouding.Down Strategy
            IntFilterSequence.Rounding defaultRounding = fSeq.GetAfterFilterRounding();
            Assert.AreEqual(IntFilterSequence.Rounding.DOWN, defaultRounding);

            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar1", .1f));
            int filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(0, filteredInt);

            fSeq.RemoveFilter("scalar1");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar4", .4f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(0, filteredInt);

            fSeq.RemoveFilter("scalar4");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar5", .5f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(0, filteredInt);

            fSeq.RemoveFilter("scalar5");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar6", .6f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(0, filteredInt);

            fSeq.RemoveFilter("scalar6");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar9", .9f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(0, filteredInt);

            fSeq.RemoveFilter("scalar9");

            // Test the Rouding.Down Strategy
            fSeq.SetAfterFilterRounding(IntFilterSequence.Rounding.UP);
            IntFilterSequence.Rounding setRounding = fSeq.GetAfterFilterRounding();
            Assert.AreEqual(IntFilterSequence.Rounding.UP, setRounding);

            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar1", .1f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(1, filteredInt);

            fSeq.RemoveFilter("scalar1");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar4", .4f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(1, filteredInt);

            fSeq.RemoveFilter("scalar4");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar5", .5f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(1, filteredInt);

            fSeq.RemoveFilter("scalar5");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar6", .6f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(1, filteredInt);

            fSeq.RemoveFilter("scalar6");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar9", .9f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(1, filteredInt);

            fSeq.RemoveFilter("scalar9");

            // Test the Rouding.Down Strategy
            fSeq.SetAfterFilterRounding(IntFilterSequence.Rounding.MIDDLE);
            setRounding = fSeq.GetAfterFilterRounding();
            Assert.AreEqual(IntFilterSequence.Rounding.MIDDLE, setRounding);

            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar1", .1f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(0, filteredInt);

            fSeq.RemoveFilter("scalar1");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar4", .4f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(0, filteredInt);

            fSeq.RemoveFilter("scalar4");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar5", .5f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(1, filteredInt);

            fSeq.RemoveFilter("scalar5");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar6", .6f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(1, filteredInt);

            fSeq.RemoveFilter("scalar6");
            fSeq.AddFilter(new IntFilterSequence.ScalarFilter("scalar9", .9f));
            filteredInt = fSeq.ApplyFilters(BASE_INT);
            Assert.AreEqual(1, filteredInt);

            fSeq.RemoveFilter("scalar9");


        }
    }

}