﻿using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Abilities.Active {

    public abstract class ActiveAbility : Ability {

        protected enum StopType {
            Completed,
            Cancelled,
            Forced
        }


        // Fields.

        private bool isUsed = false;


        // Properties.

        public bool IsUsed => isUsed;

        public abstract bool CanBeCancelled { get; } 


        // Constructor.

        public ActiveAbility(UnitController owner) : base(owner) {}


        // Interaction methods.

        public IEnumerable<GameAction> Use(object parameterSet) {
            if (isUsed) {
                throw new System.InvalidOperationException("Ability is already used!");
            }
            isUsed = true;
            return OnStart(parameterSet);
        }

        sealed override public IEnumerable<GameAction> ReceiveActions() {
            if (!isUsed) {
                return Enumerables.GetNothing<GameAction>();
            }
            var actions = OnUpdate(out bool completed);
            if (completed) {
                var otherActions = OnFinish(StopType.Completed);
                actions = Enumerables.Unite(actions, otherActions);
                isUsed = false;
            }
            return actions;
        }

        public IEnumerable<GameAction> Stop(bool forceStop) {
            if (!isUsed || !forceStop && !CanBeCancelled) {
                return Enumerables.GetNothing<GameAction>();
            }
            var actions = OnFinish(forceStop ? StopType.Forced : StopType.Cancelled);
            isUsed = false;
            return actions;
        }


        // Response methods.

        protected abstract IEnumerable<GameAction> OnStart(object parameterSet);

        protected abstract IEnumerable<GameAction> OnUpdate(out bool completed);

        protected abstract IEnumerable<GameAction> OnFinish(StopType stopType);


    }

}