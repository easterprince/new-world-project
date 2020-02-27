using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities {
    
    public class AbilityPresentation<AbilityType, PresentationType> : UnitModulePresentation<AbilityType, PresentationType>, IAbilityPresentation
        where AbilityType : Ability<PresentationType>
        where PresentationType : AbilityPresentation<AbilityType, PresentationType> {

        // Constructors.

        protected AbilityPresentation(AbilityType presented) : base(presented) {}


    }

}
