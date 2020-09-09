using NewWorld.Cores.Battle.Battlefield;
using NewWorld.Cores.Battle.Map;
using NewWorld.Cores.Battle.Unit;
using NewWorld.Utilities;
using NewWorld.Utilities.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Cores.Battle.UnitSystem {

    public class UnitSystemCore : ConnectableCoreBase<UnitSystemCore, UnitSystemPresentation, BattlefieldPresentation>,
        IEnumerable<UnitPresentation>,
        IResponsive<UnitAdditionAction>, IResponsive<UnitMotionAction>, IResponsive<UnitRemovalAction> {

        // Fields.

        // Structure.
        private readonly Dictionary<UnitCore, Vector2Int> unitsToPositions = new Dictionary<UnitCore, Vector2Int>();
        private readonly Dictionary<Vector2Int, UnitCore> positionsToUnits = new Dictionary<Vector2Int, UnitCore>();
        private readonly Dictionary<UnitPresentation, UnitCore> presentationsToUnits = new Dictionary<UnitPresentation, UnitCore>();

        // State.
        private ObjectState<UnitPresentation> state = new ObjectState<UnitPresentation>();


        // Constructors.

        public UnitSystemCore() {}

        public UnitSystemCore(UnitSystemCore other) {
            foreach (var unitAndPosition in other.unitsToPositions) {
                AddUnit(unitAndPosition.Key, unitAndPosition.Value);
            }
        }


        // Properties.

        public Vector2Int this[UnitPresentation unitPresentation] {
            get {
                if (!presentationsToUnits.TryGetValue(unitPresentation, out UnitCore unit)) {
                    throw new KeyNotFoundException($"No such unit ({unitPresentation}) in unit system!");
                }
                return unitsToPositions[unit];
            }
        }

        public UnitPresentation this[Vector2Int position] {
            get {
                if (!positionsToUnits.TryGetValue(position, out UnitCore unit)) {
                    return null;
                }
                return unit.Presentation;
            }
        }

        public ObjectState<UnitPresentation>.StateWrapper State => state.Wrapper;


        // Enumeration.

        public IEnumerator<UnitPresentation> GetEnumerator() {
            foreach (var unitPresentation in presentationsToUnits.Keys) {
                yield return unitPresentation;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }


        // Presentation generation.

        private protected override UnitSystemPresentation BuildPresentation() {
            return new UnitSystemPresentation(this);
        }


        // Cloning.

        public override UnitSystemCore Clone() {
            return new UnitSystemCore(this);
        }


        // Informational methods.

        public bool Contains(UnitPresentation unitPresentation) {
            if (unitPresentation is null) {
                throw new ArgumentNullException(nameof(unitPresentation));
            }
            return presentationsToUnits.ContainsKey(unitPresentation);
        }


        // Updating.

        public void Update() {
            ValidateContext();
            foreach (var unit in unitsToPositions.Keys) {
                unit.Update();
            }
            foreach (var unit in unitsToPositions.Keys) {
                unit.Act();
            }
        }


        // Modifying methods.

        public void AddUnit(UnitCore unit, Vector2Int position) {
            if (unit is null) {
                throw new ArgumentNullException(nameof(unit));
            }
            var onPosition = this[position];
            if (onPosition != null) {
                throw new InvalidOperationException($"Position {position} is already occupied by {onPosition}.");
            }
            unit = unit.Clone();
            unit.Connect(Presentation);
            presentationsToUnits[unit.Presentation] = unit;
            unitsToPositions[unit] = position;
            positionsToUnits[position] = unit;
            state = state.BuildTransition(unit.Presentation);
        }

        public void MoveUnit(UnitPresentation unitPresentation, Vector2Int position) {
            if (unitPresentation is null) {
                throw new ArgumentNullException(nameof(unitPresentation));
            }
            if (!Contains(unitPresentation)) {
                throw new InvalidOperationException($"There is no unit {unitPresentation}!");
            }
            var onPosition = this[position];
            if (onPosition != null) {
                if (onPosition != unitPresentation) {
                    throw new InvalidOperationException($"Position {position} is already occupied by {onPosition}.");
                }
            } else {
                UnitCore unit = presentationsToUnits[unitPresentation];
                var oldPosition = unitsToPositions[unit];
                positionsToUnits.Remove(oldPosition);
                unitsToPositions[unit] = position;
                positionsToUnits[position] = unit;
                state = state.BuildTransition(unitPresentation);
            }
        }

        public UnitCore RemoveUnit(UnitPresentation unitPresentation) {
            if (unitPresentation is null) {
                throw new ArgumentNullException(nameof(unitPresentation));
            }
            if (!Contains(unitPresentation)) {
                throw new InvalidOperationException($"There is no unit {unitPresentation}!");
            }
            UnitCore unit = presentationsToUnits[unitPresentation];
            unit.Disconnect();
            presentationsToUnits.Remove(unitPresentation);
            var position = unitsToPositions[unit];
            positionsToUnits.Remove(position);
            unitsToPositions.Remove(unit);
            state = state.BuildTransition(unitPresentation);
            return unit;
        }


        // Action processing.

        public void ProcessAction(UnitAdditionAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            try {
                AddUnit(action.Unit, action.Position);
            } catch (InvalidOperationException) {}
        }

        public void ProcessAction(UnitMotionAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            try {
                MoveUnit(action.Unit, action.Position);
            } catch (InvalidOperationException) {}
        }

        public void ProcessAction(UnitRemovalAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            try {
                RemoveUnit(action.Unit);
            } catch (InvalidOperationException) {}
        }

        public void PlanAction(UnitAdditionAction action) => PlanAction(this, action);
        public void PlanAction(UnitMotionAction action) => PlanAction(this, action);
        public void PlanAction(UnitRemovalAction action) => PlanAction(this, action);


    }

}
