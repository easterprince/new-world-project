using NewWorld.Battle.Cores.Unit.Abilities;
using NewWorld.Battle.Cores.Unit.Abilities.Attacks;
using NewWorld.Battle.Cores.Unit.Abilities.Motions;
using System.Collections.Generic;

namespace NewWorld.Battle.Cores.Unit.AbilityCollection {

    public class AbilityCollectionPresentation : UnitModulePresentationBase<AbilityCollectionModule> {
        
        // Constructor.
        
        public AbilityCollectionPresentation(AbilityCollectionModule presented) : base(presented) {}


        // Properties.

        public List<IAbilityPresentation> Abilities => Presented.Abilities;
        public List<IMotionAbilityPresentation> Motions => Presented.Motions;
        public List<IAttackAbilityPresentation> Attacks => Presented.Attacks;


    }

}
