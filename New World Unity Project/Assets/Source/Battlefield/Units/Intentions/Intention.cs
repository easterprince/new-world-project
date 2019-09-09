using NewWorld.Battlefield.Units;
using NewWorld.Battlefield.Units.Abilities;

namespace NewWorld.Battlefield.Units.Intentions {

    public abstract class Intention<IntentionType>
        where IntentionType : Intention<IntentionType> {

        // Fields.

        private readonly Ability<IntentionType> source;


        // Constructor.

        public Intention(Ability<IntentionType> source) {
            this.source = source;
        }


        // Satisfaction.
        
        public void Satisfy() {
            source.SatisfyIntention(this as IntentionType);
        }



    }

}
