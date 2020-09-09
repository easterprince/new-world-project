using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions {

    public abstract class ConditionModuleBase<TSelf, TPresentation> :
        UnitModuleBase<TSelf, TPresentation, UnitPresentation>, IConditionModule
        where TSelf : IConditionModule
        where TPresentation : class, IConditionPresentation {

        // Fields.

        private bool finished = false;


        // Properties.

        public abstract bool Cancellable { get; }
        public abstract string Description { get; }
        public abstract NamedId Id { get; }
        public abstract float ConditionSpeed { get; }
        public bool Finished => finished;

        IConditionPresentation ICore<IConditionModule, IConditionPresentation>.Presentation => Presentation;


        // Updating.

        public void Act() {
            ValidateContext();
            if (finished) {
                return;
            }
            OnAct(out bool setFinished);
            finished = setFinished;
        }

        private protected abstract void OnAct(out bool finished);


        // Cloning.

        IConditionModule ICore<IConditionModule, IConditionPresentation>.Clone() => Clone();


    }

}
