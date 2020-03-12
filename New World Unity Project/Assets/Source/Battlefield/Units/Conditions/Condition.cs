using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Conditions {
 
    public abstract class Condition : UnitModule<UnitController> {

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
        public virtual string Description => "Unknown condition";


        // Interaction methods.

        public IEnumerable<GameAction> Enter(ParentPassport<UnitController> parentPassport) {
            Connect(parentPassport);
            return OnEnter();
        }

        public IEnumerable<GameAction> Update(ParentPassport<UnitController> parentPassport) {
            ValidatePassport(parentPassport);
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

        public IEnumerable<GameAction> Stop(ParentPassport<UnitController> parentPassport, bool forceStop) {
            ValidatePassport(parentPassport);
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