using System;

namespace NewWorld.Battle.Cores.Unit.Conditions {
    
    public interface IConditionModule :
        IConnectableCore<IConditionModule, IConditionPresentation, UnitPresentation>, IConditionPresentation {

        // Properties.

        void Act();


    }

}
