using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability {
               
        // Methods.

        public abstract Intention ReceiveIntention();

        public abstract void AnswerIntention(Intention intention, bool satisfied);

    }

}