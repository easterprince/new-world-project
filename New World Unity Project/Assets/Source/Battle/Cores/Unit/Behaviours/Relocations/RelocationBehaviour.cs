using NewWorld.Battle.Cores.Unit.Abilities.Motions;
using NewWorld.Battle.Cores.Unit.Conditions.Motions;
using NewWorld.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Behaviours.Relocations {

    public class RelocationBehaviour : BehaviourModuleBase<RelocationBehaviour, RelocationPresentation, RelocationGoal> {

        // Fields.

        private Stack<Vector2Int> plannedPath;


        // Cloning.

        private protected override RelocationBehaviour ClonePartially() {
            return new RelocationBehaviour();
        }


        // Presentation generation.

        private protected override RelocationPresentation BuildPresentation() {
            return new RelocationPresentation(this);
        }


        // Acting.

        private protected override void OnAct(out GoalStatus goalStatus) {
            ValidateContext();
            var owner = Owner;
            var context = Context;

            // Check if destination is reached.
            var currentPosition = owner.Body.Position;
            if ((currentPosition - Goal.Destination).magnitude <= Goal.AdmissibleDistance) {
                goalStatus = GoalStatus.Achieved;
                return;
            }

            // Choose motion ability.
            var abilityCollection = owner.AbilityCollection;
            MotionAbilityPresentation chosenAbility = null;
            foreach (var motionAbility in abilityCollection.Motions) {
                if (chosenAbility == null || motionAbility.MovementPerSecond > chosenAbility.MovementPerSecond) {
                    chosenAbility = motionAbility;
                }
            }

            // Check if unit is in motion.
            MotionConditionPresentation currentCondition = owner.Condition as MotionConditionPresentation;

            // Check if destination node is reached.
            var currentNode = context.UnitSystem[owner];
            var destinationNode = Positions.WorldToNode(Goal.Destination);
            if (currentNode == destinationNode) {
                
                // Use motion ability if condition doesn't agree with plan.
                if (currentCondition != null && (currentCondition.Destination - Goal.Destination).magnitude <= Goal.AdmissibleDistance) {
                    goalStatus = GoalStatus.Active;
                    return;
                }
                if (chosenAbility == null) {
                    goalStatus = GoalStatus.Impossible;
                    return;
                }
                var abilityUsage = new MotionUsageAction(chosenAbility, Goal.Destination);
                owner.PlanAction(abilityUsage);
                goalStatus = GoalStatus.Active;
                return;

            } else {

                // Plan path.
                if (plannedPath == null) {
                    var path = context.Layout.TryFindShortestPath(currentNode, destinationNode);
                    if (path == null) {
                        goalStatus = GoalStatus.Impossible;
                        return;
                    }
                    path.Reverse();
                    plannedPath = new Stack<Vector2Int>(path);
                }

                // Check if next path node is reached.
                var nextNode = plannedPath.Peek();
                if (currentNode == nextNode) {
                    plannedPath.Pop();
                    nextNode = plannedPath.Peek();
                }

                // Use motion ability if condition doesn't agree with plan.
                var nextPosition = Positions.NodeToWorld(nextNode, context.Map[nextNode].Height);
                if (currentCondition != null && (currentCondition.Destination - nextPosition).magnitude <= 0.1f) {
                    goalStatus = GoalStatus.Active;
                    return;
                }
                if (chosenAbility == null) {
                    goalStatus = GoalStatus.Impossible;
                    return;
                }
                var abilityUsage = new MotionUsageAction(chosenAbility, nextPosition);
                owner.PlanAction(abilityUsage);
                goalStatus = GoalStatus.Active;
                return;

            }

        }


    }

}
