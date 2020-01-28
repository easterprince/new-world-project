using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;

namespace NewWorld.Battlefield.Units.Abilities.Active.Attacks {

    public abstract class AttackAbility : ActiveAbility {

        // Static.

        public static object FormParameterSet(UnitController target) {
            return target;
        }


        // Fields.

        private UnitController target;


        // Properties.

        public UnitController Target => target;


        // Constructor.

        public AttackAbility(UnitController owner) : base(owner) { }


        // Response methods.

        sealed override protected IEnumerable<GameAction> OnStart(object parameterSet) {
            if (!(parameterSet is UnitController target)) {
                throw new System.ArgumentException($"Parameter must be of class {nameof(UnitController)}.");
            }
            this.target = target;
            return OnStart();
        }

        protected abstract IEnumerable<GameAction> OnStart();

    }

}
