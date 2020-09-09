using NewWorld.Cores.Battle.Unit.Abilities;
using NewWorld.Cores.Battle.Unit.Abilities.Attacks;
using NewWorld.Cores.Battle.Unit.Abilities.Motions;
using System.Collections.Generic;

namespace NewWorld.Cores.Battle.Unit.AbilityCollection {

    public class AbilityCollectionPresentation : UnitModulePresentationBase<AbilityCollectionModule> {

        // Constructor.

        public AbilityCollectionPresentation(AbilityCollectionModule presented) : base(presented) {}


        // Properties.

        public List<IAbilityPresentation> Abilities => Presented.Abilities;
        public List<IMotionAbilityPresentation> Motions => Presented.Motions;
        public List<IAttackAbilityPresentation> Attacks => Presented.Attacks;


    }

}
