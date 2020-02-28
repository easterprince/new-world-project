using UnityEngine;

namespace NewWorld.Battlefield.Units {
    
    public abstract class UnitModulePresentation<TModule, TSelf> : IUnitModulePresentation
        where TModule : UnitModule<TSelf>
        where TSelf : UnitModulePresentation<TModule, TSelf> {

        // Fields.

        private readonly TModule presented;


        // Properties.

        protected TModule Presented => presented;
        public UnitController Owner => presented.Owner;


        // Constructor.

        protected UnitModulePresentation(TModule presented) {
            this.presented = presented ?? throw new System.ArgumentNullException(nameof(presented));
        }


        // Methods.

        public bool BelongsTo(IUnitModule unitModule) {
            return presented == unitModule;
        }


    }

}
