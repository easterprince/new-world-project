using NewWorld.Utilities;
using System;

namespace NewWorld.Battle.Cores.Unit.Conditions {

    public interface IConditionPresentation : IOwnerPointer {

        // Properties.

        bool Cancellable { get; }
        bool Finished { get; }
        float ConditionSpeed { get; }
        NamedId Id { get; }
        string Description { get; }


    }

}
