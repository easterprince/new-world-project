using NewWorld.Battle.Cores.Unit;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Unit {
    
    public class UnitController : MonoBehaviour {

        // Fabric.

        private const string prefabPath = "Prefabs/Unit";

        private static GameObject prefab = null;

        public static UnitController BuildUnit(UnitPresentation presentation) {
            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject unit = Instantiate(prefab);

            UnitController unitController = unit.GetComponent<UnitController>();
            unitController.Presentation = presentation;
            return unitController;
        }


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
