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
            if (unitUpdate == null) {
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
            return false;
        }

        private bool ProcessUnitUpdate(TransformUpdate transformUpdate) {
            if (transformUpdate == null) {
                throw new System.ArgumentNullException(nameof(transformUpdate));
            }
            if (transformUpdate.NewPosition != null) {
                transform.position = transformUpdate.NewPosition.Value;
            }
            if (transformUpdate.NewRotation != null) {
                transform.rotation = transformUpdate.NewRotation.Value;
            }
            return true;
        }

        private bool ProcessUnitUpdate(AnimatorParameterUpdate<float> animatorParameterUpdate) {
            if (animatorParameterUpdate == null) {
                throw new System.ArgumentNullException(nameof(animatorParameterUpdate));
            }
            animator.SetFloat(animatorParameterUpdate.AnimationParameterHash, animatorParameterUpdate.NewValue);
            return true;
        }

        private bool ProcessUnitUpdate(AnimatorTriggerApplication animatorTriggerApplication) {
            if (animatorTriggerApplication == null) {
                throw new System.ArgumentNullException(nameof(animatorTriggerApplication));
            }
            animator.SetTrigger(animatorTriggerApplication.AnimationTriggerHash);
            return true;
        }

        private bool ProcessUnitUpdate(AbilityUsage abilityUsage) {
            if (abilityUsage == null) {
                throw new System.ArgumentNullException(nameof(abilityUsage));
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
            if (plannedAbilityStop == null || plannedAbilityStop.Ability != usedAbility || !plannedAbilityStop.ForceStop && abilityStop.ForceStop) {
                plannedAbilityStop = abilityStop;
            }
            return true;
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            if (unitSystemUpdate == null) {
                throw new System.ArgumentNullException(nameof(unitSystemUpdate));
            }
            if (unitSystemUpdate is ConnectedNodeUpdate connectedNodeUpdate) {
                ProcessUnitSystemUpdate(connectedNodeUpdate);
            }
            return false;
        }

        private bool ProcessUnitSystemUpdate(ConnectedNodeUpdate connectedNodeUpdate) {
            if (connectedNodeUpdate == null) {
                throw new System.ArgumentNullException(nameof(connectedNodeUpdate));
            }
            return false;
        }


    }

}