using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Behaviours;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;

namespace NewWorld.Battlefield.Units {

    public partial class UnitController {

        // Actions processing.
        // Note: method have to return true if action should not be sent to UnitSystemController to be processed, and false otherwise.

        private bool ProcessUnitUpdate(UnitUpdate unitUpdate) {
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
            return false;
        }

        private bool ProcessUnitUpdate(TransformUpdate transformUpdate) {
            if (transformUpdate.NewPosition != null) {
                transform.position = transformUpdate.NewPosition.Value;
            }
            if (transformUpdate.NewRotation != null) {
                transform.rotation = transformUpdate.NewRotation.Value;
            }
            return true;
        }

        private bool ProcessUnitUpdate(AnimatorParameterUpdate<float> animatorParameterUpdate) {
            animator.SetFloat(animatorParameterUpdate.AnimationParameterHash, animatorParameterUpdate.NewValue);
            return true;
        }

        private bool ProcessUnitUpdate(AnimatorTriggerApplication animatorTriggerApplication) {
            animator.SetTrigger(animatorTriggerApplication.AnimationTriggerHash);
            return true;
        }

        private bool ProcessUnitUpdate(AbilityUsage abilityUsage) {
            if (HasAbility(abilityUsage.Ability)) {
                abilityUsage.Ability.Use(abilityUsage.ParameterSet);
            }
            return true;
        }


    }

}