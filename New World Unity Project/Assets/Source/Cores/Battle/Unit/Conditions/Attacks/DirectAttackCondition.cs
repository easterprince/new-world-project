using NewWorld.Cores.Battle.Unit.Body;
using NewWorld.Cores.Battle.Unit.Durability;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Conditions.Attacks {

    public class DirectAttackCondition :
        ConditionModuleBase<DirectAttackCondition, AttackConditionPresentation>, IAttackConditionPresentation {

        // Fields.

        // Meta.
        private readonly NamedId id;

        // Attack properties.
        private readonly UnitPresentation target;
        private readonly Damage singleAttackDamage;
        private readonly float attackDuration;
        private readonly float attackMoment;
        private readonly float attackRange;

        // Progress.
        private float accumulatedTime;
        private bool attacked;
        private bool atStart;


        // Constructors.

        public DirectAttackCondition(
            UnitPresentation target, Damage singleAttackDamage,
            float attackDuration, float attackMoment, float attackRange,
            NamedId id) {

            // Meta.
            this.id = id;

            // Attack properties.
            this.target = target ?? throw new ArgumentNullException(nameof(target));
            this.singleAttackDamage = singleAttackDamage;
            this.attackDuration = Floats.SetPositive(attackDuration);
            this.attackMoment = Floats.LimitPositive(attackMoment, this.attackDuration);
            this.attackRange = Floats.SetPositive(attackRange);

            // Progress.
            accumulatedTime = 0f;
            attacked = false;
            atStart = true;

        }

        public DirectAttackCondition(DirectAttackCondition other) {

            // Meta.
            id = other.id;

            // Properties.
            target = other.target;
            singleAttackDamage = other.singleAttackDamage;
            attackDuration = other.attackDuration;
            attackMoment = other.attackMoment;
            attackRange = other.attackRange;

            // Progress.
            accumulatedTime = other.accumulatedTime;
            attacked = other.attacked;
            atStart = other.atStart;

        }


        // Properties.

        // Meta.
        public override NamedId Id => id;

        // Attack properties.
        public UnitPresentation Target => target;
        public Damage SingleAttackDamage => singleAttackDamage;
        public float AttackDuration => attackDuration;
        public float AttackMoment => attackMoment;
        public float AttackRange => attackRange;
        public Damage DamagePerSecond {
            get {
                if (singleAttackDamage.IsZero || float.IsPositiveInfinity(attackMoment)) {
                    return Damage.Zero;
                }
                if (float.IsPositiveInfinity(attackDuration)) {
                    return attacked ? Damage.Zero : (singleAttackDamage / attackMoment);
                }
                return singleAttackDamage / attackDuration;
            }
        }

        // General condition properties.
        public override bool Cancellable => !attacked;
        public override float ConditionSpeed => 1 / attackDuration;


        // Cloning.

        public override DirectAttackCondition Clone() {
            return new DirectAttackCondition(this);
        }


        // Presentation generation.

        private protected override AttackConditionPresentation BuildPresentation() {
            return new AttackConditionPresentation(this);
        }


        // Updating.

        private protected override void OnAct(out bool finished) {
            ValidateContext();

            // Check if finished.
            if (target is null || target.Durability.Fallen) {
                finished = true;
                return;
            }
            finished = false;

            // Update time.
            if (Context.GameTimeDelta == 0) {
                return;
            }
            accumulatedTime += Context.GameTimeDelta;

            // Update rotation.
            UpdateRotation();

            // Process continuous attack.
            if (attackDuration == 0) {
                accumulatedTime = 0;
                attacked = false;
                atStart = true;
                if (!TryAttack(float.PositiveInfinity)) {
                    finished = true;
                }
                return;
            }

            // Process usual attack.
            float attackCount = 0;
            if (!attacked && accumulatedTime >= attackMoment) {
                attacked = true;
                attackCount += 1;
            }
            if (accumulatedTime >= attackDuration) {
                atStart = true;
                float finishCount = Mathf.Floor(accumulatedTime / attackDuration);
                if (finishCount > 1) {
                    attackCount += finishCount - 1;
                }
                accumulatedTime = Floats.LimitPositive(accumulatedTime - finishCount * attackDuration, attackDuration);
                attacked = (accumulatedTime >= attackMoment);
                if (attacked) {
                    attackCount += 1;
                }
            } else {
                atStart = false;
            }

            // Attack.
            if (attackCount > 0) {
                if (!TryAttack(attackCount)) {
                    finished = true;
                }
            }

        }


        // Support.

        private void UpdateRotation() {
            Vector3 ownerPosition = Owner.Body.Position;
            Vector3 targetPosition = Target.Body.Position;
            Vector3 look = targetPosition - ownerPosition;
            if (look != Vector3.zero) {
                Quaternion rotation = Quaternion.LookRotation(targetPosition - ownerPosition);
                Owner.PlanAction(new RotationAction(rotation));
            }
        }

        private bool TryAttack(float factor) {
            Vector3 ownerPosition = Owner.Body.Position;
            Vector3 targetPosition = Target.Body.Position;
            if ((ownerPosition - targetPosition).magnitude > attackRange) {
                return false;
            }
            target.PlanAction(new DamageCausingAction(factor * singleAttackDamage));
            return true;
        }


    }

}
