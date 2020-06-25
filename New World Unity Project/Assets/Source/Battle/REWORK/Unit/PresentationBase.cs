using UnityEngine;

namespace NewWorld.Battlefield.Unit {
    
    public class PresentationBase<TPresented> {

        // Fields.

        private TPresented presented;


        // Properties.

        protected private TPresented Presented => presented;


        // Constructor.

        public PresentationBase(TPresented presented) {
            this.presented = presented ?? throw new System.ArgumentNullException(nameof(presented));
        }

    
    }

}
