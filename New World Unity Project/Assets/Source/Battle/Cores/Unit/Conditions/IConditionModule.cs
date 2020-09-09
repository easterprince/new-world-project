using System;

namespace NewWorld.Battle.Cores.Unit.Conditions {
    
    public interface IConditionModule :
        IUnitModule<IConditionModule, IConditionPresentation, UnitPresentation>, IConditionPresentation {

        // Methods.

        void Act();


    }

}
