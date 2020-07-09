using NewWorld.Battle.Cores.Unit;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Unit {
    
    public class UnitController : MonoBehaviour {

        // Fabric.

        public static UnitController BuildUnit(UnitPresentation presentation) {
            GameObject unit = Instantiate(Prefab);
            UnitController controller = unit.GetComponent<UnitController>();
            controller.Presentation = presentation;
            return controller;
        }

        protected static GameObject Prefab => PrefabSourceController.Instance.UnitPrefab;


        // Fields.

        // Presentation.
        private UnitPresentation presentation;

        // Gameobject components.
        private Animator animator;


        // Properties.

        public UnitPresentation Presentation {
            get => presentation;
            set {
                presentation = value;
                Rebuild();
            }
        }


        // Rebuilding method.

        public void Rebuild() {
            name = presentation?.Name ?? "Unit";
            animator = GetComponent<Animator>();
        }


        // Life cycle.

        private void LateUpdate() {
            if (presentation != null) {
                transform.localPosition = presentation.Body.Position;
                transform.localRotation = presentation.Body.Rotation;
            }
        }


    }

}
