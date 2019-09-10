using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Abilities {

    public class Ability {

        // Fields.

        private bool enabled = false;


        // Properties.

        public bool Enabled {
            get => enabled;
            protected set => enabled = value;
        }


        // Methods.

        public void Disable() {
            if (enabled) {
                return;
            }
            enabled = false;
            OnDisable();
        }

        protected virtual void OnDisable() {}

        public virtual Intention ReceiveIntention() {
            return null;
        }

        public virtual void SatisfyIntention(Intention intention) {}

    }

}