﻿using NewWorld.Cores.Battle.Unit.Abilities.Motions;
using NewWorld.Cores.Battle.Unit.Conditions.Motions;
using NewWorld.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Behaviours.Relocations {

    public class RelocationBehaviour : BehaviourBase<RelocationGoal> {

        // Fields.

        private Stack<Vector2Int> plannedPath;


        // Constructor.

        public RelocationBehaviour(RelocationGoal goal, IOwnerPointer ownerPointer) : base(goal, ownerPointer) {}


        // Acting.

        private protected override void OnAct(out GoalStatus goalStatus) {
            var owner = Owner;
            var context = Context;
            if (owner is null || context is null) {
                goalStatus = GoalStatus.Active;
                return;
            }

            // Check if destination is reached.
            var currentPosition = owner.Body.Position;
            if ((currentPosition - Goal.Destination).magnitude <= Goal.AdmissibleDistance) {
                goalStatus = GoalStatus.Achieved;
                return;
            }

            // Choose motion ability.
            var abilityCollection = owner.AbilityCollection;
            IMotionAbilityPresentation chosenAbility = null;
            foreach (var motionAbility in abilityCollection.Motions) {
                if (chosenAbility == null || motionAbility.MovementPerSecond > chosenAbility.MovementPerSecond) {
                    chosenAbility = motionAbility;
                }
            }

            // Check if unit is in motion.
            IMotionConditionPresentation currentCondition = owner.Condition as IMotionConditionPresentation;

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
                    goalStatus = GoalStatus.Failed;
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
                        goalStatus = GoalStatus.Failed;
                        return;
                    }
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
                    goalStatus = GoalStatus.Failed;
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
