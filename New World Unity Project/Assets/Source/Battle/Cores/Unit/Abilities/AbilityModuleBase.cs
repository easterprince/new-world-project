﻿using System;

namespace NewWorld.Battle.Cores.Unit.Abilities {

    public abstract class AbilityModuleBase<TSelf, TAbilityPresentation> :
        UnitModuleBase<TSelf, TAbilityPresentation, UnitPresentation>, IAbilityModule
        where TSelf : AbilityModuleBase<TSelf, TAbilityPresentation>
        where TAbilityPresentation : IAbilityPresentation {

        // Properties.

        public abstract string Description { get; }

        IAbilityPresentation ICore<IAbilityModule, IAbilityPresentation>.Presentation => Presentation;


        // Cloning.

        IAbilityModule ICore<IAbilityModule, IAbilityPresentation>.Clone() => Clone();


    }

}
