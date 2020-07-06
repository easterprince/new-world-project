﻿using NewWorld.Battle.Cores.Unit.AbilityCollection;
using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Durability;
using NewWorld.Battle.Cores.Unit.Intelligence;

namespace NewWorld.Battle.Cores.Unit {

    public class UnitPresentation : UnitModulePresentationBase<UnitCore>,
        IReceptive<DamageCausingAction>, IReceptive<ConditionCausingAction>,
        IReceptive<MovementAction>, IReceptive<RotationAction> {

        // Constructor.

        public UnitPresentation(UnitCore presented) : base(presented) {}


        // Properties.

        public BodyPresentation Body => Presented.Body;
        public DurabilityPresentation Durability => Presented.Durability;
        public IConditionPresentation Condition => Presented.Condition;
        public AbilityCollectionPresentation AbilityCollection => Presented.AbilityCollection;
        public IntelligencePresentation Intelligence => Presented.Intelligence;

        public string Name => Presented.Name;


        // Action processing.

        public void PlanAction(DamageCausingAction action) => Presented.PlanAction(action);
        public void PlanAction(ConditionCausingAction action) => Presented.PlanAction(action);
        public void PlanAction(MovementAction action) => Presented.PlanAction(action);
        public void PlanAction(RotationAction action) => Presented.PlanAction(action);


    }

}
