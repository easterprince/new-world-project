using NewWorld.Battle.Cores.Battlefield;

namespace NewWorld.Battle.Cores {
    
    public abstract class ReceptiveCoreBase<TSelf, TPresentation> : CoreBase<TSelf, TPresentation>, IReceptiveCore
        where TSelf : ReceptiveCoreBase<TSelf, TPresentation>
        where TPresentation : ReceptivePresentationBase<TPresentation, TSelf> {


        // Fields.

        private ActionPlanner planner;


        // Properties.

        IReceptivePresentation IReceptiveCore.ReceptivePresentation => Presentation;


        // Constructor.

        public ReceptiveCoreBase(ActionPlanner planner) {
            if (planner is null) {
                throw new System.ArgumentNullException(nameof(planner));
            }
            this.planner = planner;
        }


        // Methods.

        public abstract void ProcessAction(IGameAction action);

        public void AddAction(IGameAction action) {
            planner.AddAction(this, action);
        }


    }

}
