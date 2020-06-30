﻿using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NewWorld.Battlefield.Unit;
using NewWorld.Battlefield.UI.SelectionSystem;
using NewWorld.Battlefield.Cameras;
using NewWorld.Battlefield.UI.Bars;

namespace NewWorld.Battlefield.UI.UnitPanel {

    public class UnitPanelController : MonoBehaviour {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [Header("Camera")]
        [SerializeField]
        private CameraController mainCamera;

        [Header("Portrait")]
        [SerializeField]
        private UnitPortraitController portrait;

        [Header("Text")]
        [SerializeField]
        private Text unitNameText;
        [SerializeField]
        private Text unitDescriptionText;

        [Header("Bars")]
        [SerializeField]
        private BarController durabilityBar;

#pragma warning restore IDE0044, CS0414, CS0649

        UnitController selectedUnit;


        // Life cycle.

        private void Start() {
            if (mainCamera == null) {
                throw new MissingReferenceException($"Missing {nameof(mainCamera)}.");
            }
            if (unitNameText == null) {
                throw new MissingReferenceException($"Missing {nameof(unitNameText)}.");
            }
            if (unitDescriptionText == null) {
                throw new MissingReferenceException($"Missing {nameof(unitDescriptionText)}.");
            }
            if (portrait == null) {
                throw new MissingReferenceException($"Missing {nameof(portrait)}.");
            }
            if (durabilityBar == null) {
                throw new MissingReferenceException($"Missing {nameof(durabilityBar)}.");
            }
            PointerInterceptorController.EnsureInstance(this);
            portrait.PointerClickEvent.AddListener(ProcessPortraitClick);
            PointerInterceptorController.Instance.ClickEvent.AddListener(ProcessInterceptorClick);
        }

        private void LateUpdate() {
            UpdateInfo();
        }

        private void OnDestroy() {
            PointerInterceptorController.Instance?.ClickEvent.RemoveListener(ProcessInterceptorClick);
        }


        // Trigger handlers.

        private void ProcessInterceptorClick(PointerEventData pointerEventData) {
            var pointerPosition = pointerEventData.position;
            var pointerRay = mainCamera.CameraComponent.ScreenPointToRay(pointerPosition);
            var layerMask = LayerMask.GetMask("Units");
            Physics.Raycast(pointerRay, out RaycastHit raycastHit, float.PositiveInfinity, layerMask);
            var colliderHit = raycastHit.collider;
            if (colliderHit != null) {
                selectedUnit = colliderHit.transform.gameObject.GetComponent<UnitController>();
            } else {
                selectedUnit = null;
            }
            portrait.Followed = selectedUnit;
            SelectionSystemController.Instance.ChangeMainSelection(selectedUnit);
        }

        private void ProcessPortraitClick() {
            if (selectedUnit == null) {
                return;
            }
            mainCamera.Center(selectedUnit);
        }


        // Support.

        private void UpdateInfo() {

            // Update name.
            if (selectedUnit != null) {
                unitNameText.text = selectedUnit.name;
            } else {
                unitNameText.text = "";
            }

            // Update description.
            if (selectedUnit != null) {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Current condition: {selectedUnit.CurrentCondition?.Description ?? "Idle"}");
                stringBuilder.AppendLine();
                var abilities = selectedUnit.Abilities;
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
            if (selectedUnit != null) {
                durabilityBar.gameObject.SetActive(true);
                var unitDurability = selectedUnit.Durability;
                if (unitDurability == null) {
                    durabilityBar.Filled = 1;
                    durabilityBar.Color = Color.white;
                    durabilityBar.TypeText = "DUR";
                    durabilityBar.ValueText = "Indestructible";
                } else {
                    durabilityBar.Filled = unitDurability.Durability / unitDurability.DurabilityLimit;
                    durabilityBar.Color = Color.red;
                    durabilityBar.TypeText = "DUR";
                    durabilityBar.ValueText = $"{unitDurability.Durability}/{unitDurability.DurabilityLimit}";
                }
            } else {
                durabilityBar.gameObject.SetActive(false);
                durabilityBar.Filled = 0;
                durabilityBar.TypeText = "";
                durabilityBar.ValueText = "";
            }

        }


    }

}