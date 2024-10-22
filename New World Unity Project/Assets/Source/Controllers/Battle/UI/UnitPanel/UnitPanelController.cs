﻿using NewWorld.Controllers.Battle.Cameras;
using NewWorld.Controllers.Battle.UI.Selection;
using NewWorld.Controllers.Battle.Unit;
using NewWorld.Controllers.MetaData;
using NewWorld.Cores.Battle.Unit;
using NewWorld.Cores.Battle.Unit.Behaviours;
using NewWorld.Cores.Battle.Unit.Behaviours.Offensives;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
using NewWorld.Utilities;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Controllers.Battle.UI.UnitPanel {

    public class UnitPanelController : MonoBehaviour {

        // Fields.

        UnitController selectedUnit;

        // Steady references.
        [Header("Inner")]
        [SerializeField]
        private UnitPortraitController portrait;
        [SerializeField]
        private Text unitNameText;
        [SerializeField]
        private Text unitDescriptionText;
        [SerializeField]
        private DurabilitySectionController durabilitySection;
        [Header("Outer")]
        [SerializeField]
        private SelectionSystemController selectionSystem;
        [SerializeField]
        private CameraController mainCamera;


        // Life cycle.

        private void Start() {
            GameObjects.ValidateReference(portrait, nameof(portrait));
            GameObjects.ValidateReference(unitNameText, nameof(unitNameText));
            GameObjects.ValidateReference(unitDescriptionText, nameof(unitDescriptionText));
            GameObjects.ValidateReference(durabilitySection, nameof(durabilitySection));
            GameObjects.ValidateReference(selectionSystem, nameof(selectionSystem));
            GameObjects.ValidateReference(mainCamera, nameof(mainCamera));
            selectionSystem.UnitSelectedEvent.AddAction(this, ProcessSelectionChange);
            selectionSystem.UnitTargetedEvent.AddAction(this, ProcessTargetSet);
            selectionSystem.PositionTargetedEvent.AddAction(this, ProcessTargetSet);
            selectionSystem.UntargetedEvent.AddAction(this, ProcessTargetUnset);
        }

        private void LateUpdate() {
            UpdateInfo();
        }

        private void OnDestroy() {
            selectionSystem.UnitSelectedEvent.RemoveSubscriber(this);
            selectionSystem.UnitTargetedEvent.RemoveSubscriber(this);
            selectionSystem.PositionTargetedEvent.RemoveSubscriber(this);
        }


        // Event handlers.

        private void ProcessSelectionChange(UnitController unit) {
            selectedUnit = unit;
            portrait.Followed = unit;
        }

        private void ProcessTargetSet(Vector3 target) {
            if (selectedUnit != null && selectedUnit.Presentation != null) {
                var destination = target;
                var action = new GoalSettingAction(new RelocationGoal(destination));
                selectedUnit.Presentation.PlanAction(action);
            }
        }

        private void ProcessTargetSet(UnitController target) {
            if (selectedUnit != null && selectedUnit.Presentation != null) {
                var action = new GoalSettingAction(new OffensiveGoal(target.Presentation));
                selectedUnit.Presentation.PlanAction(action);
            }
        }

        private void ProcessTargetUnset() {
            if (selectedUnit != null && selectedUnit.Presentation != null) {
                var action = new GoalSettingAction(IdleGoal.Instance);
                selectedUnit.Presentation.PlanAction(action);
            }
        }


        // Support.

        private void UpdateInfo() {

            UnitPresentation presentation = null;
            if (selectedUnit != null) {
                presentation = selectedUnit.Presentation;
            }

            // Update name.
            unitNameText.text = presentation?.Name ?? "";

            // Update description.
            if (presentation != null) {
                var stringBuilder = new StringBuilder();

                // Describe goal.
                var currentGoal = presentation.Intelligence.CurrentGoal;
                var goalDescriptor = Descriptors.ForGoals[currentGoal.Id];
                stringBuilder.AppendLine($"Current goal: {goalDescriptor.ComposeDescription(currentGoal)}");
                stringBuilder.AppendLine();

                // Describe condition.
                var conditionDescriptor = Descriptors.ForConditions[presentation.Condition.Id];
                stringBuilder.AppendLine($"Current condition: {conditionDescriptor.ComposeDescription(presentation.Condition)}");
                stringBuilder.AppendLine();

                // Describe abilities.
                var abilities = presentation.AbilityCollection.Abilities;
                if (abilities.Count == 0) {
                    stringBuilder.AppendLine("No abilities.");
                } else {
                    stringBuilder.AppendLine($"Abilities ({abilities.Count}):");
                    foreach (var ability in abilities) {
                        var abilityDescriptor = Descriptors.ForAbilities[ability.Id];
                        stringBuilder.AppendLine(abilityDescriptor.Name);
                    }
                }

                unitDescriptionText.text = stringBuilder.ToString();
            } else {
                unitDescriptionText.text = "Click on unit to get its description.";
            }

            // Update durability info.
            if (presentation != null) {
                var durabilityModule = presentation.Durability;
                durabilitySection.gameObject.SetActive(true);
                durabilitySection.SetDurability(durabilityModule);
            } else {
                durabilitySection.gameObject.SetActive(false);
                durabilitySection.SetDurability(null);
            }

        }


    }

}
