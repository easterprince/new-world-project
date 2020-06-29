using NewWorld.Battle.Cores.Battlefield;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Bodies {

    public class Body : UnitModuleCoreBase<Body, BodyPresentation, UnitPresentation> {

        // Fields.

        private Vector3 position = Vector3.zero;
        private Vector3 velocity = Vector3.zero;


        // Constructors.

        public Body() {}

        public Body(Body other) {
            position = other.position;
            velocity = other.velocity;
        }


        // Properties.

        public Vector3 Position {
            get => position;
            set => position = value;
        }


        public Vector3 Velocity {
            get => velocity;
            set => velocity = value;
        }


        // Presentation generation.
        
        private protected override BodyPresentation BuildPresentation() {
            return new BodyPresentation(this);
        }


        // Cloning.

        public override Body Clone() {
            return new Body(this);
        }


        // Updating.

        public void Update() {
            ValidateContext();
            throw new NotImplementedException();
        }


    }

}
