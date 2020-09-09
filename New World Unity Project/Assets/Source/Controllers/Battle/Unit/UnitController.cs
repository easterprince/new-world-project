using NewWorld.Cores.Battle.Unit;
using NewWorld.Cores.Battle.Unit.Conditions;
using NewWorld.Cores.Battle.Unit.Conditions.Attacks;
using NewWorld.Cores.Battle.Unit.Conditions.Motions;
using NewWorld.Cores.Battle.Unit.Conditions.Others;
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

        private readonly static int attackSpeedHash = Animator.StringToHash("AttackSpeed");
        private readonly static int motionSpeedHash = Animator.StringToHash("MotionSpeed");
        private readonly static int collapseHash = Animator.StringToHash("Collapse");
        private readonly static int riseHash = Animator.StringToHash("Rise");


        // Fields.

        // Presentation.
        private UnitPresentation presentation;
        private IConditionPresentation currentCondition;

        // Gameobject components.
        private Animator animator;
        private new Collider collider;

        // Animation.
        


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
            currentCondition = presentation.Condition;
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
                    if (currentCondition is MotionConditionPresentation) {
                        animator.SetFloat(motionSpeedHash, 0);
                    } else if (currentCondition is AttackConditionPresentation) {
                        animator.SetFloat(attackSpeedHash, 0);
                    }

                    // Set current animation.
                    currentCondition = presentation.Condition;
                    if (currentCondition is MotionConditionPresentation motion) {
                        animator.SetFloat(motionSpeedHash, motion.MovementPerSecond);
                    } else if (currentCondition is AttackConditionPresentation attack) {
                        animator.SetFloat(attackSpeedHash, 1f);
                    } else if (currentCondition is CollapseConditionPresentation collapse) {
                        animator.SetTrigger(collapseHash);
                    }

                }

            }
        }


    }

}
