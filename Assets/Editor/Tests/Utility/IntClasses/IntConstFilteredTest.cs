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
    public class IntConstFilteredTest {

        [Test]
        public void Base_Int_Remains_Retrievable() {
            const int BASE_INT = 20;

            // Create new filter int
            IntConstFiltered fint = new IntConstFiltered(BASE_INT);

            // Retrieve the baseInt
            int baseInt = fint.GetBaseInt();

            // Compare
            Assert.AreEqual(BASE_INT, baseInt);

        }

        [Test]
        public void Filtered_Int_Starts_Same_As_Base_Int() {
            const int BASE_INT = 20;

            // Create new filter int
            IntConstFiltered fint = new IntConstFiltered(BASE_INT);

            // Retrieve the filteredInt
            int filteredInt = fint.GetFilteredInt();

            // Compare
            Assert.AreEqual(BASE_INT, filteredInt);

        }

        [Test]
        public void Filter_Order_Default_Setting() {
            const int BASE_INT = 20;

            // Create new filter int
            IntConstFiltered fint = new IntConstFiltered(BASE_INT);

            IntFilterSequence.Order filterOrder = fint.GetFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM, filterOrder);

        }

        [Test]
        public void Can_Set_And_Retrieve_Filter_Order() {
            const int BASE_INT = 20;

            // Create new filter int
            IntConstFiltered fint = new IntConstFiltered(BASE_INT);

            fint.SetFilterOrder(IntFilterSequence.Order.CUSTOM);
            IntFilterSequence.Order filterOrder = fint.GetFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CUSTOM, filterOrder);

            fint.SetFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filterOrder = fint.GetFilterOrder();
            Assert.AreEqual(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM, filterOrder);

        }

        [Test]
        public void Add_And_Remove_Constant_Filters_Works() {
            const int BASE_INT = 20;

            // Create new filter int
            IntConstFiltered fint = new IntConstFiltered(BASE_INT);

            // Add and test filters
            fint.AddFilter(new IntFilterSequence.ConstantFilter("filter1", 5));
            int filteredInt = fint.GetFilteredInt();
            Assert.AreEqual(BASE_INT + 5, filteredInt);

            // Add and test filters
            fint.AddFilter(new IntFilterSequence.ConstantFilter("filter2", -10));
            filteredInt = fint.GetFilteredInt();
            Assert.AreEqual(BASE_INT + 5 - 10, filteredInt);

            // Remove and test filters
            fint.RemoveFilter("filter1");
            filteredInt = fint.GetFilteredInt();
            Assert.AreEqual(BASE_INT - 10, filteredInt);

            // Remove and test filters
            fint.RemoveFilter("filter2");
            filteredInt = fint.GetFilteredInt();
            Assert.AreEqual(BASE_INT, filteredInt);
        }

        [Test]
        public void Add_And_Remove_Scalar_Filters_Works() {
            const int BASE_INT = 20;

            // Create new filter int
            IntConstFiltered fint = new IntConstFiltered(BASE_INT);

            // Add and test filters
            fint.AddFilter(new IntFilterSequence.ScalarFilter("filter1", 2));
            int filteredInt = fint.GetFilteredInt();
            Assert.AreEqual(BASE_INT * 2, filteredInt);

            // Add and test filters
            fint.AddFilter(new IntFilterSequence.ScalarFilter("filter2", .5f));
            filteredInt = fint.GetFilteredInt();
            Assert.AreEqual(BASE_INT * 2 * .5f, filteredInt);

            // Remove and test filters
            fint.RemoveFilter("filter1");
            filteredInt = fint.GetFilteredInt();
            Assert.AreEqual(BASE_INT * .5f, filteredInt);

            // Remove and test filters
            fint.RemoveFilter("filter2");
            filteredInt = fint.GetFilteredInt();
            Assert.AreEqual(BASE_INT, filteredInt);
        }

        [Test]
        public void Add_And_Remove_Custom_Filters_Work() {
            const int BASE_INT = 20;

            // Create new filter int
            IntConstFiltered fint = new IntConstFiltered(BASE_INT);

            // Add and test filters
            fint.AddFilter(new IntFilterSequence.CustomFilter("filter1",
                (float value) => {
                    return (value + 8) / 2;
                }
            ));
            int filteredInt = fint.GetFilteredInt();
            Assert.AreEqual((BASE_INT + 8) / 2, filteredInt);

            // Remove and test filters
            fint.RemoveFilter("filter1");
            filteredInt = fint.GetFilteredInt();
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
            IntConstFiltered fint = new IntConstFiltered(BASE_INT);

            // Add and test filters
            fint.AddFilter(new IntFilterSequence.CustomFilter("filter3", customVal));
            int filteredInt = fint.GetFilteredInt();
            int afterCustom = (int)customVal(BASE_INT);
            Assert.AreEqual(afterCustom, filteredInt);

            // Add and test filters
            fint.AddFilter(new IntFilterSequence.ConstantFilter("filter1", CONSTANT_VAL));
            filteredInt = fint.GetFilteredInt();
            int afterConstant = BASE_INT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            // Add and test filters
            fint.AddFilter(new IntFilterSequence.ScalarFilter("filter2", SCALAR_VAL));
            filteredInt = fint.GetFilteredInt();
            afterConstant = BASE_INT + CONSTANT_VAL;
            int afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test Single Restrictions -- //

            // Restrict to CONSTANT only
            fint.SetFilterOrder(IntFilterSequence.Order.CONSTANT);
            filteredInt = fint.GetFilteredInt();
            afterConstant = BASE_INT + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

            // Restrict to SCALAR only
            fint.SetFilterOrder(IntFilterSequence.Order.SCALAR);
            filteredInt = fint.GetFilteredInt();
            afterScalar = BASE_INT * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            // Restrict to CUSTOM only
            fint.SetFilterOrder(IntFilterSequence.Order.CUSTOM);
            filteredInt = fint.GetFilteredInt();
            afterCustom = (int)customVal(BASE_INT);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test that filters are still present, just not used. -- //

            // Reset to default filtering Order
            fint.SetFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filteredInt = fint.GetFilteredInt();
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            // -- Test Double Restrictions/Orderings -- //

            fint.SetFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR);
            filteredInt = fint.GetFilteredInt();
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            fint.SetFilterOrder(IntFilterSequence.Order.SCALAR_CONSTANT);
            filteredInt = fint.GetFilteredInt();
            afterScalar = BASE_INT * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);


            fint.SetFilterOrder(IntFilterSequence.Order.CONSTANT_CUSTOM);
            filteredInt = fint.GetFilteredInt();
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            fint.SetFilterOrder(IntFilterSequence.Order.CUSTOM_CONSTANT);
            filteredInt = fint.GetFilteredInt();
            afterCustom = (int)customVal(BASE_INT);
            afterConstant = afterCustom + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);


            fint.SetFilterOrder(IntFilterSequence.Order.SCALAR_CUSTOM);
            filteredInt = fint.GetFilteredInt();
            afterScalar = BASE_INT * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            fint.SetFilterOrder(IntFilterSequence.Order.CUSTOM_SCALAR);
            filteredInt = fint.GetFilteredInt();
            afterCustom = (int)customVal(BASE_INT);
            afterScalar = afterCustom * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            // -- Test Triple Restrictions/Orderings -- //

            fint.SetFilterOrder(IntFilterSequence.Order.CONSTANT_SCALAR_CUSTOM);
            filteredInt = fint.GetFilteredInt();
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            Assert.AreEqual(afterCustom, filteredInt);

            fint.SetFilterOrder(IntFilterSequence.Order.SCALAR_CONSTANT_CUSTOM);
            filteredInt = fint.GetFilteredInt();
            afterScalar = BASE_INT * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            Assert.AreEqual(afterCustom, filteredInt);

            fint.SetFilterOrder(IntFilterSequence.Order.CONSTANT_CUSTOM_SCALAR);
            filteredInt = fint.GetFilteredInt();
            afterConstant = BASE_INT + CONSTANT_VAL;
            afterCustom = (int)customVal(afterConstant);
            afterScalar = afterCustom * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            fint.SetFilterOrder(IntFilterSequence.Order.SCALAR_CUSTOM_CONSTANT);
            filteredInt = fint.GetFilteredInt();
            afterScalar = BASE_INT * SCALAR_VAL;
            afterCustom = (int)customVal(afterScalar);
            afterConstant = afterCustom + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

            fint.SetFilterOrder(IntFilterSequence.Order.CUSTOM_CONSTANT_SCALAR);
            filteredInt = fint.GetFilteredInt();
            afterCustom = (int)customVal(BASE_INT);
            afterConstant = afterCustom + CONSTANT_VAL;
            afterScalar = afterConstant * SCALAR_VAL;
            Assert.AreEqual(afterScalar, filteredInt);

            fint.SetFilterOrder(IntFilterSequence.Order.CUSTOM_SCALAR_CONSTANT);
            filteredInt = fint.GetFilteredInt();
            afterCustom = (int)customVal(BASE_INT);
            afterScalar = afterCustom * SCALAR_VAL;
            afterConstant = afterScalar + CONSTANT_VAL;
            Assert.AreEqual(afterConstant, filteredInt);

        }
    }

}
