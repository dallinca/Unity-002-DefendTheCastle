using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


namespace Tests {

    [TestFixture]
    public class UnitTest {

        StatFactoryDefault StatFactory = new StatFactoryDefault();

        [Test]
        public void Unit_Base_And_Starting_Health_Is_Correct() {
            const int BASE_HEALTH = 1000;

            UnitDefault unit = new UnitDefault(StatFactory);

            // Test Default setting
            Assert.AreEqual(BASE_HEALTH, UnitDefault.UNIT_BASE_HEALTH);

            int unitBaseHealth = unit.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetStatBase();
            Assert.AreEqual(BASE_HEALTH, unitBaseHealth);

            // Make sure starting health is full
            int unitCurrentHealth = unit.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH, unitCurrentHealth);

        }

        [Test]
        public void Unit_Takes_Damge_To_0_And_Heals_to_Base_Health() {
            const int BASE_HEALTH = 1000;

            UnitDefault unit = new UnitDefault(StatFactory);

            // Taking Damage
            var damageSoFar = 0;
            var damage1 = 400;
            var damage2 = 300;
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> takeDamage1Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_DEPLETE, EDefaultStatType.HEALTH, EDefaultStatInteractionType.NORMAL, damage1);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> takeDamage2Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_DEPLETE, EDefaultStatType.HEALTH, EDefaultStatInteractionType.NORMAL, damage2);

            unit.InteractWithStat(takeDamage1Obj);
            damageSoFar = damage1;
            int unitCurrentHealth = unit.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            unit.InteractWithStat(takeDamage1Obj);
            damageSoFar = damage1 + damage1;
            unitCurrentHealth = unit.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            unit.InteractWithStat(takeDamage2Obj);
            damageSoFar = damage1 + damage1 + damage2; // Greater than BASE_HEALTH of 1000
            damageSoFar = 1000; // Cap damage
            unitCurrentHealth = unit.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            // Healing
            var healingSoFar = 0;
            var heal1 = 350;
            var heal2 = 500;

            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> heal1Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_GAIN, EDefaultStatType.HEALTH, EDefaultStatInteractionType.NORMAL, heal1);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> heal2Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_GAIN, EDefaultStatType.HEALTH, EDefaultStatInteractionType.NORMAL, heal2);

            unit.InteractWithStat(heal1Obj);
            healingSoFar = heal1;
            unitCurrentHealth = unit.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(0 + healingSoFar, unitCurrentHealth);

            unit.InteractWithStat(heal2Obj);
            healingSoFar = heal1 + heal2;
            unitCurrentHealth = unit.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(0 + healingSoFar, unitCurrentHealth);

            unit.InteractWithStat(heal1Obj);
            healingSoFar = heal1 + heal2 + heal1; // Greater than the BASE_HEALTH of 1000
            healingSoFar = 1000; // Cap Healing
            unitCurrentHealth = unit.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(0 + healingSoFar, unitCurrentHealth);

        }

        [Test]
        public void Unit_Takes_Damage_By_Type() {
            const int BASE_HEALTH = 1000;

            UnitDefault fireDemon = new UnitDefault(StatFactory);

            string fireDemon_filterName_fireImmunity = "fireImmunity";
            string fireDemon_filterName_waterWeakness = "waterWeakness";
            string fireDemon_filterName_amuletOfFire_depleteHealth = "amuletOfFire";

            // Setup Deplete Health Filters

            fireDemon.InteractWithStat(
                StatFactory.NewStatInteractionObject(
                    EStatInteractionIntent.ADD_STAT_DEPLETE_FILTER,
                    EDefaultStatType.HEALTH,
                    EDefaultStatInteractionType.FIRE,
                    new IntFilterSequence.ScalarFilter(fireDemon_filterName_fireImmunity, 0.1f)
                )
            );

            fireDemon.InteractWithStat(
                StatFactory.NewStatInteractionObject(
                    EStatInteractionIntent.ADD_STAT_DEPLETE_FILTER,
                    EDefaultStatType.HEALTH,
                    EDefaultStatInteractionType.WATER,
                    new IntFilterSequence.ScalarFilter(fireDemon_filterName_waterWeakness, 1.5f)
                )
            );

            fireDemon.InteractWithStat(
                StatFactory.NewStatInteractionObject(
                    EStatInteractionIntent.ADD_STAT_DEPLETE_FILTER,
                    EDefaultStatType.HEALTH,
                    EDefaultStatInteractionType.FIRE,
                    new IntFilterSequence.ConstantFilter(fireDemon_filterName_amuletOfFire_depleteHealth, -10)
                )
            );


            // Taking Damage
            var damageSoFar = 0;
            var damage1 = 100;
            var damage2 = 200;
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> takeFireDamage1Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_DEPLETE, EDefaultStatType.HEALTH, EDefaultStatInteractionType.FIRE, damage1);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> takePlantDamage1Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_DEPLETE, EDefaultStatType.HEALTH, EDefaultStatInteractionType.PLANT, damage1);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> takeWaterDamage1Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_DEPLETE, EDefaultStatType.HEALTH, EDefaultStatInteractionType.WATER, damage1);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> takeFireDamage2Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_DEPLETE, EDefaultStatType.HEALTH, EDefaultStatInteractionType.FIRE, damage2);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> takePlantDamage2Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_DEPLETE, EDefaultStatType.HEALTH, EDefaultStatInteractionType.PLANT, damage2);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> takeWaterDamage2Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_DEPLETE, EDefaultStatType.HEALTH, EDefaultStatInteractionType.WATER, damage2);
            var fireDamage1Effect_withAmulet = 9;
            var plantDamage1Effect = 100;
            var waterDamage1Effect = 150;
            var fireDamage2Effect_withAmulet = 19;
            var plantDamage2Effect = 200;
            var waterDamage2Effect = 300;
            var fireDamage2Effect = 20;

            fireDemon.InteractWithStat(takeFireDamage1Obj);
            damageSoFar = fireDamage1Effect_withAmulet;
            int unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(takePlantDamage1Obj);
            damageSoFar = fireDamage1Effect_withAmulet + plantDamage1Effect;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(takeWaterDamage1Obj);
            damageSoFar = fireDamage1Effect_withAmulet + plantDamage1Effect + waterDamage1Effect;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(takeFireDamage2Obj);
            damageSoFar = fireDamage1Effect_withAmulet + plantDamage1Effect + waterDamage1Effect + fireDamage2Effect_withAmulet;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(takePlantDamage2Obj);
            damageSoFar = fireDamage1Effect_withAmulet + plantDamage1Effect + waterDamage1Effect + fireDamage2Effect_withAmulet + plantDamage2Effect;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(takeWaterDamage2Obj);
            damageSoFar = fireDamage1Effect_withAmulet + plantDamage1Effect + waterDamage1Effect + fireDamage2Effect_withAmulet + plantDamage2Effect + waterDamage2Effect;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(
                StatFactory.NewStatInteractionObject(
                    EStatInteractionIntent.REMOVE_STAT_DEPLETE_FILTER,
                    EDefaultStatType.HEALTH,
                    EDefaultStatInteractionType.FIRE,
                    fireDemon_filterName_amuletOfFire_depleteHealth
                )
            );

            fireDemon.InteractWithStat(takeFireDamage2Obj);
            damageSoFar = fireDamage1Effect_withAmulet + plantDamage1Effect + waterDamage1Effect + fireDamage2Effect_withAmulet + plantDamage2Effect + waterDamage2Effect + fireDamage2Effect;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(
                StatFactory.NewStatInteractionObject(
                    EStatInteractionIntent.ADD_STAT_DEPLETE_FILTER,
                    EDefaultStatType.HEALTH,
                    EDefaultStatInteractionType.FIRE,
                    new IntFilterSequence.ConstantFilter(fireDemon_filterName_amuletOfFire_depleteHealth, -10)
                )
            );

            fireDemon.InteractWithStat(takeFireDamage2Obj);
            damageSoFar = fireDamage1Effect_withAmulet + plantDamage1Effect + waterDamage1Effect + fireDamage2Effect_withAmulet + plantDamage2Effect + waterDamage2Effect + fireDamage2Effect + fireDamage2Effect_withAmulet;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(BASE_HEALTH - damageSoFar, unitCurrentHealth);


            fireDemon.InteractWithStat(takeWaterDamage2Obj);
            damageSoFar = fireDamage1Effect_withAmulet + plantDamage1Effect + waterDamage1Effect + fireDamage2Effect_withAmulet + plantDamage2Effect + waterDamage2Effect + waterDamage2Effect;
            damageSoFar = 1000; // More Damage than available health
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(0, unitCurrentHealth);

            // Setup Gain Health Filters

            string fireDemon_filterName_fireHealing = "fireImmunity";
            string fireDemon_filterName_waterHealing = "waterWeakness";
            string fireDemon_filterName_amuletOfFire_gainHealth = "amuletOfFire";

            fireDemon.InteractWithStat(
                StatFactory.NewStatInteractionObject(
                    EStatInteractionIntent.ADD_STAT_GAIN_FILTER,
                    EDefaultStatType.HEALTH,
                    EDefaultStatInteractionType.FIRE,
                    new IntFilterSequence.ScalarFilter(fireDemon_filterName_fireHealing, 1.5f)
                )
            );

            fireDemon.InteractWithStat(
                StatFactory.NewStatInteractionObject(
                    EStatInteractionIntent.ADD_STAT_GAIN_FILTER,
                    EDefaultStatType.HEALTH,
                    EDefaultStatInteractionType.WATER,
                    new IntFilterSequence.ScalarFilter(fireDemon_filterName_waterHealing, 0.1f)
                )
            );

            fireDemon.InteractWithStat(
                StatFactory.NewStatInteractionObject(
                    EStatInteractionIntent.ADD_STAT_GAIN_FILTER,
                    EDefaultStatType.HEALTH,
                    EDefaultStatInteractionType.FIRE,
                    new IntFilterSequence.ConstantFilter(fireDemon_filterName_amuletOfFire_gainHealth, 10)
                )
            );

            // Gain Stat Filters
            var healingSoFar = 0;
            var heal1 = 100;
            var heal2 = 200;
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> fireHeal1Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_GAIN, EDefaultStatType.HEALTH, EDefaultStatInteractionType.FIRE, heal1);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> plantHeal1Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_GAIN, EDefaultStatType.HEALTH, EDefaultStatInteractionType.PLANT, heal1);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> waterHeal1Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_GAIN, EDefaultStatType.HEALTH, EDefaultStatInteractionType.WATER, heal1);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> fireHeal2Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_GAIN, EDefaultStatType.HEALTH, EDefaultStatInteractionType.FIRE, heal2);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> plantHeal2Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_GAIN, EDefaultStatType.HEALTH, EDefaultStatInteractionType.PLANT, heal2);
            StatInteraction<EDefaultStatType, EDefaultStatInteractionType> waterHeal2Obj = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_GAIN, EDefaultStatType.HEALTH, EDefaultStatInteractionType.WATER, heal2);
            var fireHeal1Effect_withAmulet = 165;
            var plantHeal1Effect = 100;
            var waterHeal1Effect = 10;
            var fireHeal2Effect_withAmulet = 315;
            var plantHeal2Effect = 200;
            var waterHeal2Effect = 20;


            fireDemon.InteractWithStat(fireHeal1Obj);
            healingSoFar = fireHeal1Effect_withAmulet;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(healingSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(plantHeal1Obj);
            healingSoFar = fireHeal1Effect_withAmulet + plantHeal1Effect;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(healingSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(waterHeal1Obj);
            healingSoFar = fireHeal1Effect_withAmulet + plantHeal1Effect + waterHeal1Effect;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(healingSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(fireHeal2Obj);
            healingSoFar = fireHeal1Effect_withAmulet + plantHeal1Effect + waterHeal1Effect + fireHeal2Effect_withAmulet;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(healingSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(plantHeal2Obj);
            healingSoFar = fireHeal1Effect_withAmulet + plantHeal1Effect + waterHeal1Effect + fireHeal2Effect_withAmulet + plantHeal2Effect;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(healingSoFar, unitCurrentHealth);

            fireDemon.InteractWithStat(waterHeal2Obj);
            healingSoFar = fireHeal1Effect_withAmulet + plantHeal1Effect + waterHeal1Effect + fireHeal2Effect_withAmulet + plantHeal2Effect + waterHeal2Effect;
            unitCurrentHealth = fireDemon.GetStat(EDefaultStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(healingSoFar, unitCurrentHealth);


        }

        [Test]
        public void Unit_Stat_Reference_functions_properly() {
            const int BASE_SPEED = 100;

            UnitDefault unit = new UnitDefault(StatFactory);
            StatReferenceDefault statSpeed = (StatReferenceDefault)unit.GetStat(EDefaultStatType.MOVEMENT_SPEED).asStatReference;

            // Base speed set correctly
            statSpeed.SetStatBase(BASE_SPEED);
            int statBase = statSpeed.GetStatBase();
            int expectedBase = BASE_SPEED;
            Assert.AreEqual(expectedBase, statBase);

            int currentSpeed = statSpeed.GetStatFiltered();
            int expectedSpeed = BASE_SPEED;
            Assert.AreEqual(expectedSpeed, currentSpeed);

            // Can Add filters
            string quicksand = "quicksand";
            string bootsOfSpeed = "bootsOfSpeed";
            string slowSpell = "slowSpell";

            unit.InteractWithStat(StatFactory.NewStatInteractionObject(
                EStatInteractionIntent.ADD_STAT_FILTER,
                EDefaultStatType.MOVEMENT_SPEED,
                EDefaultStatInteractionType.NORMAL,
                new IntFilterSequence.ScalarFilter(quicksand, 0.9f)
                )
            );
            int expectedUnitSpeed = 90;
            int unitSpeed = unit.GetStat(EDefaultStatType.MOVEMENT_SPEED).asStatReference.GetStatFiltered();
            Assert.AreEqual(expectedUnitSpeed, unitSpeed);

            unit.InteractWithStat(StatFactory.NewStatInteractionObject(
                EStatInteractionIntent.ADD_STAT_FILTER,
                EDefaultStatType.MOVEMENT_SPEED,
                EDefaultStatInteractionType.NORMAL,
                new IntFilterSequence.ScalarFilter(bootsOfSpeed, 1.1f)
                )
            );
            expectedUnitSpeed = 99;
            unitSpeed = unit.GetStat(EDefaultStatType.MOVEMENT_SPEED).asStatReference.GetStatFiltered();
            Assert.AreEqual(expectedUnitSpeed, unitSpeed);

            unit.InteractWithStat(StatFactory.NewStatInteractionObject(
                EStatInteractionIntent.ADD_STAT_FILTER,
                EDefaultStatType.MOVEMENT_SPEED,
                EDefaultStatInteractionType.NORMAL,
                new IntFilterSequence.ConstantFilter(slowSpell, -50)
                )
            );
            expectedUnitSpeed = 49;
            unitSpeed = unit.GetStat(EDefaultStatType.MOVEMENT_SPEED).asStatReference.GetStatFiltered();
            Assert.AreEqual(expectedUnitSpeed, unitSpeed);

            // Can Remove filters
            unit.InteractWithStat(StatFactory.NewStatInteractionObject(
                EStatInteractionIntent.REMOVE_STAT_FILTER,
                EDefaultStatType.MOVEMENT_SPEED,
                EDefaultStatInteractionType.NORMAL,
                quicksand
                )
            );
            expectedUnitSpeed = 55;
            unitSpeed = unit.GetStat(EDefaultStatType.MOVEMENT_SPEED).asStatReference.GetStatFiltered();
            Assert.AreEqual(expectedUnitSpeed, unitSpeed);

            unit.InteractWithStat(StatFactory.NewStatInteractionObject(
                EStatInteractionIntent.REMOVE_STAT_FILTER,
                EDefaultStatType.MOVEMENT_SPEED,
                EDefaultStatInteractionType.NORMAL,
                bootsOfSpeed
                )
            );
            expectedUnitSpeed = 50;
            unitSpeed = unit.GetStat(EDefaultStatType.MOVEMENT_SPEED).asStatReference.GetStatFiltered();
            Assert.AreEqual(expectedUnitSpeed, unitSpeed);

            unit.InteractWithStat(StatFactory.NewStatInteractionObject(
                EStatInteractionIntent.REMOVE_STAT_FILTER,
                EDefaultStatType.MOVEMENT_SPEED,
                EDefaultStatInteractionType.NORMAL,
                slowSpell
                )
            );
            expectedUnitSpeed = 100;
            unitSpeed = unit.GetStat(EDefaultStatType.MOVEMENT_SPEED).asStatReference.GetStatFiltered();
            Assert.AreEqual(expectedUnitSpeed, unitSpeed);

        }
    }
}