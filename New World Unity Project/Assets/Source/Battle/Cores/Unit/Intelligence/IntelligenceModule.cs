using NewWorld.Battle.Cores.Unit.Abilities;
using NewWorld.Battle.Cores.Unit.Conditions;
using System.Collections.Generic;

namespace NewWorld.Battle.Cores.Unit.Intelligence {

    public class IntelligenceModule : UnitModuleBase<IntelligenceModule, IntelligencePresentation, UnitPresentation> {

        // Constructors.

        public IntelligenceModule() {}

        public IntelligenceModule(IntelligenceModule other) {}


        // Cloning.
        
        public override IntelligenceModule Clone() {
            return new IntelligenceModule(this);
        }


        // Presentation generation.

        private protected override IntelligencePresentation BuildPresentation() {
            return new IntelligencePresentation(this);
        }


        // Updating.

        public void Act() {}


    }

}
