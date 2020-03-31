using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWorld.Battlefield.Unit {
    
    public class UnitModulePresentation<TModule>
        where TModule : UnitModule {

        // Fields.

        private TModule presented;


        // Properties.

        protected private TModule Presented => presented;


        // Constructor.

        public UnitModulePresentation(TModule presented) {
            this.presented = presented ?? throw new ArgumentNullException(nameof(presented));
        }

    
    }

}
