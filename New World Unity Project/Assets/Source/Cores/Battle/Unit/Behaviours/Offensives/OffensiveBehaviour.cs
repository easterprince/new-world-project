using NewWorld.Cores.Battle.Unit.Abilities.Attacks;
using NewWorld.Cores.Battle.Unit.Abilities.Motions;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
using NewWorld.Cores.Battle.Unit.Conditions.Attacks;
using NewWorld.Cores.Battle.Unit.Conditions.Motions;
using NewWorld.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Behaviours.Offensives {

    public class OffensiveBehaviour : BehaviourModuleBase<OffensiveBehaviour, OffensivePresentation, OffensiveGoal> {

        // Fields.

        private RelocationBehaviour relocation = null;


        // Presentation generation.

        private protected override OffensivePresentation BuildPresentation() {
            return new OffensivePresentation(this);
        }


        // Cloning.

        private protected override OffensiveBehaviour ClonePartially() {
            return new OffensiveBehaviour();
        }


        // Acting.

        private protected override void OnAct(out GoalStatus goalStatus) {
            ValidateContext();
            var owner = Owner;
            var context = Context;

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
                goalStatus = GoalStatus.Impossible;
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
                relocation = new RelocationBehaviour();
                relocation.Connect(Presentation);
                relocation.Goal = new RelocationGoal(targetPosition);
            }
            relocation.Act(out GoalStatus relocationStatus);
            if (relocationStatus == GoalStatus.Impossible) {
                goalStatus = GoalStatus.Impossible;
                return;
            }
            if (relocationStatus == GoalStatus.Achieved) {
                relocation = null;
            }

            goalStatus = GoalStatus.Active;
        }


    }

}
