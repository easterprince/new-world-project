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
            if (presentation is null) {
                throw new ArgumentNullException(nameof(presentation));
            }

            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject unit = Instantiate(prefab);
            unit.name = presentation.Name;

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

        public UnitPresentation Presentation {
            get => presentation;
            set {
                presentation = value ?? throw new ArgumentNullException(nameof(value));
                name = presentation.Name;
            }
        }


        // Life cycle.

        private void Start() {
            animator = GetComponent<Animator>();
            GameObjects.ValidateComponent(animator);
        }

        private void Update() {
            if (presentation is null) {
                return;
            }
            transform.position = presentation.Body.Position;
            transform.rotation = presentation.Body.Rotation;
        }


    }

}
