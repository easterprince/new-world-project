using NewWorld.Battle.Cores.Battlefield;
using System;

namespace NewWorld.Battle.Cores {

    public abstract class CoreBase<TSelf, TPresentation> : ICore<TSelf, TPresentation>
        where TSelf : CoreBase<TSelf, TPresentation>
        where TPresentation : class, IContextPointer {

        // Fields.

        private TPresentation presentation;


        // Properties.

        public TPresentation Presentation {
            get {
                if (presentation is null) {
                    presentation = BuildPresentation();
                    if (presentation is null) {
                        throw new System.NullReferenceException("Presentation building method returned null reference.");
                    }
                }
                return presentation;
            }
        }

        public abstract BattlefieldPresentation Context { get; }


        // Presentation generation.

        private protected abstract TPresentation BuildPresentation();


        // Cloning.

        public abstract TSelf Clone();


        // Support methods.

        private protected void ValidateContext() {
            if (Context is null) {
                throw new NullReferenceException("Context must be not null.");
            }
        }

        private protected void PlanAction<TGameAction>(IResponsive<TGameAction> responsive, TGameAction action)
            where TGameAction : GameAction {

            if (responsive is null) {
                throw new ArgumentNullException(nameof(responsive));
            }
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            if (Context is null) {
                return;
            }
            Context.ExecuteAfterUpdate(() => responsive.ProcessAction(action));
        }

        private protected void PlanAction(Action action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            Context.ExecuteAfterUpdate(action);
        }


    }

}
