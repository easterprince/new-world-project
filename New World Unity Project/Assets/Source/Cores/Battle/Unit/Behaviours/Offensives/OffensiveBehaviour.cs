using NewWorld.Cores.Battle.Unit.Abilities.Attacks;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
using NewWorld.Cores.Battle.Unit.Conditions.Attacks;

namespace NewWorld.Cores.Battle.Unit.Behaviours.Offensives {

    public class OffensiveBehaviour : BehaviourBase<OffensiveGoal> {

        // Fields.

        private RelocationBehaviour relocation = null;


        // Constructor.

        public OffensiveBehaviour(OffensiveGoal goal, IOwnerPointer ownerPointer) : base(goal, ownerPointer) {}


        // Acting.

        private protected override void OnAct(out GoalStatus goalStatus) {
            var owner = Owner;
            var context = Context;
            if (owner is null || context is null) {
                goalStatus = GoalStatus.Active;
                return;
            }

            // Check if target is down.
            if (Goal.Target.Durability.Fallen) {
                goalStatus = GoalStatus.Achieved;
                return;
            }

            // Check if already attacking.
            if (owner.Condition is IAttackConditionPresentation currentAttack && currentAttack.Target == Goal.Target) {
                goalStatus = GoalStatus.Active;
                return;
            }

            // Choose attack ability.
            IAttackAbilityPresentation chosenAttack = null;
            foreach (var attackAbility in owner.AbilityCollection.Attacks) {
                if (chosenAttack == null || attackAbility.DamagePerSecond.DamageValue > chosenAttack.DamagePerSecond.DamageValue) {
                    chosenAttack = attackAbility;
                }
            }
            if (chosenAttack == null) {
                goalStatus = GoalStatus.Failed;
                return;
            }

            // Check if target is reached.
            var currentPosition = owner.Body.Position;
            var targetPosition = Goal.Target.Body.Position;
            if ((currentPosition - targetPosition).magnitude <= chosenAttack.AttackRange) {
                var abilityUsage = new AttackUsageAction(chosenAttack, Goal.Target);
                owner.PlanAction(abilityUsage);
                goalStatus = GoalStatus.Active;
                return;
            }

            // Reach target.
            if (relocation == null) {
                var goal = new RelocationGoal(targetPosition);
                relocation = new RelocationBehaviour(goal, this);
            }
            relocation.Act(out GoalStatus relocationStatus);
            if (relocationStatus == GoalStatus.Failed) {
                goalStatus = GoalStatus.Failed;
                return;
            }
            if (relocationStatus == GoalStatus.Achieved) {
                relocation = null;
            }

            goalStatus = GoalStatus.Active;
        }


    }

}
