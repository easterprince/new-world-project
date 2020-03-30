using UnityEngine;
using NewWorld.Battlefield.Unit.Conditions;
using NewWorld.Battlefield.Unit.Conditions.Attacks;

namespace NewWorld.Battlefield.Unit.Abilities.Attacks {

    public class BasicAttack : AttackAbility {

        // Constructors.

        public BasicAttack(float attackPower = 1, float attackSpeed = 1, float attackTime = 0.5f) : base(attackPower, attackSpeed, attackTime) {}


        // Methods.

        sealed override protected private UnitCondition MakeCondition(object parameterSet) {
            if (!(parameterSet is UnitController target)) {
                throw new System.ArgumentException($"Parameter set must be of type {typeof(UnitController)}.");
            }
            var condition = new DirectAttack(target, AttackPower, AttackSpeed, AttackTime);
            return condition;
        }


    }

}
