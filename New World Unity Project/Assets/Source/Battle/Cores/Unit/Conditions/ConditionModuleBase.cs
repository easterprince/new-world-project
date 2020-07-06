using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions {

    public abstract class ConditionModuleBase<TSelf, TPresentation> :
        UnitModuleBase<TSelf, TPresentation, UnitPresentation>, IConditionModule
        where TSelf : ConditionModuleBase<TSelf, TPresentation>
        where TPresentation : class, IOwnerPointer, IConditionPresentation {

        // Properties.

        public abstract bool Cancellable { get; }
        public abstract bool Finished { get; }
        public abstract string Description { get; }

        IConditionPresentation ICore<IConditionModule, IConditionPresentation>.Presentation => Presentation;


        // Updating.

        public abstract void Update();

        IConditionModule ICore<IConditionModule, IConditionPresentation>.Clone() => Clone();


    }

}
