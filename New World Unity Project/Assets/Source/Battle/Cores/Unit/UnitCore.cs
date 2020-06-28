using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Unit.Bodies;
using NewWorld.Battle.Cores.UnitSystem;
using System;

namespace NewWorld.Battle.Cores.Unit {

    public class UnitCore : ConnectableCoreBase<UnitPresentation, UnitSystemPresentation>, IOwnerPointer {

        // Fields.

        private readonly Body body;


        // Properties.

        public BodyPresentation Body => body.Presentation;
        public UnitPresentation Owner => Presentation;


        // Constructor.

        public UnitCore(UnitSystemPresentation parent) : base(parent) {
            body = new Body(Presentation);
        }


        // Presentation generation.

        private protected override UnitPresentation BuildPresentation() {
            return new UnitPresentation(this);
        }


        // Updating.

        public void Update() {
            ValidateContext();
            body.Update();
        }


    }

}
