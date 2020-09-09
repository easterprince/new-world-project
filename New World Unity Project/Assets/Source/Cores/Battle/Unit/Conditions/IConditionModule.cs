using System;

namespace NewWorld.Cores.Battle.Unit.Conditions {
    
    public interface IConditionModule :
        IUnitModule<IConditionModule, IConditionPresentation, UnitPresentation>, IConditionPresentation {

        // Methods.

        void Act();


    }

}
