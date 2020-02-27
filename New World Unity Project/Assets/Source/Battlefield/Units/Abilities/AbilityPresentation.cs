using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities {
    
    public class AbilityPresentation<TAbility, TPresentation> : UnitModulePresentation<TAbility, TPresentation>, IAbilityPresentation
        where TAbility : Ability<TPresentation>
        where TPresentation : AbilityPresentation<TAbility, TPresentation> {

        // Constructors.

        protected AbilityPresentation(TAbility presented) : base(presented) {}


    }

}
