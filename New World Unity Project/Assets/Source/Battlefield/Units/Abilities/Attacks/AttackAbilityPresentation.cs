using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities.Attacks {
    
    public class AttackAbilityPresentation : AbilityPresentation<AttackAbility, AttackAbilityPresentation> {

        // Constructor.

        public AttackAbilityPresentation(AttackAbility presented) : base(presented) {}


        // Properties.

        public float AttackPower => Presented.AttackPower;
        public float AttackSpeed => Presented.AttackSpeed;
        public float AttackTime => Presented.AttackTime;


    }

}
