using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Unit.Abilities;
using NewWorld.Battlefield.Unit.Conditions;
using NewWorld.Battlefield.UnitSystem;
using NewWorld.Battlefield.Unit.Core;
using NewWorld.Battlefield.Unit.Durability;

namespace NewWorld.Battlefield.Unit {

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
            if (generalUnitUpdate is DamageTaking causeDamage) {
                return ProcessOwnGeneralUnitUpdate(causeDamage);
            }
            if (generalUnitUpdate is ConditionStop stopCondition) {
                return ProcessOwnGeneralUnitUpdate(stopCondition);
            }
            if (generalUnitUpdate is ConditionChange forceCondition) {
                return ProcessOwnGeneralUnitUpdate(forceCondition);
            }
            if (generalUnitUpdate is AbilityUsage useAbility) {
                return ProcessOwnGeneralUnitUpdate(useAbility);
            }
            if (generalUnitUpdate is AbilityAttachment attachAbility) {
                return ProcessOwnGeneralUnitUpdate(attachAbility);
            }
            return false;
        }

        private bool ProcessOwnGeneralUnitUpdate(DamageTaking causeDamage) {
            if (durability != null) {
                durability.TakeDamage(ownPassport, causeDamage.DamageValue);
            }
            return true;
        }

        private bool ProcessOwnGeneralUnitUpdate(ConditionStop stopCondition) {
            if (stopCondition.Condition == currentCondition) {
                var actions = currentCondition.Stop(ownPassport, stopCondition.ForceStop);
                if (currentCondition.Status == UnitCondition.StatusType.Exited) {
                    currentCondition.Disconnect(ownPassport);
                    currentCondition = null;
                }
                ProcessGameActions(actions, false);
            }
            return true;
        }

        private bool ProcessOwnGeneralUnitUpdate(ConditionChange forceCondition) {
            if (forceCondition.Condition.Connected || forceCondition.Condition.Status != UnitCondition.StatusType.NotEntered) {
                return true;
            }
            if (currentCondition != null) {
                var stopCondition = new ConditionStop(CurrentCondition, true);
                ProcessOwnGeneralUnitUpdate(stopCondition);
            }
            currentCondition = forceCondition.Condition;
            currentCondition.Connect(ownPassport);
            var actions = currentCondition.Enter(ownPassport);
            ProcessGameActions(actions, false);
            return true;
        }

        private bool ProcessOwnGeneralUnitUpdate(AbilityUsage useAbility) {
            var ability = useAbility.Ability;
            if (abilities.Contains(ability)) {
                if (currentCondition != null) {
                    var cancelCondition = new ConditionCancellation(CurrentCondition);
                    ProcessOwnGeneralUnitUpdate(cancelCondition);
                }
                if (currentCondition == null) {
                    var newCondition = ability.Use(ownPassport, useAbility.ParameterSet);
                    var forceCondition = new ConditionChange(this, newCondition);
                    ProcessOwnGeneralUnitUpdate(forceCondition);
                }
            }
            return true;
        }

        private bool ProcessOwnGeneralUnitUpdate(AbilityAttachment attachAbility) {
            var ability = attachAbility.Ability;
            if (!ability.Connected) {
                ability.Connect(ownPassport);
                abilities.Add(ability);
            }
            return true;
        }


        // Internal unit updates.

        private bool ProcessOwnInternalUnitUpdate(InternalUnitUpdate internalUnitUpdate) {
            if (internalUnitUpdate is PositionShift moveUnit) {
                return ProcessOwnInternalUnitUpdate(moveUnit);
            }
            if (internalUnitUpdate is RotationUpdate setRotation) {
                return ProcessOwnInternalUnitUpdate(setRotation);
            }
            if (internalUnitUpdate is AnimatorParameterUpdate<float> updateFloatAnimatorParameter) {
                return ProcessOwnInternalUnitUpdate(updateFloatAnimatorParameter);
            }
            if (internalUnitUpdate is AnimatorParameterUpdate<bool> updateBoolAnimatorParameter) {
                return ProcessOwnInternalUnitUpdate(updateBoolAnimatorParameter);
            }
            if (internalUnitUpdate is AnimatorTriggerApplication applyAnimatorTrigger) {
                return ProcessOwnInternalUnitUpdate(applyAnimatorTrigger);
            }
            return false;
        }

        private bool ProcessOwnInternalUnitUpdate(PositionShift moveUnit) {
            Vector2 newPosition2D = new Vector2(transform.position.x, transform.position.z) + moveUnit.Addition;
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

        private bool ProcessOwnInternalUnitUpdate(RotationUpdate setRotation) {
            transform.rotation = setRotation.Rotation;
            return true;
        }

        private bool ProcessOwnInternalUnitUpdate(AnimatorParameterUpdate<float> animatorParameterUpdate) {
            animator.SetFloat(animatorParameterUpdate.AnimationParameterHash, animatorParameterUpdate.NewValue);
            return true;
        }

        private bool ProcessOwnInternalUnitUpdate(AnimatorParameterUpdate<bool> animatorParameterUpdate) {
            animator.SetBool(animatorParameterUpdate.AnimationParameterHash, animatorParameterUpdate.NewValue);
            return true;
        }

        private bool ProcessOwnInternalUnitUpdate(AnimatorTriggerApplication animatorTriggerApplication) {
            animator.SetTrigger(animatorTriggerApplication.AnimationTriggerHash);
            return true;
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            UnitSystemController.Instance.AddAction(unitSystemUpdate);
            return true;
        }


    }

}