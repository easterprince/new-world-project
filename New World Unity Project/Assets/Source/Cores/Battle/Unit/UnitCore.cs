using NewWorld.Cores.Battle.Unit.Abilities;
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
using NewWorld.Cores.Battle.UnitSystem;
using System;

namespace NewWorld.Cores.Battle.Unit {

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
            this.condition = condition?.Clone() ?? this.durability.CreateUsualCondition();
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
            if (!durability.Fallen) {
                intelligence.Act();
            }

            // Let condition act.
            condition.Act();
            if (condition.Finished) {
                ChangeCondition(null, forceChange: false);
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

        public void ChangeCondition(IConditionModule condition, bool forceChange) {
            if (!this.condition.Finished && !forceChange && !this.condition.Cancellable) {
                return;
            }
            if (condition is null) {
                condition = durability.CreateUsualCondition();
            }
            this.condition.Disconnect();
            this.condition = condition.Clone();
            this.condition.Connect(Presentation);
        }

        public void CauseDamage(Damage damage) {
            durability.CauseDamage(damage);
        }

        public void AddAbility(IAbilityModule ability) {
            abilityCollection.AddAbility(ability);
        }

        public void UseAbility(AttackUsageAction attackUsage) {
            if (attackUsage is null) {
                throw new ArgumentNullException();
            }
            abilityCollection.UseAbility(attackUsage.Ability, attackUsage.Target);
        }

        public void UseAbility(MotionUsageAction motionUsage) {
            if (motionUsage is null) {
                throw new ArgumentNullException();
            }
            abilityCollection.UseAbility(motionUsage.Ability, motionUsage.Destination);
        }

        public void SetGoal(RelocationGoal goal) {
            if (goal is null) {
                throw new ArgumentNullException();
            }
            intelligence.SetGoal(goal);
        }

        public void SetGoal(OffensiveGoal goal) {
            if (goal is null) {
                throw new ArgumentNullException();
            }
            intelligence.SetGoal(goal);
        }

        public void SetGoal(IdleGoal goal) {
            if (goal is null) {
                throw new ArgumentNullException();
            }
            intelligence.SetGoal(goal);
        }


    }

}
