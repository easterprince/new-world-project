using NewWorld.Controllers.Battle.Map;
using NewWorld.Controllers.Battle.UnitSystem;
using NewWorld.Cores.Battle.Unit;
using NewWorld.Cores.Battle.UnitSystem;
using NewWorld.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Controllers.Battle.Nodes {
    
    public class NodeGridController : MonoBehaviour {

        // Fields.

        private readonly Dictionary<UnitPresentation, NodeController> unitsToNodes = new Dictionary<UnitPresentation, NodeController>();
        private UnitSystemPresentation unitSystemPresentation = null;
        private ObjectState<UnitPresentation>.StateWrapper currentState = null;

        // Steady references.
        [SerializeField]
        MapController map;
        [SerializeField]
        UnitSystemController unitSystem;


        // Life cycle.

        private void Start() {
            
            // Validate steady references.
            GameObjects.ValidateReference(map, nameof(map));
            GameObjects.ValidateReference(unitSystem, nameof(unitSystem));

            // Setup unit system event handlers.
            unitSystem.ExecuteWhenBuilt(this, () => {
                unitSystemPresentation = unitSystem.Presentation;
                currentState = unitSystemPresentation.State;
                foreach (var unit in unitSystemPresentation) {
                    UpdateNode(unit);
                }
            });

        }

        private void LateUpdate() {
            while (currentState != null && !currentState.IsLatest) {
                currentState = currentState.Transit(out var unitPresentation);
                UpdateNode(unitPresentation);
            }
        }


        // Event handlers.

        private void UpdateNode(UnitPresentation unit) {
            if (unitSystemPresentation.Contains(unit)) {
                var position = unitSystemPresentation[unit];
                if (unitsToNodes.TryGetValue(unit, out var node)) {
                    node.Position = position;
                } else {
                    node = NodeController.BuildNode(map, position);
                    node.transform.parent = transform;
                    unitsToNodes[unit] = node;
                }
            } else {
                if (unitsToNodes.TryGetValue(unit, out var node)) {
                    Destroy(node.gameObject);
                    unitsToNodes.Remove(unit);
                }
            }
        }


    }

}
