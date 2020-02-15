using UnityEngine;
using NewWorld.Battlefield.Units.Conditions;
using NewWorld.Battlefield.Units.Conditions.Attacks;

namespace NewWorld.Battlefield.Units.Abilities.Attacks {

    public class BasicAttack : AttackAbility {

        // Constructors.

        public BasicAttack(
            UnitController owner,
            float attackPower = 1, float attackSpeed = 1, float attackTime = 0.5f
        ) : base(owner, attackPower, attackSpeed, attackTime) {}


        // Methods.

        sealed override public Condition Use(object parameterSet) {
            if (!(parameterSet is UnitController target)) {
                throw new System.ArgumentException($"Parameter set must be of type {typeof(UnitController)}.");
            }
            var condition = new DirectAttack(Owner, target, AttackPower, AttackSpeed, AttackTime);
            return condition;
        }


    }

}
