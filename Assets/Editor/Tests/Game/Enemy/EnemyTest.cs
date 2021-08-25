using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


namespace Tests {

    [TestFixture]
    public class EnemyTest {
        
        StatFactory<EGameStatTypes, EDefaultStatInteractionType> StatFactory = new StatFactory<EGameStatTypes, EDefaultStatInteractionType>();

        [Test]
        public void Enemy_Base_And_Starting_Health_Is_Correct() {
            const int BASE_HEALTH = 10;

            // Test setting
            Assert.AreEqual(BASE_HEALTH, EnemyMan.ENEMY_MAN_BASE_HEALTH);

            // Test instantiation
            EnemyMan enemy = new EnemyMan(StatFactory);
            int enemyBaseHealth = enemy.GetBaseHealth();
            Assert.AreEqual(BASE_HEALTH, enemyBaseHealth);

            int enemyHealth = enemy.GetCurrentHealth();
            Assert.AreEqual(BASE_HEALTH, enemyHealth);

        }

        [Test]
        public void Enemy_Takes_Damage_and_Heals_Damage() {
            const int BASE_HEALTH = 10;
            const int DAMAGE = 3;
            const int HEAL = 4;

            EnemyNormal enemy = new EnemyMan(StatFactory);

            // DAMAGING
            StatInteraction<EGameStatTypes, EDefaultStatInteractionType> damageInteraction = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_DEPLETE, EGameStatTypes.HEALTH, EDefaultStatInteractionType.NORMAL, 3);

            enemy.InteractWithStat(damageInteraction);
            int expectedHealth = BASE_HEALTH - DAMAGE;
            int actualHealth = enemy.GetStat(EGameStatTypes.HEALTH).asStatDepletion.GetRemainingStatValue();
            Assert.AreEqual(expectedHealth, actualHealth);

            enemy.InteractWithStat(damageInteraction);
            expectedHealth = BASE_HEALTH - DAMAGE * 2;
            actualHealth = enemy.GetCurrentHealth();
            Assert.AreEqual(expectedHealth, actualHealth);

            enemy.InteractWithStat(damageInteraction);
            expectedHealth = BASE_HEALTH - DAMAGE * 3;
            actualHealth = enemy.GetCurrentHealth();
            Assert.AreEqual(expectedHealth, actualHealth);

            enemy.InteractWithStat(damageInteraction);
            expectedHealth = 0; // Should not deplete past zero
            actualHealth = enemy.GetCurrentHealth();
            Assert.AreEqual(expectedHealth, actualHealth);

            // HEALING
            StatInteraction<EGameStatTypes, EDefaultStatInteractionType> healInteraction = StatFactory.NewStatInteractionObject(EStatInteractionIntent.STAT_GAIN, EGameStatTypes.HEALTH, EDefaultStatInteractionType.NORMAL, 4);

            enemy.InteractWithStat(healInteraction);
            expectedHealth = 0 + HEAL;
            actualHealth = enemy.GetCurrentHealth();
            Assert.AreEqual(expectedHealth, actualHealth);
        }

    }

}
