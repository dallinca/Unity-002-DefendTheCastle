using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;

namespace Tests {
    
    /// <summary>
    /// TO TEST
    /// Timed Filter Removal
    /// -- Custom Enums (with depletion and gaining)
    /// </summary>
    [TestFixture]
    public class StatDepletionTest {

        [Test]
        public void Base_Stat_Remains_Retrievable() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            // Retrieve the baseInt
            int baseStat = statDep.GetStatBase();

            // Compare
            Assert.AreEqual(BASE_STAT, baseStat);

        }

        [Test]
        public void Base_Stat_Changeable() {
            const int BASE_STAT = 20;
            const int NEW_BASE_STAT = 30;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);
            int baseStat = statDep.GetStatBase();
            Assert.AreEqual(BASE_STAT, baseStat);

            // Change the filter Base Int and Test
            statDep.SetStatBase(NEW_BASE_STAT);
            baseStat = statDep.GetStatBase();
            Assert.AreEqual(NEW_BASE_STAT, baseStat);

        }

        [Test]
        public void Filtered_Stat_Starts_Same_As_Base_Stat() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            // Retrieve the filteredInt
            int filteredStat = statDep.GetStatFiltered();

            // Compare
            Assert.AreEqual(BASE_STAT, filteredStat);

        }

        [Test]
        public void Filter_Order_Default_Setting() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            IntFilterSequence.Order filterOrder = statDep.GetStatFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM, filterOrder);

        }

        [Test]
        public void Can_Set_And_Retrieve_Stat_Filter_Order() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM);
            IntFilterSequence.Order statFilterOrder = statDep.GetStatFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CUSTOM, statFilterOrder);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            statFilterOrder = statDep.GetStatFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM, statFilterOrder);

        }

        [Test]
        public void Add_And_Remove_Constant_Stat_Filters_Works() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            // Add and test filters
            statDep.AddStatFilter(new IntFilterSequence.ConstantFilter("filter1", 5));
            int filteredInt = statDep.GetStatFiltered();
            Assert.AreEqual(BASE_STAT + 5, filteredInt);

            // Add and test filters
            statDep.AddStatFilter(new IntFilterSequence.ConstantFilter("filter2", -10));
            filteredInt = statDep.GetStatFiltered();
            Assert.AreEqual(BASE_STAT + 5 - 10, filteredInt);

            // Remove and test filters
            statDep.RemoveStatFilter("filter1");
            filteredInt = statDep.GetStatFiltered();
            Assert.AreEqual(BASE_STAT - 10, filteredInt);

            // Remove and test filters
            statDep.RemoveStatFilter("filter2");
            filteredInt = statDep.GetStatFiltered();
            Assert.AreEqual(BASE_STAT, filteredInt);
        }

        [Test]
        public void Add_And_Remove_Scalar_Stat_Filters_Works() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            // Add and test filters
            statDep.AddStatFilter(new IntFilterSequence.ScalarFilter("filter1", 2));
            int filteredInt = statDep.GetStatFiltered();
            Assert.AreEqual(BASE_STAT * 2, filteredInt);

            // Add and test filters
            statDep.AddStatFilter(new IntFilterSequence.ScalarFilter("filter2", .5f));
            filteredInt = statDep.GetStatFiltered();
            Assert.AreEqual(BASE_STAT * 2 * .5f, filteredInt);

            // Remove and test filters
            statDep.RemoveStatFilter("filter1");
            filteredInt = statDep.GetStatFiltered();
            Assert.AreEqual(BASE_STAT * .5f, filteredInt);

            // Remove and test filters
            statDep.RemoveStatFilter("filter2");
            filteredInt = statDep.GetStatFiltered();
            Assert.AreEqual(BASE_STAT, filteredInt);
        }

        [Test]
        public void Add_And_Remove_Custom_Stat_Filters_Work() {
            const int BASE_STAT = 20;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            // Add and test filters
            statDep.AddStatFilter(new IntFilterSequence.CustomFilter("filter1",
                (float value) => {
                    return (value + 8) / 2;
                }
            ));
            int filteredStat = statDep.GetStatFiltered();
            Assert.AreEqual((BASE_STAT + 8) / 2, filteredStat);

            // Remove and test filters
            statDep.RemoveStatFilter("filter1");
            filteredStat = statDep.GetStatFiltered();
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
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            // Add and test filters
            statDep.AddStatFilter(new IntFilterSequence.CustomFilter("filter3", customVal));
            int filteredInt = statDep.GetStatFiltered();
            int afterCustom = (int)customVal(BASE_STAT);
            Assert.AreEqual(afterCustom, filteredInt);

            // Add and test filters
            statDep.AddStatFilter(new IntFilterSequence.ConstantFilter("filter1", CONSTANT_VAL));
            filteredInt = statDep.GetStatFiltered();
            int afterConstant = BASE_STAT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            // Add and test filters
            statDep.AddStatFilter(new IntFilterSequence.ScalarFilter("filter2", SCALAR_VAL));
            filteredInt = statDep.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            int afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test Single Restrictions -- //

            // Restrict to CONSTANT only
            statDep.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT);
            filteredInt = statDep.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

            // Restrict to SCALAR only
            statDep.SetStatFilterOrder(IntFilterSequence.Order.SCALAR);
            filteredInt = statDep.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            // Restrict to CUSTOM only
            statDep.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM);
            filteredInt = statDep.GetStatFiltered();
            afterCustom = (int)customVal(BASE_STAT);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test that filters are still present, just not used. -- //

            // Reset to default filtering Order
            statDep.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filteredInt = statDep.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test Double Restrictions/Orderings -- //

            statDep.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR);
            filteredInt = statDep.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.SCALAR_CONSTANT);
            filteredInt = statDep.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);


            statDep.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_CUSTOM);
            filteredInt = statDep.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM_CONSTANT);
            filteredInt = statDep.GetStatFiltered();
            afterCustom = (int)customVal(BASE_STAT);
            afterConstant = afterCustom + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);


            statDep.SetStatFilterOrder(IntFilterSequence.Order.SCALAR_CUSTOM);
            filteredInt = statDep.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM_SCALAR);
            filteredInt = statDep.GetStatFiltered();
            afterCustom = (int)customVal(BASE_STAT);
            afterScalar = afterCustom * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            // -- Test Triple Restrictions/Orderings -- //

            statDep.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filteredInt = statDep.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.SCALAR_CONSTANT_CUSTOM);
            filteredInt = statDep.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.CONSTANT_CUSTOM_SCALAR);
            filteredInt = statDep.GetStatFiltered();
            afterConstant = BASE_STAT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            afterScalar = afterCustom * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.SCALAR_CUSTOM_CONSTANT);
            filteredInt = statDep.GetStatFiltered();
            afterScalar = BASE_STAT * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            afterConstant = afterCustom + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM_CONSTANT_SCALAR);
            filteredInt = statDep.GetStatFiltered();
            afterCustom = (int)customVal(BASE_STAT);
            afterConstant = afterCustom + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            statDep.SetStatFilterOrder(IntFilterSequence.Order.CUSTOM_SCALAR_CONSTANT);
            filteredInt = statDep.GetStatFiltered();
            afterCustom = (int)customVal(BASE_STAT);
            afterScalar = afterCustom * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

        }

        [Test]
        public void Remaining_Stat_Value_Starts_Same_As_Filtered_Stat() {
            const int BASE_STAT = 10;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            // Compare without filters
            int remainingValue = statDep.GetRemainingStatValue();
            int filteredStat = statDep.GetStatFiltered();
            Assert.AreEqual(filteredStat, remainingValue);

            // Compare with a filter
            statDep.AddStatFilter(new IntFilterSequence.ConstantFilter("Filter1", 5));
            remainingValue = statDep.GetRemainingStatValue();
            filteredStat = statDep.GetStatFiltered();
            Assert.AreEqual(filteredStat, remainingValue);

        }

        [Test]
        public void Can_Deplete_Past_Zero_Default_Setting() {
            const int BASE_STAT = 10;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            bool canDepletePastZero = statDep.GetCanDepletePastZeroSetting();
            Assert.AreEqual(false, canDepletePastZero);

        }

        [Test]
        public void Can_Gain_Past_Ceiling_Default_Setting() {
            const int BASE_STAT = 10;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            bool canGainPastCeiling = statDep.GetCanGainPastCeilingSetting();
            Assert.AreEqual(false, canGainPastCeiling);

        }

        [Test]
        public void Can_Deplete_Stat_And_Remaining_Stat_Value_Stays_In_Bounds() {
            const int BASE_STAT = 10;
            const int DEPLETE_STAT_VALUE_3 = 3;
            const int DEPLETE_STAT_VALUE_4 = 4;
            const int DEPLETE_STAT_VALUE_5 = 5;

            const int GAIN_STAT_VALUE_4 = 4;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_3);
            int remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - DEPLETE_STAT_VALUE_3, remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - (DEPLETE_STAT_VALUE_3 + DEPLETE_STAT_VALUE_4), remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_5);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(0, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(GAIN_STAT_VALUE_4, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(GAIN_STAT_VALUE_4 * 2, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT, remainingValue);

        }

        [Test]
        public void Can_Deplete_Stat_Past_Zero_And_Stay_Below_Ceiling_For_Total_Depletion() {
            const int BASE_STAT = 10;
            const int DEPLETE_STAT_VALUE_3 = 3;
            const int DEPLETE_STAT_VALUE_4 = 4;
            const int DEPLETE_STAT_VALUE_5 = 5;

            const int GAIN_STAT_VALUE_4 = 4;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT, true, false);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_3);
            int remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - DEPLETE_STAT_VALUE_3, remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - (DEPLETE_STAT_VALUE_3 + DEPLETE_STAT_VALUE_4), remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_5);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(-2, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(2, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(6, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(10, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT, remainingValue);

        }

        [Test]
        public void Can_Gain_Past_Ceiling_And_Stay_Above_Zero_For_Total_Depletion() {
            const int BASE_STAT = 10;
            const int DEPLETE_STAT_VALUE_3 = 3;
            const int DEPLETE_STAT_VALUE_4 = 4;
            const int DEPLETE_STAT_VALUE_5 = 5;

            const int GAIN_STAT_VALUE_4 = 4;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT, false, true);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_3);
            int remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - DEPLETE_STAT_VALUE_3, remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - (DEPLETE_STAT_VALUE_3 + DEPLETE_STAT_VALUE_4), remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_5);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(0, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(4, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(8, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(12, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(16, remainingValue);

        }

        [Test]
        public void Can_Gain_Past_Ceiling_And_Deplete_Past_Zero_For_Total_Depletion() {
            const int BASE_STAT = 10;
            const int DEPLETE_STAT_VALUE_3 = 3;
            const int DEPLETE_STAT_VALUE_4 = 4;
            const int DEPLETE_STAT_VALUE_5 = 5;

            const int GAIN_STAT_VALUE_4 = 4;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT, true, true);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_3);
            int remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - DEPLETE_STAT_VALUE_3, remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - (DEPLETE_STAT_VALUE_3 + DEPLETE_STAT_VALUE_4), remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_5);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(-2, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(2, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(6, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(10, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(14, remainingValue);

        }

        [Test]
        public void Can_Reset_Depletion_Amount() {
            const int BASE_STAT = 10;
            const int DEPLETE_STAT_VALUE_3 = 3;
            const int DEPLETE_STAT_VALUE_4 = 4;
            const int DEPLETE_STAT_VALUE_5 = 5;

            const int GAIN_STAT_VALUE_4 = 4;

            // Create new filter int
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> statDep = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_STAT, true, true);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_3);
            int remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - DEPLETE_STAT_VALUE_3, remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(BASE_STAT - (DEPLETE_STAT_VALUE_3 + DEPLETE_STAT_VALUE_4), remainingValue);

            // Test Stat Depletion
            statDep.DepleteStat(DEPLETE_STAT_VALUE_5);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(-2, remainingValue);

            // Test Reset Total Stat Depletion
            statDep.ResetDepletionAmount(10);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(0, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(4, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(8, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(12, remainingValue);
            
            // Test Reset Total Stat Depletion
            statDep.ResetDepletionAmount(3);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(7, remainingValue);

            // Test Stat Gain
            statDep.GainStat(GAIN_STAT_VALUE_4);
            remainingValue = statDep.GetRemainingStatValue();
            Assert.AreEqual(11, remainingValue);

        }

        [Test]
        public void Test_Float_To_Int_Conversion() {

            int float25 = (int)Math.Round(2.5f, MidpointRounding.AwayFromZero);
            int float35 = (int)Math.Round(3.5f, MidpointRounding.AwayFromZero);
            int float45 = (int)Math.Round(4.5f, MidpointRounding.AwayFromZero);
            int float55 = (int)Math.Round(5.5f, MidpointRounding.AwayFromZero);
            int float65 = (int)Math.Round(6.5f, MidpointRounding.AwayFromZero);

            Assert.AreEqual(3, float25); // Actual = 2
            Assert.AreEqual(4, float35);
            Assert.AreEqual(5, float45); // Actual = 4
            Assert.AreEqual(6, float55);
            Assert.AreEqual(7, float65); // Actual = 6
        }

        [Test]
        public void Depletion_And_Gain_Filters_Work_With_Default_Interaction_Type_And_Different_Rounding_Strategies() {

            // Init the health stat depletion
            const int BASE_HEALTH = 100;
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> health = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_HEALTH);

            // Equip Items
            health.AddDepletionFilter(new IntFilterSequence.ConstantFilter("Helmet", -2));
            health.AddDepletionFilter(new IntFilterSequence.ConstantFilter("PlateLegs", -4));
            health.AddDepletionFilter(new IntFilterSequence.ConstantFilter("PlateBody", -5));
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("AmuletOfDefense", .9f));

            // Test Soft Attack that does no damage
            int healthLeft = health.DepleteStat(2);
            Assert.AreEqual(BASE_HEALTH, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(30);
            Assert.AreEqual(83, healthLeft);

            // Take Soft Attack that does no damage
            healthLeft = health.DepleteStat(2);
            Assert.AreEqual(83, healthLeft);

            // UnEquip Items
            health.RemoveDepletionFilter("AmuletOfDefense");

            // Take Damage
            healthLeft = health.DepleteStat(30);
            Assert.AreEqual(64, healthLeft);

            // Gain Health (Drink Potion)
            healthLeft = health.GainStat(6);
            Assert.AreEqual(70, healthLeft);

            // Equip Items
            health.AddGainFilter(new IntFilterSequence.ConstantFilter("AmuletOfHealing", 2));

            // Gain Health (Drink Potion)
            healthLeft = health.GainStat(6);
            Assert.AreEqual(78, healthLeft);

            // Gain Health (Drink Potion)
            healthLeft = health.GainStat(6);
            Assert.AreEqual(86, healthLeft);

            // Gain Health (Drink Potion)
            healthLeft = health.GainStat(6);
            Assert.AreEqual(94, healthLeft);

            // Gain Health (Drink Potion)
            healthLeft = health.GainStat(6);
            Assert.AreEqual(100, healthLeft);

            // UnEquip Items
            health.RemoveGainFilter("AmuletOfHealing");

            // Equip Items
            health.AddGainFilter(new IntFilterSequence.ConstantFilter("AmuletOfTheDead", -100));

            // Gain Health (Drink Potion)
            healthLeft = health.GainStat(6);
            Assert.AreEqual(100, healthLeft);

            // UnEquip Items
            health.RemoveDepletionFilter("Helmet");
            health.RemoveDepletionFilter("PlateLegs");
            health.RemoveDepletionFilter("PlateBody");

            // Change Rounding Strategy
            health.SetDepletionAfterFilterRounding(IntFilterSequence.Rounding.UP);

            // Equip Items
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("AmuletOfDefense", .9f));

            // Take Damage
            healthLeft = health.DepleteStat(1);
            Assert.AreEqual(99, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(2);
            Assert.AreEqual(97, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(3);
            Assert.AreEqual(94, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(4);
            Assert.AreEqual(90, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(5);
            Assert.AreEqual(85, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(6);
            Assert.AreEqual(79, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(7);
            Assert.AreEqual(72, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(8);
            Assert.AreEqual(64, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(9);
            Assert.AreEqual(55, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(10);
            Assert.AreEqual(46, healthLeft);

            // -------------
            // Reset Health
            health.ResetDepletionAmount(0);
            Assert.AreEqual(BASE_HEALTH, health.GetRemainingStatValue());

            // Change Rounding Strategy
            health.SetDepletionAfterFilterRounding(IntFilterSequence.Rounding.MIDDLE);

            // Equip Items
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("AmuletOfDefense", .9f));

            // Take Damage
            healthLeft = health.DepleteStat(1);
            Assert.AreEqual(99, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(2);
            Assert.AreEqual(97, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(3);
            Assert.AreEqual(94, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(4);
            Assert.AreEqual(90, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(5);
            Assert.AreEqual(85, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(6);
            Assert.AreEqual(80, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(7);
            Assert.AreEqual(74, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(8);
            Assert.AreEqual(67, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(9);
            Assert.AreEqual(59, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(10);
            Assert.AreEqual(50, healthLeft);


            // -------------
            // Reset Health
            health.ResetDepletionAmount(0);
            Assert.AreEqual(BASE_HEALTH, health.GetRemainingStatValue());

            // Change Rounding Strategy
            health.SetDepletionAfterFilterRounding(IntFilterSequence.Rounding.DOWN);

            // Equip Items
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("AmuletOfDefense", .9f));

            // Take Damage
            healthLeft = health.DepleteStat(1);
            Assert.AreEqual(100, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(2);
            Assert.AreEqual(99, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(3);
            Assert.AreEqual(97, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(4);
            Assert.AreEqual(94, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(5);
            Assert.AreEqual(90, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(6);
            Assert.AreEqual(85, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(7);
            Assert.AreEqual(79, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(8);
            Assert.AreEqual(72, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(9);
            Assert.AreEqual(64, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(10);
            Assert.AreEqual(55, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(11);
            Assert.AreEqual(46, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(12);
            Assert.AreEqual(36, healthLeft);

        }

        [Test]
        public void Multiple_Scalar_Filters_And_Rounding() {

            // Init the health stat depletion
            const int BASE_HEALTH = 100;
            StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType> health = new StatDepletion<EDefaultStatInteractionType, EDefaultStatInteractionType>(BASE_HEALTH);
            health.SetDepletionAfterFilterRounding(IntFilterSequence.Rounding.MIDDLE);

            // Equip Items
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("UltimateHelmet", .95f));
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("UltimatePlateLegs", .9f));
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("UltimatePlateBody", .8f));
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("AmuletOfDefense", .9f));

            // resulting filter scalar is (.6156)
            float combinedScalars = 0.6156f;
            
            // Take Damage
            int healthLeft = health.DepleteStat(1);
            Assert.AreEqual(99, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(2);
            Assert.AreEqual(98, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(3);
            Assert.AreEqual(96, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(4);
            Assert.AreEqual(94, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(5);
            Assert.AreEqual(91, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(6);
            Assert.AreEqual(87, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(6);
            Assert.AreEqual(83, healthLeft);
            
        }

        public enum ECustomTypes {
            CRUSH, PIERCE, SLASH,
            FIRE, ICE,
        }

        [Test]
        public void Custom_Interaction_Types() {
            const int BASE_STAT = 100;

            StatDepletion<ECustomTypes, ECustomTypes> health = new StatDepletion<ECustomTypes, ECustomTypes>(BASE_STAT);
            health.SetDepletionAfterFilterRounding(IntFilterSequence.Rounding.MIDDLE, true);

            // Ice Giant Defense Interactions
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("CrushDepletion", 1.5f), ECustomTypes.CRUSH);
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("PierceDepletion", .7f), ECustomTypes.PIERCE);
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("SlashDepletion", .5f), ECustomTypes.SLASH);
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("FireDepletion", 2f), ECustomTypes.FIRE);
            health.AddDepletionFilter(new IntFilterSequence.ScalarFilter("IceDepletion", .2f), ECustomTypes.ICE);
            
            // Take Damage
            int healthLeft = health.DepleteStat(10, ECustomTypes.CRUSH);
            Assert.AreEqual(85, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(10, ECustomTypes.PIERCE);
            Assert.AreEqual(78, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(10, ECustomTypes.SLASH);
            Assert.AreEqual(73, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(10, ECustomTypes.FIRE);
            Assert.AreEqual(53, healthLeft);

            // Take Damage
            healthLeft = health.DepleteStat(10, ECustomTypes.ICE);
            Assert.AreEqual(51, healthLeft);


        }
    }
}