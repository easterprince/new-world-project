using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.General;

namespace NewWorld.Battlefield.Units {

    public partial class UnitController {

        // Actions processing.
        // Note: method have to return true if action has been processed, and false otherwise.

        private void ProcessGameAction(GameAction gameAction, bool external) {
            if (gameAction == null) {
                throw new System.ArgumentNullException(nameof(gameAction));
            }
            bool processed = false;
            
            if (gameAction is UnitUpdate unitUpdate && unitUpdate.Unit == this) {
                if (unitUpdate is GeneralUnitUpdate generalUnitUpdate) {
                    processed = ProcessGeneralUnitUpdate(generalUnitUpdate);
                } else if (unitUpdate is InternalUnitUpdate internalUnitUpdate) {
                    if (!external) {
                        processed = ProcessInternalUnitUpdate(internalUnitUpdate);
                    }
                }
            } else if (gameAction is UnitSystemUpdate unitSystemUpdate) {
                if (!external) {
                    processed = ProcessUnitSystemUpdate(unitSystemUpdate);
                }
            } else if (!external) {
                actionsToReturn.Add(gameAction);
                processed = true;
            }

            if (!processed) {
                Debug.LogWarning($"{(external ? "External" : "Internal")} action of type {gameAction.GetType()} was not processed!", this);
            }
        }

        private void ProcessGameActions(IEnumerable<GameAction> gameActions, bool external) {
            foreach (var gameAction in gameActions) {
                ProcessGameAction(gameAction, external);
            }
        }

        public void ProcessGameAction(GameAction gameAction) {
            if (this == null) {
                return;
            }
            ProcessGameAction(gameAction, true);
        }

        public void ProcessGameActions(IEnumerable<GameAction> gameActions) {
            if (this == null) {
                return;
            }
            foreach (var gameAction in gameActions) {
                ProcessGameAction(gameAction, true);
            }
        }


        // General unit updates.

        private bool ProcessGeneralUnitUpdate(GeneralUnitUpdate generalUnitUpdate) {
            if (generalUnitUpdate is CauseDamage causeDamage) {
                return ProcessGeneralUnitUpdate(causeDamage);
            }
            if (generalUnitUpdate is StopCondition stopCondition) {
                return ProcessGeneralUnitUpdate(stopCondition);
            }
            if (generalUnitUpdate is ForceCondition forceCondition) {
                return ProcessGeneralUnitUpdate(forceCondition);
            }
            if (generalUnitUpdate is UseAbility useAbility) {
                return ProcessGeneralUnitUpdate(useAbility);
            }
            return false;
        }

        private bool ProcessGeneralUnitUpdate(CauseDamage causeDamage) {
            if (durability != null) {
                durability.TakeDamage(causeDamage.DamageValue);
            }
            return true;
        }

        private bool ProcessGeneralUnitUpdate(StopCondition stopCondition) {
            if (stopCondition.Condition == currentCondition) {
                var actions = currentCondition.Stop(stopCondition.ForceStop);
                if (currentCondition.Exited) {
                    currentCondition = null;
                }
                ProcessGameActions(actions, false);
            }
            return true;
        }

        private bool ProcessGeneralUnitUpdate(ForceCondition forceCondition) {
            if (currentCondition != null) {
                var stopCondition = new StopCondition(currentCondition, true);
                ProcessGeneralUnitUpdate(stopCondition);
            }
            currentCondition = forceCondition.Condition;
            var actions = currentCondition.Enter();
            ProcessGameActions(actions, false);
            return true;
        }

        private bool ProcessGeneralUnitUpdate(UseAbility useAbility) {
            if (HasAbility(useAbility.Ability)) {
                if (currentCondition != null) {
                    var cancelCondition = new CancelCondition(currentCondition);
                    ProcessGeneralUnitUpdate(cancelCondition);
                }
                if (currentCondition == null) {
                    var newCondition = useAbility.Ability.Use(useAbility.ParameterSet);
                    var forceCondition = new ForceCondition(newCondition);
                    ProcessGeneralUnitUpdate(forceCondition);
                }
            }
            return true;
        }


        // Internal unit updates.

        private bool ProcessInternalUnitUpdate(InternalUnitUpdate internalUnitUpdate) {
            if (internalUnitUpdate is MoveUnit moveUnit) {
                return ProcessInternalUnitUpdate(moveUnit);
            }
            if (internalUnitUpdate is UpdateAnimatorParameter<float> updateAnimatorParameter) {
                return ProcessInternalUnitUpdate(updateAnimatorParameter);
            }
            if (internalUnitUpdate is ApplyAnimatorTrigger applyAnimatorTrigger) {
                return ProcessInternalUnitUpdate(applyAnimatorTrigger);
            }
            return false;
        }

        private bool ProcessInternalUnitUpdate(MoveUnit moveUnit) {
            Vector3 positionChange = Vector3.zero;
            if (moveUnit.PositionChange != null) {
                Vector2 newPosition2D = new Vector2(transform.position.x, transform.position.z) + moveUnit.PositionChange.Value;
                float newY = MapController.Instance.GetSurfaceHeight(newPosition2D);
                Vector3 newPosition = new Vector3(newPosition2D.x, newY, newPosition2D.y);
                positionChange = newPosition - transform.position;
                transform.position = newPosition;
            }
            if (moveUnit.RotationFromForward != null) {
                Quaternion newRotation = transform.rotation;
                if (positionChange != Vector3.zero) {
                    newRotation = Quaternion.LookRotation(positionChange);
                }
                newRotation *= moveUnit.RotationFromForward.Value;
                transform.rotation = newRotation;
            }
            return true;
        }

        private bool ProcessInternalUnitUpdate(UpdateAnimatorParameter<float> animatorParameterUpdate) {
            animator.SetFloat(animatorParameterUpdate.AnimationParameterHash, animatorParameterUpdate.NewValue);
            return true;
        }

        private bool ProcessInternalUnitUpdate(ApplyAnimatorTrigger animatorTriggerApplication) {
            animator.SetTrigger(animatorTriggerApplication.AnimationTriggerHash);
            return true;
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            if (unitSystemUpdate is UpdateConnectedNode updateConnectedNode) {
                return ProcessUnitSystemUpdate(updateConnectedNode);
            }
            return false;
        }

        private bool ProcessUnitSystemUpdate(UpdateConnectedNode updateConnectedNode) {
            actionsToReturn.Add(updateConnectedNode);
            return true;
        }


    }

}