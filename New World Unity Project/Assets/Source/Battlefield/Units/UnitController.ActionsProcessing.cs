using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;

namespace NewWorld.Battlefield.Units {

    public partial class UnitController {

        // Actions processing.
        // Note: method have to return true if action should not be sent to UnitSystemController to be processed, and false otherwise.

        private bool ProcessGameAction(GameAction gameAction) {
            if (gameAction == null) {
                throw new System.ArgumentNullException(nameof(gameAction));
            }
            if (gameAction is UnitUpdate unitUpdate && unitUpdate.UpdatedUnit == this) {
                return ProcessUnitUpdate(unitUpdate);
            }
            if (gameAction is UnitSystemUpdate unitSystemUpdate && unitSystemUpdate.UpdatedUnit == this) {
                return ProcessUnitSystemUpdate(unitSystemUpdate);
            }
            return false;
        }


        // Unit updates.

        private bool ProcessUnitUpdate(UnitUpdate unitUpdate) {
            if (unitUpdate == null || unitUpdate.UpdatedUnit != this) {
                throw new System.ArgumentNullException(nameof(unitUpdate));
            }
            if (unitUpdate is TransformUpdate transformUpdate) {
                return ProcessUnitUpdate(transformUpdate);
            }
            if (unitUpdate is AnimatorParameterUpdate<float> animatorParameterUpdate) {
                return ProcessUnitUpdate(animatorParameterUpdate);
            }
            if (unitUpdate is AnimatorTriggerApplication animatorTriggerApplication) {
                return ProcessUnitUpdate(animatorTriggerApplication);
            }
            if (unitUpdate is AbilityUsage abilityUsage) {
                return ProcessUnitUpdate(abilityUsage);
            }
            if (unitUpdate is AbilityStop abilityStop) {
                return ProcessUnitUpdate(abilityStop);
            }
            if (unitUpdate is DamageCausing damageCausing) {
                return ProcessUnitUpdate(damageCausing);
            }
            return false;
        }

        private bool ProcessUnitUpdate(TransformUpdate transformUpdate) {
            if (transformUpdate == null || transformUpdate.UpdatedUnit != this) {
                throw new System.ArgumentNullException(nameof(transformUpdate));
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

        private bool ProcessUnitUpdate(AnimatorParameterUpdate<float> animatorParameterUpdate) {
            if (animatorParameterUpdate == null || animatorParameterUpdate.UpdatedUnit != this) {
                throw new System.ArgumentNullException(nameof(animatorParameterUpdate));
            }
            animator.SetFloat(animatorParameterUpdate.AnimationParameterHash, animatorParameterUpdate.NewValue);
            return true;
        }

        private bool ProcessUnitUpdate(AnimatorTriggerApplication animatorTriggerApplication) {
            if (animatorTriggerApplication == null || animatorTriggerApplication.UpdatedUnit != this) {
                throw new System.ArgumentNullException(nameof(animatorTriggerApplication));
            }
            animator.SetTrigger(animatorTriggerApplication.AnimationTriggerHash);
            return true;
        }

        private bool ProcessUnitUpdate(AbilityUsage abilityUsage) {
            if (abilityUsage == null || abilityUsage.UpdatedUnit != this) {
                throw new System.ArgumentNullException(nameof(abilityUsage));
            }
            if (plannedAbilityUsage == null || plannedAbilityUsage.Ability == usedAbility) {
                plannedAbilityUsage = abilityUsage;
            }
            return true;
        }

        private bool ProcessUnitUpdate(AbilityStop abilityStop) {
            if (abilityStop == null || abilityStop.UpdatedUnit != this) {
                throw new System.ArgumentNullException(nameof(abilityStop));
            }
            if (plannedAbilityStop == null || plannedAbilityStop.Ability != usedAbility || !plannedAbilityStop.ForceStop && abilityStop.ForceStop) {
                plannedAbilityStop = abilityStop;
            }
            return true;
        }

        private bool ProcessUnitUpdate(DamageCausing damageCausing) {
            if (damageCausing == null || damageCausing.UpdatedUnit != this) {
                throw new System.ArgumentNullException(nameof(damageCausing));
            }
            if (durability != null) {
                durability.TakeDamage(damageCausing);
            }
            return true;
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            if (unitSystemUpdate == null || unitSystemUpdate.UpdatedUnit != this) {
                throw new System.ArgumentNullException(nameof(unitSystemUpdate));
            }
            if (unitSystemUpdate is ConnectedNodeUpdate connectedNodeUpdate) {
                return ProcessUnitSystemUpdate(connectedNodeUpdate);
            }
            return false;
        }

        private bool ProcessUnitSystemUpdate(ConnectedNodeUpdate connectedNodeUpdate) {
            if (connectedNodeUpdate == null || connectedNodeUpdate.UpdatedUnit != this) {
                throw new System.ArgumentNullException(nameof(connectedNodeUpdate));
            }
            Vector2Int connectedNode = UnitSystemController.Instance.GetConnectedNode(this);
            if (PositionIsAllowed(transform.position, connectedNode)) {
                return false;
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