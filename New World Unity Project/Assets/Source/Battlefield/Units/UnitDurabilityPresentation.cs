using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public class UnitDurabilityPresentation : UnitModulePresentation<UnitDurability, UnitDurabilityPresentation> {
    
        // Constructor.

        public UnitDurabilityPresentation(UnitDurability presented) : base(presented) {}


        // Properties.

        public float DurabilityLimit => Presented.DurabilityLimit;
        public float Durability => Presented.Durability;
        public bool Broken => Presented.Broken;


    }

}
