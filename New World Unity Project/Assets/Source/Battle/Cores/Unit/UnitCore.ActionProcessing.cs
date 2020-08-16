using NewWorld.Battle.Cores.Unit.Abilities;
using NewWorld.Battle.Cores.Unit.Abilities.Attacks;
using NewWorld.Battle.Cores.Unit.Abilities.Motions;
using NewWorld.Battle.Cores.Unit.Behaviours;
using NewWorld.Battle.Cores.Unit.Behaviours.Relocations;
using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Durability;
using System;

namespace NewWorld.Battle.Cores.Unit {
    
    public partial class UnitCore :
        IResponsive<ConditionCausingAction>, IResponsive<ConditionCancellingAction>, IResponsive<DamageCausingAction>,
        IResponsive<MovementAction>, IResponsive<RotationAction>,
        IResponsive<AttackUsageAction>, IResponsive<MotionUsageAction>,
        IResponsive<GoalSettingAction<RelocationGoal>> {

        // Action processing.

        public void ProcessAction(ConditionCausingAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            CauseCondition(action.Condition);
        }

        public void ProcessAction(DamageCausingAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            CauseDamage(action.Damage);
        }

        public void ProcessAction(MovementAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            Move(action);
        }

        public void ProcessAction(RotationAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            Rotate(action);
        }

        public void ProcessAction(ConditionCancellingAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            CancelCondition();
        }

        public void ProcessAction(AttackUsageAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            UseAbility(action);
        }

        public void ProcessAction(MotionUsageAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            UseAbility(action);
        }

        public void ProcessAction(GoalSettingAction<RelocationGoal> action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            SetGoal(action.Goal);
        }


        // Action planning.

        public void PlanAction(ConditionCausingAction action) => PlanAction(this, action);
        public void PlanAction(DamageCausingAction action) => PlanAction(this, action);
        public void PlanAction(MovementAction action) => PlanAction(this, action);
        public void PlanAction(RotationAction action) => PlanAction(this, action);
        public void PlanAction(ConditionCancellingAction action) => PlanAction(this, action);
        public void PlanAction(AttackUsageAction action) => PlanAction(this, action);
        public void PlanAction(MotionUsageAction action) => PlanAction(this, action);
        public void PlanAction(GoalSettingAction<RelocationGoal> action) => PlanAction(this, action);


    }

}
