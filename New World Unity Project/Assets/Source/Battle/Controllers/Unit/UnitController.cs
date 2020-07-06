using NewWorld.Battle.Cores.Unit;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Unit {
    
    public class UnitController : MonoBehaviour {

        // Fabric.

        private const string prefabPath = "Prefabs/Unit";

        private static GameObject prefab = null;
        private static int builtUnits = 0;

        public static UnitController BuildUnit(UnitPresentation presentation) {
            if (presentation is null) {
                throw new ArgumentNullException(nameof(presentation));
            }

            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject unit = Instantiate(prefab);
            unit.name = $"Unit {builtUnits + 1}";
            ++builtUnits;

            UnitController unitController = unit.GetComponent<UnitController>();
            unitController.presentation = presentation;
            return unitController;
        }


        // Fields.

        // Presentation.
        private UnitPresentation presentation;

        // Gameobject components.
        private Animator animator;


        // Properties.

        public UnitPresentation Presentation => presentation;


        // Life cycle.

        private void Awake() {
            animator = GetComponent<Animator>();
            GameObjects.ValidateComponent(animator);
        }

        private void Update() {
            transform.position = Presentation.Body.Position;
            transform.rotation = Presentation.Body.Rotation;
        }


    }

}
