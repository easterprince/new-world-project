using NewWorld.Battle.Controllers.Map;
using NewWorld.Battle.Controllers.UnitSystem;
using NewWorld.Battle.Cores.Unit;
using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities;
using NewWorld.Utilities.Events;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Nodes {
    
    public class NodeGridController : MonoBehaviour {

        // Fields.

        private readonly ActionQueue actionQueue = new ActionQueue();
        private readonly Dictionary<UnitPresentation, NodeController> unitsToNodes = new Dictionary<UnitPresentation, NodeController>();
        private UnitSystemPresentation unitSystemPresentation = null;

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
                foreach (var unit in unitSystemPresentation) {
                    UpdateNode(unit);
                }
                unitSystemPresentation.AdditionEvent.AddAction(actionQueue, UpdateNode);
                unitSystemPresentation.MotionEvent.AddAction(actionQueue, UpdateNode);
                unitSystemPresentation.RemovalEvent.AddAction(actionQueue, UpdateNode);
            });

        }

        private void Update() {
            actionQueue.RunAll();
        }

        private void OnDestroy() {
            if (unitSystemPresentation != null) {
                unitSystemPresentation.AdditionEvent.RemoveSubscriber(actionQueue);
                unitSystemPresentation.MotionEvent.RemoveSubscriber(actionQueue);
                unitSystemPresentation.RemovalEvent.RemoveSubscriber(actionQueue);
            }
        }


        // Event handlers.

        private void UpdateNode(UnitPresentation unit) {
            if (unitSystemPresentation.HasUnit(unit)) {
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
