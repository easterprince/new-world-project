using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.Unit.Durability;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Attacks {
    
    public class DirectAttackCondition : AttackCondition {

        // Fields.

        // Target.
        private readonly UnitPresentation target; 

        // Properties.
        private readonly Damage singleAttackDamage;
        private readonly float attackDuration;
        private readonly float attackMoment;
        private readonly float attackRange;

        // Time measurement.
        private float accumulatedTime;
        private bool attacked;


        // Constructors.

        public DirectAttackCondition(
            UnitPresentation target, Damage singleAttackDamage,
            float attackDuration, float attackMoment, float attackRange) {

            this.target = target ?? throw new ArgumentNullException(nameof(target)); 
            this.singleAttackDamage = singleAttackDamage;
            this.attackDuration = Mathf.Max(attackDuration, 0);
            this.attackMoment = Mathf.Clamp(attackMoment, 0, attackDuration);
            this.attackRange = Math.Max(attackRange, 0);
            
            accumulatedTime = 0f;
            attacked = false;
        }

        public DirectAttackCondition(DirectAttackCondition other) {
            target = other.target;
            singleAttackDamage = other.singleAttackDamage;
            attackDuration = other.attackDuration;
            attackMoment = other.attackMoment;
            attackRange = other.attackRange;
            accumulatedTime = other.accumulatedTime;
            attacked = other.attacked;
        }


        // Properties.

        public override UnitPresentation Target => target;
        public Damage SingleAttackDamage => singleAttackDamage;
        public float AttackDuration => attackDuration;
        public float AttackMoment => attackMoment;
        public float AttackRange => attackRange;
        public override Damage DamagePerSecond => singleAttackDamage / attackDuration;


        public override bool Cancellable => !attacked;

        public override string Description => $"Attacking target {target.Name}.";


        // Cloning.

        public override AttackCondition Clone() {
            return new DirectAttackCondition(this);
        }


        // Updating.


        private protected override void OnAct(out bool finished) {
            ValidateContext();
            if (target is null || target.Durability.Fallen) {
                finished = true;
                return;
            }
            finished = false;
            UpdateRotation();
            accumulatedTime += Context.GameTimeDelta;
            while (true) {
                if (!attacked) {
                    if (accumulatedTime >= attackMoment) {
                        attacked = true;
                        if (!TryAttack()) {
                            finished = true;
                        }
                    } else {
                        break;
                    }
                } else {
                    if (accumulatedTime >= attackDuration) {
                        attacked = false;
                        accumulatedTime -= attackDuration;
                    } else {
                        break;
                    }
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

        private bool TryAttack() {
            Vector3 ownerPosition = Owner.Body.Position;
            Vector3 targetPosition = Target.Body.Position;
            if ((ownerPosition - targetPosition).magnitude > attackRange) {
                return false;
            }
            target.PlanAction(new DamageCausingAction(singleAttackDamage));
            return true;
        }


    }

}
