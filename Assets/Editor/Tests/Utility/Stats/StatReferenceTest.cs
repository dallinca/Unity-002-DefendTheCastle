using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace Tests {

    /// <summary>
    /// TEST TODO
    /// Rounding
    /// Timed Filter Removal
    /// </summary>
    [TestFixture]
    public class StatReferenceTest {

        [Test]
        public void Base_Stat_Remains_Retrievable() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatReference statRef = new StatReference(BASE_STAT);

            // Retrieve the baseInt
            int baseStat = statRef.GetStatBase();

            // Compare
            Assert.AreEqual(BASE_STAT, baseStat);

        }

        [Test]
        public void Base_Stat_Changeable() {
            const int BASE_STAT = 20;
            const int NEW_BASE_STAT = 30;

            // Create new filter int
            StatReference statRef = new StatReference(BASE_STAT);
            int baseStat = statRef.GetStatBase();
            Assert.AreEqual(BASE_STAT, baseStat);

            // Change the filter Base Int and Test
            statRef.SetStatBase(NEW_BASE_STAT);
            baseStat = statRef.GetStatBase();
            Assert.AreEqual(NEW_BASE_STAT, baseStat);

        }

        [Test]
        public void Filtered_Stat_Starts_Same_As_Base_Stat() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatReference statRef = new StatReference(BASE_STAT);

            // Retrieve the filteredInt
            int filteredStat = statRef.GetStatFiltered();

            // Compare
            Assert.AreEqual(BASE_STAT, filteredStat);

        }

        [Test]
        public void Filter_Order_Default_Setting() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatReference statRef = new StatReference(BASE_STAT);

            IntFilterSequence.Order filterOrder = statRef.GetStatFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM, filterOrder);

        }

        [Test]
        public void Can_Set_And_Retrieve_Stat_Filter_Order() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatReference statRef = new StatReference(BASE_STAT);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM);
            IntFilterSequence.Order statFilterOrder = statRef.GetStatFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CUSTOM, statFilterOrder);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            statFilterOrder = statRef.GetStatFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM, statFilterOrder);

        }

        [Test]
        public void Add_And_Remove_Constant_Stat_Filters_Works() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatReference statRef = new StatReference(BASE_STAT);

            // Add and test filters
            statRef.AddStatFilter(new IntFilterSequence.ConstantFilter("filter1", 5));
            int filteredInt = statRef.GetStatFiltered();
            Assert.AreEqual(BASE_STAT + 5, filteredInt);

            // Add and test filters
            statRef.AddStatFilter(new IntFilterSequence.ConstantFilter("filter2", -10));
            filteredInt = statRef.GetStatFiltered();
            Assert.AreEqual(BASE_STAT + 5 - 10, filteredInt);

            // Remove and test filters
            statRef.RemoveStatFilter("filter1");
            filteredInt = statRef.GetStatFiltered();
            Assert.AreEqual(BASE_STAT - 10, filteredInt);

            // Remove and test filters
            statRef.RemoveStatFilter("filter2");
            filteredInt = statRef.GetStatFiltered();
            Assert.AreEqual(BASE_STAT, filteredInt);
        }

        [Test]
        public void Add_And_Remove_Scalar_Stat_Filters_Works() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatReference statRef = new StatReference(BASE_STAT);

            // Add and test filters
            statRef.AddStatFilter(new IntFilterSequence.ScalarFilter("filter1", 2));
            int filteredInt = statRef.GetStatFiltered();
            Assert.AreEqual(BASE_STAT * 2, filteredInt);

            // Add and test filters
            statRef.AddStatFilter(new IntFilterSequence.ScalarFilter("filter2", .5f));
            filteredInt = statRef.GetStatFiltered();
            Assert.AreEqual(BASE_STAT * 2 * .5f, filteredInt);

            // Remove and test filters
            statRef.RemoveStatFilter("filter1");
            filteredInt = statRef.GetStatFiltered();
            Assert.AreEqual(BASE_STAT * .5f, filteredInt);

            // Remove and test filters
            statRef.RemoveStatFilter("filter2");
            filteredInt = statRef.GetStatFiltered();
            Assert.AreEqual(BASE_STAT, filteredInt);
        }

        [Test]
        public void Add_And_Remove_Custom_Stat_Filters_Work() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatReference statRef = new StatReference(BASE_STAT);

            // Add and test filters
            statRef.AddStatFilter(new IntFilterSequence.CustomFilter("filter1",
                (float value) => {
                    return (value + 8) / 2;
                }
            ));
            int filteredStat = statRef.GetStatFiltered();
            Assert.AreEqual((BASE_STAT + 8) / 2, filteredStat);

            // Remove and test filters
            statRef.RemoveStatFilter("filter1");
            filteredStat = statRef.GetStatFiltered();
            Assert.AreEqual(BASE_STAT, filteredStat);
        }

        [Test]
        public void Can_Restrict_And_Order_Stat_Filters() {
            const int BASE_STAT = 20;
            const int CONSTANT_VAL = 6;
            const int SCALAR_VAL = 10;
            IntFilterSequence.DFilter customVal = (float value) => {
                return (value + 8) / 2;
            };

            // Create new filter int
            StatReference statRef = new StatReference(BASE_STAT);

            // Add and test filters
            statRef.AddStatFilter(new IntFilterSequence.CustomFilter("filter3", customVal));
            int filteredInt = statRef.GetStatFiltered();
            int afterCustom = (int) customVal(BASE_STAT);
            Assert.AreEqual(afterCustom, filteredInt);

            // Add and test filters
            statRef.AddStatFilter(new IntFilterSequence.ConstantFilter("filter1", CONSTANT_VAL));
            filteredInt = statRef.GetStatFiltered();
            int afterConstant = BASE_STAT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            // Add and test filters
            statRef.AddStatFilter(new IntFilterSequence.ScalarFilter("filter2", SCALAR_VAL));
            filteredInt = statRef.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            int afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test Single Restrictions -- //

            // Restrict to CONSTANT only
            statRef.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT);
            filteredInt = statRef.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

            // Restrict to SCALAR only
            statRef.SetStatFilterOrder(IntFilterSequence.Order.SCALAR);
            filteredInt = statRef.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            // Restrict to CUSTOM only
            statRef.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM);
            filteredInt = statRef.GetStatFiltered();
            afterCustom = (int) customVal(BASE_STAT);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test that filters are still present, just not used. -- //

            // Reset to default filtering Order
            statRef.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filteredInt = statRef.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int) customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test Double Restrictions/Orderings -- //

            statRef.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR);
            filteredInt = statRef.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.SCALAR_CONSTANT);
            filteredInt = statRef.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);


            statRef.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_CUSTOM);
            filteredInt = statRef.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterCustom = (int) customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM_CONSTANT);
            filteredInt = statRef.GetStatFiltered();
            afterCustom = (int) customVal(BASE_STAT);
            afterConstant = afterCustom + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);


            statRef.SetStatFilterOrder(IntFilterSequence.Order.SCALAR_CUSTOM);
            filteredInt = statRef.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            afterCustom = (int) customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM_SCALAR);
            filteredInt = statRef.GetStatFiltered();
            afterCustom = (int) customVal(BASE_STAT);
            afterScalar = afterCustom * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            // -- Test Triple Restrictions/Orderings -- //

            statRef.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filteredInt = statRef.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int) customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.SCALAR_CONSTANT_CUSTOM);
            filteredInt = statRef.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            afterCustom = (int) customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_CUSTOM_SCALAR);
            filteredInt = statRef.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterCustom = (int) customVal(afterConstant);
            afterScalar = afterCustom * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.SCALAR_CUSTOM_CONSTANT);
            filteredInt = statRef.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            afterCustom = (int) customVal(afterScalar);
            afterConstant = afterCustom + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM_CONSTANT_SCALAR);
            filteredInt = statRef.GetStatFiltered();
            afterCustom = (int) customVal(BASE_STAT);
            afterConstant = afterCustom + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            statRef.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM_SCALAR_CONSTANT);
            filteredInt = statRef.GetStatFiltered();
            afterCustom = (int) customVal(BASE_STAT);
            afterScalar = afterCustom * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

        }
    }
}