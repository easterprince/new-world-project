using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions {

    public abstract class ConditionModule : UnitModuleBase<ConditionModule, ConditionPresentation, UnitPresentation> {

        // Properties.

        public virtual Vector3? MotionTarget => null;
        public virtual Vector3? Velocity => null;
        public virtual UnitPresentation? AttackTarget => null;
        public virtual float? DamagePerSecond => null;
        public virtual float? TimeUntilExtinction => null;
        public abstract string Description { get; }


        // Updating.

        public abstract void Update();


        // Presentation generation.

        private protected override ConditionPresentation BuildPresentation() {
            return new ConditionPresentation(this);
        }


    }

}
