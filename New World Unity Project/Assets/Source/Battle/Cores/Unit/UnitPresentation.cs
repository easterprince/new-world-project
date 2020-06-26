namespace NewWorld.Battle.Cores.Unit {

    public class UnitPresentation : ReceptivePresentationBase<UnitCore, UnitAction>, IParentPresentation {

        // Properties.

        UnitPresentation IParentPresentation.Owner => this;


        // Constructor.

        public UnitPresentation(UnitCore presented) : base(presented) {}


    }

}
