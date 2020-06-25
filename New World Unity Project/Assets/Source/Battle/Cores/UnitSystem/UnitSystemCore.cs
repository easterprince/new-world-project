using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battle.Cores.UnitSystem {

    public class UnitSystemCore : ReceptiveCoreBase<UnitSystemCore, UnitSystemPresentation> {

        // Fields.
        ]
        private readonly Dictionary<UnitCore, Vector2Int> unitsToPositions = new Dictionary<UnitCore, Vector2Int>();
        private readonly Dictionary<Vector2Int, UnitCore> positionsToUnits = new Dictionary<Vector2Int, UnitCore>();


        // Constructor.

        public UnitSystemCore(ActionPlanner planner) : base(planner) {}


        // Properties.

        public Vector2Int this[UnitCore unit] {
            get {
                if (!unitsToPositions.TryGetValue(unit, out Vector2Int position)) {
                    throw new KeyNotFoundException($"No such unit ({unit}) in unit system!");
                }
                return position;
            }
        }

        public UnitCore this[Vector2Int position] {
            get {
                if (!positionsToUnits.TryGetValue(position, out UnitCore unit)) {
                    return null;
                }
                return unit;
            }
        }


        // Presentation generation.

        private protected override UnitSystemPresentation BuildPresentation() {
            return new UnitSystemPresentation(this);
        }


        // Action processing.

        public override void ProcessAction(IGameAction action) {
            throw new System.NotImplementedException(); // TODO.
        }


        // Informational methods.

        public bool HasUnit(UnitCore unit) {
            if (unit is null) {
                throw new System.ArgumentNullException(nameof(unit));
            }
            return unitsToPositions.ContainsKey(unit);
        }


        // Modifying methods.

        public void AddUnit(UnitCore unit, Vector2Int position) {
            if (unit is null) {
                throw new System.ArgumentNullException(nameof(unit));
            }
            if (HasUnit(unit)) {
                if (position != unitsToPositions[unit]) {
                    throw new System.InvalidOperationException($"There is already unit {unit}!");
                }
            } else {
                var onPosition = this[position];
                if (onPosition != null) {
                    throw new System.InvalidOperationException($"Position {position} is already occupied by {onPosition}.");
                }
                unitsToPositions[unit] = position;
                positionsToUnits[position] = unit;
            }
        }

        public void MoveUnit(UnitCore unit, Vector2Int position) {
            if (unit is null) {
                throw new System.ArgumentNullException(nameof(unit));
            }
            if (!HasUnit(unit)) {
                throw new System.InvalidOperationException($"There is no unit {unit}!");
            }
            var onPosition = this[position];
            if (onPosition != null) {
                if (onPosition != unit) {
                    throw new System.InvalidOperationException($"Position {position} is already occupied by {onPosition}.");
                }
            } else {
                positionsToUnits.Remove(unitsToPositions[unit]);
                unitsToPositions[unit] = position;
                positionsToUnits[position] = unit;
            }
        }

        public void RemoveUnit(UnitCore unit) {
            if (unit is null) {
                throw new System.ArgumentNullException(nameof(unit));
            }
            if (!HasUnit(unit)) {
                throw new System.InvalidOperationException($"There is no unit {unit}!");
            }
            positionsToUnits.Remove(unitsToPositions[unit]);
            unitsToPositions.Remove(unit);
        }


    }

}
