using System;

namespace NewWorld.Battle.Cores.Unit.Conditions {
   
    public interface IConditionPresentation : IOwnerPointer {

        // Properties.

        bool Cancellable { get; }
        bool Finished { get; }
        string Description { get; }


    }

}
