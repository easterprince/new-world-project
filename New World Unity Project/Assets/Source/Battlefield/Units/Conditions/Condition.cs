using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Conditions {
 
    public abstract class Condition : UnitModule {

        // Enumerators.

        protected enum Status {
            Ready,
            Entered,
            Exited
        }

        protected enum StopType {
            Completed,
            Cancelled,
            Forced
        }


        // Fields.

        private Status status;


        // Properties.

        public bool Ready => status == Status.Ready;
        public bool Exited => status == Status.Exited;
        public virtual bool CanBeCancelled => false;


        // Constructor.

        public Condition(UnitController owner) : base(owner) {}


        // To string conversion.

        override public string ToString() {
            return "Unknown condition";
        }


        // Interaction methods.

        public IEnumerable<GameAction> Enter() {
            if (status != Status.Ready) {
                throw new System.InvalidOperationException("Condition has been entered already!");
            }
            status = Status.Entered;
            return OnEnter();
        }

        public IEnumerable<GameAction> Update() {
            if (status != Status.Entered) {
                throw new System.InvalidOperationException("Condition is not active!");
            }
            var actions = OnUpdate(out bool exited);
            if (exited) {
                var otherActions = OnFinish(StopType.Completed);
                actions = Enumerables.Unite(actions, otherActions);
                status = Status.Exited;
            }
            return actions;
        }

        public IEnumerable<GameAction> Stop(bool forceStop) {
            if (status != Status.Entered) {
                throw new System.InvalidOperationException("Condition is not active!");
            }
            if (!forceStop && !CanBeCancelled) {
                return Enumerables.GetNothing<GameAction>();
            }
            var actions = OnFinish(forceStop ? StopType.Forced : StopType.Cancelled);
            status = Status.Exited;
            return actions;
        }


        // Life cycle.

        protected abstract IEnumerable<GameAction> OnEnter();

        protected abstract IEnumerable<GameAction> OnUpdate(out bool exited);

        protected abstract IEnumerable<GameAction> OnFinish(StopType stopType);


    }

}