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

            if (gameAction is UnitUpdate unitUpdate) {
                if (unitUpdate.Unit == this) {
                    if (unitUpdate is GeneralUnitUpdate ownGeneralUnitUpdate) {
                        processed = ProcessOwnGeneralUnitUpdate(ownGeneralUnitUpdate);
                    } else if (unitUpdate is InternalUnitUpdate ownInternalUnitUpdate && !external) {
                        processed = ProcessOwnInternalUnitUpdate(ownInternalUnitUpdate);
                    }
                } else if (!external) {
                    unitUpdate.Unit.AddAction(unitUpdate);
                    processed = true;
                }
            } else if (gameAction is UnitSystemUpdate unitSystemUpdate && !external) {
                processed = ProcessUnitSystemUpdate(unitSystemUpdate);
            }

            if (!processed) {
                Debug.LogWarning($"{(external ? "External" : "Internal")} action of type {gameAction.GetType()} was not processed!", this);
            }
        }

        private void ProcessGameActions(IEnumerable<GameAction> gameActions, bool external) {
            if (gameActions == null) {
                throw new System.ArgumentNullException(nameof(gameActions));
            }
            foreach (var gameAction in gameActions) {
                ProcessGameAction(gameAction, external);
            }
        }


        // General unit updates.

        private bool ProcessOwnGeneralUnitUpdate(GeneralUnitUpdate generalUnitUpdate) {
            if (generalUnitUpdate is CauseDamage causeDamage) {
                return ProcessOwnGeneralUnitUpdate(causeDamage);
            }
            if (generalUnitUpdate is StopCondition stopCondition) {
                return ProcessOwnGeneralUnitUpdate(stopCondition);
            }
            if (generalUnitUpdate is ForceCondition forceCondition) {
                return ProcessOwnGeneralUnitUpdate(forceCondition);
            }
            if (generalUnitUpdate is UseAbility useAbility) {
                return ProcessOwnGeneralUnitUpdate(useAbility);
            }
            if (generalUnitUpdate is AttachAbility attachAbility) {
                return ProcessOwnGeneralUnitUpdate(attachAbility);
            }
            return false;
        }

        private bool ProcessOwnGeneralUnitUpdate(CauseDamage causeDamage) {
            if (durability != null) {
                durability.TakeDamage(causeDamage.DamageValue);
            }
            return true;
        }

        private bool ProcessOwnGeneralUnitUpdate(StopCondition stopCondition) {
            if (stopCondition.Condition.BelongsTo(currentCondition)) {
                var actions = currentCondition.Stop(stopCondition.ForceStop);
                if (currentCondition.Exited) {
                    currentCondition = null;
                }
                ProcessGameActions(actions, false);
            }
            return true;
        }

        private bool ProcessOwnGeneralUnitUpdate(ForceCondition forceCondition) {
            if (currentCondition != null) {
                var stopCondition = new StopCondition(CurrentCondition, true);
                ProcessOwnGeneralUnitUpdate(stopCondition);
            }
            currentCondition = forceCondition.Condition;
            var actions = currentCondition.Enter(this);
            ProcessGameActions(actions, false);
            return true;
        }

        private bool ProcessOwnGeneralUnitUpdate(UseAbility useAbility) {
            IAbility ability = FindAbility(useAbility.AbilityPresentation);
            if (ability != null) {
                if (currentCondition != null) {
                    var cancelCondition = new CancelCondition(CurrentCondition);
                    ProcessOwnGeneralUnitUpdate(cancelCondition);
                }
                if (currentCondition == null) {
                    var newCondition = ability.Use(useAbility.ParameterSet);
                    var forceCondition = new ForceCondition(this, newCondition);
                    ProcessOwnGeneralUnitUpdate(forceCondition);
                }
            }
            return true;
        }

        private bool ProcessOwnGeneralUnitUpdate(AttachAbility attachAbility) {
            IAbility ability = attachAbility.Ability;
            if (!ability.Connected) {
                ability.Connect(this);
                abilities[ability.Presentation] = ability;
            }
            return true;
        }


        // Internal unit updates.

        private bool ProcessOwnInternalUnitUpdate(InternalUnitUpdate internalUnitUpdate) {
            if (internalUnitUpdate is MoveUnit moveUnit) {
                return ProcessOwnInternalUnitUpdate(moveUnit);
            }
            if (internalUnitUpdate is SetRotation setRotation) {
                return ProcessOwnInternalUnitUpdate(setRotation);
            }
            if (internalUnitUpdate is UpdateAnimatorParameter<float> updateFloatAnimatorParameter) {
                return ProcessOwnInternalUnitUpdate(updateFloatAnimatorParameter);
            }
            if (internalUnitUpdate is UpdateAnimatorParameter<bool> updateBoolAnimatorParameter) {
                return ProcessOwnInternalUnitUpdate(updateBoolAnimatorParameter);
            }
            if (internalUnitUpdate is ApplyAnimatorTrigger applyAnimatorTrigger) {
                return ProcessOwnInternalUnitUpdate(applyAnimatorTrigger);
            }
            return false;
        }

        private bool ProcessOwnInternalUnitUpdate(MoveUnit moveUnit) {
            Vector2 newPosition2D = new Vector2(transform.position.x, transform.position.z) + moveUnit.PositionChange;
            float newY = MapController.Instance.GetSurfaceHeight(newPosition2D);
            Vector3 newPosition = new Vector3(newPosition2D.x, newY, newPosition2D.y);
            Vector3 positionChange = newPosition - transform.position;
            transform.position = newPosition;
            if (moveUnit.RotationFromForward != null) {
                Quaternion newRotation = (positionChange == Vector3.zero ? transform.rotation : Quaternion.LookRotation(positionChange));
                newRotation *= moveUnit.RotationFromForward.Value;
                transform.rotation = newRotation;
            }
            return true;
        }

        private bool ProcessOwnInternalUnitUpdate(SetRotation setRotation) {
            transform.rotation = setRotation.Rotation;
            return true;
        }

        private bool ProcessOwnInternalUnitUpdate(UpdateAnimatorParameter<float> animatorParameterUpdate) {
            animator.SetFloat(animatorParameterUpdate.AnimationParameterHash, animatorParameterUpdate.NewValue);
            return true;
        }

        private bool ProcessOwnInternalUnitUpdate(UpdateAnimatorParameter<bool> animatorParameterUpdate) {
            animator.SetBool(animatorParameterUpdate.AnimationParameterHash, animatorParameterUpdate.NewValue);
            return true;
        }

        private bool ProcessOwnInternalUnitUpdate(ApplyAnimatorTrigger animatorTriggerApplication) {
            animator.SetTrigger(animatorTriggerApplication.AnimationTriggerHash);
            return true;
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            UnitSystemController.Instance.AddAction(unitSystemUpdate);
            return true;
        }


        // Support.

        private IAbility FindAbility(IAbilityPresentation abilityPresentation) {
            foreach (var pair in abilities) {
                if (pair.Key == abilityPresentation) {
                    return pair.Value;
                }
            }
            return null;
        }


    }

}