using NewWorld.Utilities;

namespace NewWorld.Cores.Battle.Unit.Conditions {

    public interface IConditionPresentation : IOwnerPointer {

        // Properties.

        bool Cancellable { get; }
        bool Finished { get; }
        float ConditionSpeed { get; }
        NamedId Id { get; }


    }

}
