using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability<IntentionType>
        where IntentionType : Intention<IntentionType> {

        public abstract void SatisfyIntention(IntentionType intention);

    }

}