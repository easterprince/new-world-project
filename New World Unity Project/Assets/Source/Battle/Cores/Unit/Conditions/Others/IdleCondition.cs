namespace NewWorld.Battle.Cores.Unit.Conditions.Others {

    public class IdleCondition :
        ConditionModuleBase<IdleCondition, ConditionPresentationBase<IdleCondition>> {

        // Properties.

        public override string Description => "Idle.";


        // Cloning.

        public override IdleCondition Clone() {
            return new IdleCondition();
        }


        // Presentation generation.

        private protected override ConditionPresentationBase<IdleCondition> BuildPresentation() {
            return new ConditionPresentationBase<IdleCondition>(this);
        }


        // Updating.

        public override void Update() {}
    
    
    }

}
