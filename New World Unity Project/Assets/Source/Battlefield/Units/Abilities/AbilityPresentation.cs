using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities {
    
    public class AbilityPresentation<TAbility, TSelf> : UnitModulePresentation<TAbility, TSelf>, IAbilityPresentation
        where TAbility : Ability<TSelf>
        where TSelf : AbilityPresentation<TAbility, TSelf> {

        // Constructors.

        protected AbilityPresentation(TAbility presented) : base(presented) {}


    }

}
