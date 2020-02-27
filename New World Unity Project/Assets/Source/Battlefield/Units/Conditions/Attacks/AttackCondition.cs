using UnityEngine;

namespace NewWorld.Battlefield.Units.Conditions.Attacks {

    public abstract class AttackCondition : Condition<AttackConditionPresentation> {

        // Fields.

        private readonly UnitController target;
        private readonly float attackPower;
        private readonly float attackSpeed;
        private readonly float attackTime;


        // Properties.

        protected UnitController Target => target;
        protected float AttackPower => attackPower;
        protected float AttackSpeed => attackSpeed;
        protected float AttackTime => attackTime;


        // To string conversion.

        override public string ToString() {
            return $"Attacking traget {target.name}";
        }


        // Constructors.

        public AttackCondition(
            UnitController target,
            float attackPower = 1, float attackSpeed = 1, float attackTime = 0.5f
        ) : base() {
            this.target = target;
            this.attackPower = Mathf.Max(0, attackPower);
            this.attackSpeed = Mathf.Max(0, attackSpeed);
            this.attackTime = Mathf.Clamp(attackTime, 0f, 1f);
            Presentation = new AttackConditionPresentation(this);
        }


    }

}
