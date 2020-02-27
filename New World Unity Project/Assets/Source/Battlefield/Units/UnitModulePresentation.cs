using UnityEngine;

namespace NewWorld.Battlefield.Units {
    
    public abstract class UnitModulePresentation<ModuleType, PresentationType>
        where ModuleType : UnitModule<PresentationType>
        where PresentationType : UnitModulePresentation<ModuleType, PresentationType> {

        // Fields.

        private readonly ModuleType presented;


        // Properties.

        protected ModuleType Presented => presented;


        // Constructor.

        protected UnitModulePresentation(ModuleType presented) {
            this.presented = presented ?? throw new System.ArgumentNullException(nameof(presented));
        }


    }

}
