using NewWorld.Cores.Battle.Unit.Abilities.Attacks;
using NewWorld.Cores.Battle.Unit.Abilities.Motions;
using NewWorld.Cores.Battle.Unit.Behaviours;
using NewWorld.Cores.Battle.Unit.Behaviours.Offensives;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
using NewWorld.Cores.Battle.Unit.Body;
using NewWorld.Cores.Battle.Unit.Conditions;
using NewWorld.Cores.Battle.Unit.Durability;
using System;

namespace NewWorld.Cores.Battle.Unit {

    public partial class UnitCore :
        IResponsive<ConditionChangingAction>, IResponsive<DamageCausingAction>,
        IResponsive<MovementAction>, IResponsive<RotationAction>,
        IResponsive<AttackUsageAction>, IResponsive<MotionUsageAction>,
        IResponsive<GoalSettingAction<RelocationGoal>>, IResponsive<GoalSettingAction<OffensiveGoal>>,
        IResponsive<GoalSettingAction<IdleGoal>> {

        // Action processing.

        public void ProcessAction(ConditionChangingAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            ChangeCondition(action.Condition, action.ForceChange);
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

        public void ProcessAction(GoalSettingAction<OffensiveGoal> action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            SetGoal(action.Goal);
        }

        public void ProcessAction(GoalSettingAction<IdleGoal> action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            SetGoal(action.Goal);
        }


        // Action planning.

        public void PlanAction(ConditionChangingAction action) => PlanAction(this, action);
        public void PlanAction(DamageCausingAction action) => PlanAction(this, action);
        public void PlanAction(MovementAction action) => PlanAction(this, action);
        public void PlanAction(RotationAction action) => PlanAction(this, action);
        public void PlanAction(AttackUsageAction action) => PlanAction(this, action);
        public void PlanAction(MotionUsageAction action) => PlanAction(this, action);
        public void PlanAction(GoalSettingAction<RelocationGoal> action) => PlanAction(this, action);
        public void PlanAction(GoalSettingAction<OffensiveGoal> action) => PlanAction(this, action);
        public void PlanAction(GoalSettingAction<IdleGoal> action) => PlanAction(this, action);


    }

}
