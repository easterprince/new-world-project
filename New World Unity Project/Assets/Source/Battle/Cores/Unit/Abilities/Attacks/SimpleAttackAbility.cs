using NewWorld.Battle.Cores.Unit.Durability;
using System;

namespace NewWorld.Battle.Cores.Unit.Abilities.Attacks {

    public class SimpleAttackAbility : AttackAbility {

        // Fields.

        private Damage singleAttackDamage;
        private float attackDuration;
        private float attackMoment;
        private float attackRange;


        // Constructors.

        public SimpleAttackAbility() {
            singleAttackDamage = new Damage();
            attackDuration = 1f;
            attackMoment = 1f;
            attackRange = 1f;
        }

        public SimpleAttackAbility(SimpleAttackAbility other) {
            singleAttackDamage = other.singleAttackDamage;
            attackDuration = other.attackDuration;
            attackMoment = other.attackMoment;
            attackRange = other.attackRange;
        }


        // Properties.

        public Damage SingleAttackDamage {
            get => singleAttackDamage;
            set => singleAttackDamage = value;
        }

        public float AttackDuration {
            get => attackDuration;
            set => attackDuration = Math.Max(0, value);
        }

        public float AttackMoment {
            get => attackMoment;
            set => attackMoment = Math.Max(0, value);
        }

        public float AttackRange {
            get => attackRange;
            set => attackRange = Math.Max(0, value);
        }

        public override Damage DamagePerSecond => singleAttackDamage / attackDuration;

        public override string Description => "Attack target.";


        // Cloning.

        public override AttackAbility Clone() {
            return new SimpleAttackAbility(this);
        }


        // Udage.

        public override bool CheckIfUsable(UnitPresentation target) {
            ValidateContext();
            throw new NotImplementedException();
        }

        public override void Use(UnitPresentation target) {
            ValidateContext();
            throw new NotImplementedException();
        }


    }

}
