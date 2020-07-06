using UnityEngine;

namespace NewWorld.Battlefield.Unit.Conditions.Attacks {

    public abstract class AttackCondition : UnitCondition {

        // Fields.

        private readonly UnitController target;
        private readonly float attackPower;
        private readonly float attackSpeed;
        private readonly float attackTime;


        // Properties.

        public UnitController Target => target;
        public float AttackPower => attackPower;
        public float AttackSpeed => attackSpeed;
        public float AttackTime => attackTime;

        override public string Description => $"Attacking {target.name}";


        // Constructors.

        public AttackCondition(
            UnitController target,
            float attackPower = 1, float attackSpeed = 1, float attackTime = 0.5f
        ) : base() {
            this.target = target;
            this.attackPower = Mathf.Max(0, attackPower);
            this.attackSpeed = Mathf.Max(0, attackSpeed);
            this.attackTime = Mathf.Clamp(attackTime, 0f, 1f);
        }


    }

}
