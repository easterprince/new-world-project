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

        public void ProcessGameAction(GameAction gameAction, bool external = true) {
            if (gameAction == null) {
                throw new System.ArgumentNullException(nameof(gameAction));
            }
            bool processed = false;
            
            if (gameAction is UnitUpdate unitUpdate && unitUpdate.Unit == this) {
                if (unitUpdate is GeneralUnitUpdate generalUnitUpdate) {
                    processed = ProcessGeneralUnitUpdate(unitSystemUpdate);
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
            }

            if (!processed) {
                Debug.LogWarning($"Action of type {gameAction.GetType()} was not processed!", this);
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
        }


        // Internal unit updates.

        private bool ProcessInternalUnitUpdate(InternalUnitUpdate internalUnitUpdate) {
            if (internalUnitUpdate is UpdateTransform transformUpdate) {
                return ProcessInternalUnitUpdate(transformUpdate);
            }
            if (internalUnitUpdate is UpdateAnimatorParameter<float> animatorParameterUpdate) {
                return ProcessInternalUnitUpdate(animatorParameterUpdate);
            }
            if (internalUnitUpdate is ApplyAnimatorTrigger animatorTriggerApplication) {
                return ProcessInternalUnitUpdate(animatorTriggerApplication);
            }
            return false;
        }

        private bool ProcessUnitUpdate(UpdateTransform transformUpdate) {
            if (transformUpdate == null) {
                throw new System.ArgumentNullException(nameof(transformUpdate));
            }
            if (transformUpdate.Unit != this) {
                return false;
            }
            if (transformUpdate.NewPosition != null) {
                Vector3 newPosition = transformUpdate.NewPosition.Value;
                Vector2Int connectedNode = UnitSystemController.Instance.GetConnectedNode(this);
                if (PositionIsAllowed(newPosition, connectedNode)) {
                    transform.position = newPosition;
                }
            }
            if (transformUpdate.NewRotation != null) {
                transform.rotation = transformUpdate.NewRotation.Value;
            }
            return true;
        }

        private bool ProcessUnitUpdate(UpdateAnimatorParameter<float> animatorParameterUpdate) {
            if (animatorParameterUpdate == null) {
                throw new System.ArgumentNullException(nameof(animatorParameterUpdate));
            }
            if (animatorParameterUpdate.Unit != this) {
                return false;
            }
            animator.SetFloat(animatorParameterUpdate.AnimationParameterHash, animatorParameterUpdate.NewValue);
            return true;
        }

        private bool ProcessUnitUpdate(ApplyAnimatorTrigger animatorTriggerApplication) {
            if (animatorTriggerApplication == null) {
                throw new System.ArgumentNullException(nameof(animatorTriggerApplication));
            }
            if (animatorTriggerApplication.Unit != this) {
                return false;
            }
            animator.SetTrigger(animatorTriggerApplication.AnimationTriggerHash);
            return true;
        }

        private bool ProcessUnitUpdate(AbilityUsage abilityUsage) {
            if (abilityUsage == null) {
                throw new System.ArgumentNullException(nameof(abilityUsage));
            }
            if (abilityUsage.UpdatedUnit != this) {
                return false;
            }
            if (plannedAbilityUsage == null || plannedAbilityUsage.Ability == usedAbility) {
                plannedAbilityUsage = abilityUsage;
            }
            return true;
        }

        private bool ProcessUnitUpdate(AbilityStop abilityStop) {
            if (abilityStop == null) {
                throw new System.ArgumentNullException(nameof(abilityStop));
            }
            if (abilityStop.UpdatedUnit != this) {
                return false;
            }
            if (plannedAbilityStop == null || plannedAbilityStop.Ability != usedAbility || !plannedAbilityStop.ForceStop && abilityStop.ForceStop) {
                plannedAbilityStop = abilityStop;
            }
            return true;
        }

        private bool ProcessUnitUpdate(DamageCausing damageCausing) {
            if (damageCausing == null) {
                throw new System.ArgumentNullException(nameof(damageCausing));
            }
            if (damageCausing.UpdatedUnit != this) {
                return false;
            }
            if (durability != null) {
                durability.TakeDamage(damageCausing);
            }
            return true;
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            if (unitSystemUpdate == null) {
                throw new System.ArgumentNullException(nameof(unitSystemUpdate));
            }
            if (unitSystemUpdate is UpdateConnectedNode connectedNodeUpdate) {
                return ProcessUnitSystemUpdate(connectedNodeUpdate);
            }
            return false;
        }

        private bool ProcessUnitSystemUpdate(UpdateConnectedNode connectedNodeUpdate) {
            if (connectedNodeUpdate == null) {
                throw new System.ArgumentNullException(nameof(connectedNodeUpdate));
            }
            if (connectedNodeUpdate.Unit != this) {
                return false;
            }
            Vector2Int connectedNode = UnitSystemController.Instance.GetConnectedNode(this);
            if (PositionIsAllowed(transform.position, connectedNode)) {
                actionsToReturn.Add(connectedNodeUpdate);
            }
            return true;
        }


        // Support methods.

        private static bool PositionIsAllowed(Vector3 position, Vector2Int connectedNode) {
            Vector2 position2D = new Vector2(position.x, position.z);
            return MaximumMetric.GetNorm(position2D - connectedNode) <= nodeDistanceLimit;
        }


    }

}