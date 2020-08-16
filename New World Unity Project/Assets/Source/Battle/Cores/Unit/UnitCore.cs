using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Unit.Abilities;
using NewWorld.Battle.Cores.Unit.Abilities.Attacks;
using NewWorld.Battle.Cores.Unit.Abilities.Motions;
using NewWorld.Battle.Cores.Unit.AbilityCollection;
using NewWorld.Battle.Cores.Unit.Behaviours.Relocations;
using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Conditions.Others;
using NewWorld.Battle.Cores.Unit.Durability;
using NewWorld.Battle.Cores.Unit.Intelligence;
using NewWorld.Battle.Cores.UnitSystem;
using System;
using System.Collections.Generic;
using System.Data;

namespace NewWorld.Battle.Cores.Unit {

    public partial class UnitCore : ConnectableCoreBase<UnitCore, UnitPresentation, UnitSystemPresentation>, IOwnerPointer {

        // Fields.

        // Modules.
        private readonly BodyModule body;
        private readonly DurabilityModule durability;
        private IConditionModule condition;
        private readonly AbilityCollectionModule abilityCollection;
        private readonly IntelligenceModule intelligence;

        // Other.
        private string name;


        // Constructors.

        public UnitCore(
            BodyModule body = null,
            DurabilityModule durability = null,
            IConditionModule condition = null,
            AbilityCollectionModule abilityCollection = null,
            IntelligenceModule intelligence = null) {

            // Modules.
            this.body = body?.Clone() ?? new BodyModule();
            this.body.Connect(Presentation);
            this.durability = durability?.Clone() ?? new DurabilityModule();
            this.durability.Connect(Presentation);
            this.condition = condition?.Clone() ?? new IdleCondition();
            this.condition.Connect(Presentation);
            this.abilityCollection = abilityCollection?.Clone() ?? new AbilityCollectionModule();
            this.abilityCollection.Connect(Presentation);
            this.intelligence = intelligence?.Clone() ?? new IntelligenceModule();
            this.intelligence.Connect(Presentation);

        }

        public UnitCore(UnitCore other) :
            this(other?.body, other?.durability, other?.condition, other?.abilityCollection, other?.intelligence) {

            // Other.
            name = other?.name ?? "Unit";

        }


        // Properties.

        public BodyPresentation Body => body.Presentation;
        public DurabilityPresentation Durability => durability.Presentation;
        public IConditionPresentation Condition => condition.Presentation;
        public AbilityCollectionPresentation AbilityCollection => abilityCollection.Presentation;
        public IntelligencePresentation Intelligence => intelligence.Presentation;

        public string Name {
            get => name;
            set => name = value ?? "";
        }

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

            // Update body.
            body.Update();

        }

        public void Act() {
            ValidateContext();

            // Let durability act.
            durability.Act();

            // Let intelligence act.
            intelligence.Act();

            // Let condition act.
            condition.Act();
            if (condition.Finished) {
                condition.Disconnect();
                condition = new IdleCondition();
                condition.Connect(Presentation);
            }

        }


        // Modifying methods.

        public void Move(MovementAction movement) {
            if (movement is null) {
                throw new ArgumentNullException(nameof(movement));
            }
            body.Move(movement);
        }

        public void Rotate(RotationAction rotation) {
            if (rotation is null) {
                throw new ArgumentNullException(nameof(rotation));
            }
            body.Rotate(rotation);
        }

        public void CauseCondition(IConditionModule condition) {
            if (condition is null) {
                throw new ArgumentNullException(nameof(condition));
            }
            this.condition.Disconnect();
            this.condition = condition.Clone();
            this.condition.Connect(Presentation);
        }

        public void CancelCondition() {
            if (condition.Cancellable) {
                condition.Disconnect();
                condition = new IdleCondition();
                condition.Connect(Presentation);
            }
        }

        public void CauseDamage(Damage damage) {
            durability.CauseDamage(damage);
        }

        public void AddAbility(AttackAbility ability) {
            abilityCollection.AddAbility(ability);
        }

        public void AddAbility(MotionAbility ability) {
            abilityCollection.AddAbility(ability);
        }

        public void UseAbility(AttackUsageAction attackUsage) {
            if (attackUsage is null) {
                throw new ArgumentNullException();
            }
            abilityCollection.UseAbility(attackUsage);
        }

        public void UseAbility(MotionUsageAction motionUsage) {
            if (motionUsage is null) {
                throw new ArgumentNullException();
            }
            abilityCollection.UseAbility(motionUsage);
        }

        public void SetGoal(RelocationGoal goal) {
            if (goal is null) {
                throw new ArgumentNullException();
            }
            intelligence.SetGoal(goal);
        }


    }

}
