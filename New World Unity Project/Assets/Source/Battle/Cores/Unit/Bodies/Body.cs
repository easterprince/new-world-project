namespace NewWorld.Battle.Cores.Unit.Bodies {

    public class Body : UnitModuleBase<Body, BodyPresentation, UnitPresentation> {
        
        // Presentation generation.
        
        private protected override BodyPresentation BuildPresentation() {
            return new BodyPresentation(this);
        }


    }

}
