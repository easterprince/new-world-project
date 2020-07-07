﻿using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Unit.Abilities;
using NewWorld.Battle.Cores.Unit.Abilities.Attacks;
using NewWorld.Battle.Cores.Unit.Abilities.Motions;
using NewWorld.Battle.Cores.Unit.AbilityCollection;
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

        public UnitCore() {

            // Modules.
            body = new BodyModule();
            body.Connect(Presentation);
            durability = new DurabilityModule();
            durability.Connect(Presentation);
            condition = new IdleCondition();
            condition.Connect(Presentation);
            abilityCollection = new AbilityCollectionModule();
            abilityCollection.Connect(Presentation);
            intelligence = new IntelligenceModule();
            intelligence.Connect(Presentation);

            // Other.
            name = "Unit";

        }

        public UnitCore(UnitCore other) {

            // Modules.
            body = other.body.Clone();
            body.Connect(Presentation);
            durability = other.durability.Clone();
            durability.Connect(Presentation);
            condition = other.condition.Clone();
            condition.Connect(Presentation);
            abilityCollection = other.abilityCollection.Clone();
            abilityCollection.Connect(Presentation);
            intelligence = other.intelligence.Clone();
            intelligence.Connect(Presentation);

            // Other.
            name = other.name;

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

            // Update durability.
            durability.Update();
            if (condition.Finished) {
                condition.Disconnect();
                condition = new IdleCondition();
                condition.Connect(Presentation);
            }

            // Update condition.
            condition.Update();

            // Update intelligence.
            intelligence.Act();

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


    }

}
