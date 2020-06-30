using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.Unit.Durability;
using NewWorld.Battle.Cores.UnitSystem;
using System;

namespace NewWorld.Battle.Cores.Unit {

    public class UnitCore : ConnectableCoreBase<UnitCore, UnitPresentation, UnitSystemPresentation>, IOwnerPointer {

        // Fields.

        private readonly BodyModule body;
        private readonly DurabilityModule durability;


        // Constructors.

        public UnitCore() {
            body = new BodyModule();
            body.Connect(Presentation);
            durability = new DurabilityModule();
            durability.Connect(Presentation);
        }

        public UnitCore(UnitCore other) {
            body = other.body.Clone();
            body.Connect(Presentation);
            durability = other.durability.Clone();
            durability.Connect(Presentation);
        }


        // Properties.

        public BodyPresentation Body => body.Presentation;
        public DurabilityPresentation Durability => durability.Presentation;
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
        }


    }

}
