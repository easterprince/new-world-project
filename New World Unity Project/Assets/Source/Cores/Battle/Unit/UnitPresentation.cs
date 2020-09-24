using NewWorld.Cores.Battle.Unit.Abilities.Attacks;
using NewWorld.Cores.Battle.Unit.Abilities.Motions;
using NewWorld.Cores.Battle.Unit.AbilityCollection;
using NewWorld.Cores.Battle.Unit.Behaviours;
using NewWorld.Cores.Battle.Unit.Behaviours.Offensives;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
using NewWorld.Cores.Battle.Unit.Body;
using NewWorld.Cores.Battle.Unit.Conditions;
using NewWorld.Cores.Battle.Unit.Durability;
using NewWorld.Cores.Battle.Unit.Intelligence;

namespace NewWorld.Cores.Battle.Unit {

    public class UnitPresentation : UnitModulePresentationBase<UnitCore>,
        IReceptive<DamageCausingAction>, IReceptive<ConditionChangingAction>,
        IReceptive<MovementAction>, IReceptive<RotationAction>,
        IReceptive<AttackUsageAction>, IReceptive<MotionUsageAction>,
        IReceptive<GoalSettingAction> {

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
        public void PlanAction(ConditionChangingAction action) => Presented.PlanAction(action);
        public void PlanAction(MovementAction action) => Presented.PlanAction(action);
        public void PlanAction(RotationAction action) => Presented.PlanAction(action);
        public void PlanAction(AttackUsageAction action) => Presented.PlanAction(action);
        public void PlanAction(MotionUsageAction action) => Presented.PlanAction(action);
        public void PlanAction(GoalSettingAction action) => Presented.PlanAction(action);


    }

}
