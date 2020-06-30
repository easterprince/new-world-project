namespace NewWorld.Battle.Cores.Unit.Durability {

    public class DurabilityPresentation : UnitPresentationBase<DurabilityModule> {
        
        // Constructor.
        
        public DurabilityPresentation(DurabilityModule presented) : base(presented) {}


        // Properties.

        public float DurabilityLimit => Presented.DurabilityLimit;
        public float Durability => Presented.Durability;
        public bool Fallen => Presented.Fallen;


    }

}
