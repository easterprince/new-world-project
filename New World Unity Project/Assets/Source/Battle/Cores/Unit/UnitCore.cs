using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Conditions.Others;
using NewWorld.Battle.Cores.Unit.Durability;
using NewWorld.Battle.Cores.UnitSystem;
using System;

namespace NewWorld.Battle.Cores.Unit {

    public class UnitCore : ConnectableCoreBase<UnitCore, UnitPresentation, UnitSystemPresentation>, IOwnerPointer,
        IResponsive<ConditionCausingAction>, IResponsive<DamageCausingAction>,
        IResponsive<MovementAction>, IResponsive<RotationAction> {

        // Fields.

        private readonly BodyModule body;
        private readonly DurabilityModule durability;
        private IConditionModule condition;


        // Constructors.

        public UnitCore() {
            body = new BodyModule();
            body.Connect(Presentation);
            durability = new DurabilityModule();
            durability.Connect(Presentation);
            condition = new IdleCondition();
            condition.Connect(Presentation);
        }

        public UnitCore(UnitCore other) {
            body = other.body.Clone();
            body.Connect(Presentation);
            durability = other.durability.Clone();
            durability.Connect(Presentation);
            condition = other.condition.Clone();
            condition.Connect(Presentation);
        }


        // Properties.

        public BodyPresentation Body => body.Presentation;
        public DurabilityPresentation Durability => durability.Presentation;
        public IConditionPresentation Condition => condition.Presentation;
        public UnitPresentation Owner => Presentation;


        // Presentation generation.

        private protected override UnitPresentation BuildPresentation() {
            return new UnitPresentation(this);
        }


        // Cloning.

        public override UnitCore Clone() {
            return new UnitCore(this);
        }


        // Updating.

        public void Update() {
            ValidateContext();
            body.Update();
            durability.Update();
            condition.Update();
        }


        // Modifying methods.

        public void CauseCondition(IConditionModule condition) {
            if (condition is null) {
                throw new ArgumentNullException(nameof(condition));
            }
            this.condition.Disconnect();
            this.condition = condition.Clone();
            this.condition.Connect(Presentation);
        }

        public void CauseDamage(DamageCausingAction damageCausing) {
            if (damageCausing is null) {
                throw new ArgumentNullException(nameof(damageCausing));
            }
            durability.CauseDamage(damageCausing.Damage);
        }


        // Action processing.

        public void ProcessAction(ConditionCausingAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            CauseCondition(action.Condition);
        }

        public void ProcessAction(DamageCausingAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            durability.CauseDamage(action.Damage);
        }

        public void ProcessAction(MovementAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            body.ApplyMovement(action);
        }

        public void ProcessAction(RotationAction action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            body.Rotate(action);
        }

        public void PlanAction(ConditionCausingAction action) => PlanAction(this, action);
        public void PlanAction(DamageCausingAction action) => PlanAction(this, action);
        public void PlanAction(MovementAction action) => PlanAction(this, action);
        public void PlanAction(RotationAction action) => PlanAction(this, action);


    }

}
