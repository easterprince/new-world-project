namespace NewWorld.Battle.Cores.Unit.Conditions {

    public class IdleCondition : ConditionModule {
       
        // Properties.
        
        public override string Description => "Idle.";


        // Cloning.

        public override ConditionModule Clone() {
            return new IdleCondition();
        }


        // Updating.

        public override void Update() {}


    }

}
