using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Conditions {
 
    public abstract class Condition : UnitModule<ConditionPresentation> {

        // Enumerators.

        protected enum StopType {
            Completed,
            Cancelled,
            Forced
        }


        // Fields.

        private bool exited = false;


        // Properties.

        public bool Exited => exited;
        public virtual bool CanBeCancelled => false;


        // To string conversion.

        override public string ToString() {
            return "Unknown condition";
        }


        // Constructor.

        protected Condition() {
            Presentation = new ConditionPresentation(this);
        }


        // Interaction methods.

        public IEnumerable<GameAction> Enter(UnitController owner) {
            if (Connected) {
                throw new System.InvalidOperationException("Condition has been entered already!");
            }
            Connect(owner);
            return OnEnter();
        }

        public IEnumerable<GameAction> Update() {
            if (!Connected) {
                throw new System.InvalidOperationException("Condition is not connected!");
            }
            if (exited) {
                throw new System.InvalidOperationException("Condition is over!");
            }
            var actions = OnUpdate(out exited);
            if (exited) {
                var otherActions = OnFinish(StopType.Completed);
                actions = Enumerables.Unite(actions, otherActions);
            }
            return actions;
        }

        public IEnumerable<GameAction> Stop(bool forceStop) {
            if (!Connected) {
                throw new System.InvalidOperationException("Condition is not connected!");
            }
            if (exited || !forceStop && !CanBeCancelled) {
                return Enumerables.GetNothing<GameAction>();
            }
            var actions = OnFinish(forceStop ? StopType.Forced : StopType.Cancelled);
            exited = true;
            return actions;
        }


        // Life cycle.

        protected abstract IEnumerable<GameAction> OnEnter();

        protected abstract IEnumerable<GameAction> OnUpdate(out bool completed);

        protected abstract IEnumerable<GameAction> OnFinish(StopType stopType);


    }

}