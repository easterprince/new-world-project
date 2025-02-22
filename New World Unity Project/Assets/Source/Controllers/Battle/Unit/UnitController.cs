﻿using NewWorld.Controllers.MetaData;
using NewWorld.Cores.Battle.Unit;
using NewWorld.Cores.Battle.Unit.Conditions;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using System;
using UnityEngine;

namespace NewWorld.Controllers.Battle.Unit {

    public class UnitController : BuildableController {

        // Fabric.

        public static UnitController BuildUnit(UnitPresentation presentation) {
            GameObject unit = Instantiate(Prefab);
            UnitController controller = unit.GetComponent<UnitController>();
            controller.Build(presentation);
            return controller;
        }

        protected static GameObject Prefab => PrefabSourceController.Instance.UnitPrefab;


        // Static fields.

        private readonly static int speedHash = Animator.StringToHash("Speed");


        // Fields.

        // Presentation.
        private UnitPresentation presentation;
        private IConditionPresentation currentCondition;

        // Gameobject components.
        private Animator animator;
        private new Collider collider;


        // Properties.

        public UnitPresentation Presentation => presentation;

        public Vector3 Center {
            get {
                if (collider == null) {
                    return transform.position;
                }
                return collider.bounds.center;
            }
        }

        public Collider Collider => collider;


        // Building.

        public void Build(UnitPresentation presentation) {
            if (presentation is null) {
                throw new ArgumentNullException(nameof(presentation));
            }
            Start();
            ValidateNotStartedBuilding();

            // Build stuff.
            SetStartedBuilding();
            this.presentation = presentation;
            name = presentation.Name;
            SetFinishedBuilding();

        }



        // Life cycle.

        private protected override void OnStart() {
            base.OnStart();
            name = "Empty Unit";
            animator = GetComponent<Animator>();
            GameObjects.ValidateComponent(animator);
            collider = GetComponent<Collider>();
            GameObjects.ValidateComponent(collider);
        }

        private void LateUpdate() {
            if (presentation != null) {

                // Update transform.
                transform.localPosition = presentation.Body.Position;
                transform.localRotation = presentation.Body.Rotation;

                // Update animation.
                if (currentCondition != presentation.Condition) {

                    // Clear previous animation.
                    ConditionDescriptor conditionDescriptor;
                    if (currentCondition != null) {
                        conditionDescriptor = Descriptors.ForConditions[currentCondition.Id];
                        if (conditionDescriptor.AnimationHash.HasValue) {
                            animator.SetBool(conditionDescriptor.AnimationHash.Value, false);
                        }
                    }

                    // Set current animation.
                    currentCondition = presentation.Condition;
                    animator.SetFloat(speedHash, currentCondition.ConditionSpeed);
                    conditionDescriptor = Descriptors.ForConditions[currentCondition.Id];
                    if (conditionDescriptor.AnimationHash.HasValue) {
                        animator.SetBool(conditionDescriptor.AnimationHash.Value, true);
                    }

                }

            }
        }


    }

}
