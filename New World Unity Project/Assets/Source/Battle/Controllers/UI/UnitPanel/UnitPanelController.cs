﻿using NewWorld.Battle.Controllers.Cameras;
using NewWorld.Battle.Controllers.Map;
using NewWorld.Battle.Controllers.UI.Bars;
using NewWorld.Battle.Controllers.UI.Selection;
using NewWorld.Battle.Controllers.Unit;
using NewWorld.Battle.Cores.Unit;
using NewWorld.Battle.Cores.Unit.Behaviours;
using NewWorld.Battle.Cores.Unit.Behaviours.Relocations;
using NewWorld.Utilities;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NewWorld.Battle.Controllers.UI.UnitPanel {

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
            portrait.ClickEvent.AddAction(this, ProcessPortraitClick);
            selectionSystem.UnitSelectedEvent.AddAction(this, ProcessSelectionChange);
            selectionSystem.PositionTargetedEvent.AddAction(this, ProcessTargetSet);
        }

        private void LateUpdate() {
            UpdateInfo();
        }

        private void OnDestroy() {
            portrait.ClickEvent.RemoveSubscriber(this);
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
                var action = new GoalSettingAction<RelocationGoal>(new RelocationGoal(destination));
                selectedUnit.Presentation.PlanAction(action);
            }
        }

        private void ProcessPortraitClick(PointerEventData pointerEventData) {

            // Process camera relocation.
            if (pointerEventData.button == PointerEventData.InputButton.Left) {
                if (selectedUnit == null) {
                    return;
                }
                var location = mainCamera.CurrentLocation;
                location.ViewedPosition = selectedUnit.Center;
                mainCamera.CurrentLocation = location;
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
                unitDescriptionText.text = "There are some abilities... ?";
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Current condition: {presentation.Condition.Description}");
                stringBuilder.AppendLine();
                var abilities = presentation.AbilityCollection.Abilities;
                if (abilities.Count == 0) {
                    stringBuilder.AppendLine("No abilities.");
                } else {
                    stringBuilder.AppendLine($"Abilities ({abilities.Count}):");
                    foreach (var ability in abilities) {
                        stringBuilder.AppendLine(ability.Name);
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
