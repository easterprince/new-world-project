using NewWorld.Battle.Cores.Battlefield;
using System;
using System.Collections.Generic;

namespace NewWorld.Battle.Cores {
    
    public abstract class ReceptiveCoreBase<TSelf, TPresentation, TGameActionSeries> : CoreBase<TSelf, TPresentation>, IReceptiveCore<TGameActionSeries>
        where TSelf : ReceptiveCoreBase<TSelf, TPresentation, TGameActionSeries>
        where TPresentation : ReceptivePresentationBase<TSelf, TGameActionSeries>
        where TGameActionSeries : GameAction {

        // Fields.

        private ActionPlanner planner;


        // Properties.

        IReceptivePresentation<TGameActionSeries> IReceptiveCore<TGameActionSeries>.ReceptivePresentation => Presentation;


        // Constructor.

        public ReceptiveCoreBase(ActionPlanner planner) {
            this.planner = planner ?? throw new ArgumentNullException(nameof(planner));
        }


        // Methods.

        public void AddAction<TAction>(TAction action)
            where TAction : TGameActionSeries {

            if (this is IResponsiveCore<TAction> itself) {
                planner.AddAction(() => itself.ProcessAction(action));
            }
        }


    }

}
