using NewWorld.Battle.Cores.Battlefield;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Bodies {

    public class Body : UnitModuleCoreBase<BodyPresentation, UnitPresentation> {

        // Fields.

        private Vector3 position;
        private Vector3 velocity;


        // Constructor.

        public Body(UnitPresentation parent) : base(parent) {}


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


        // Updating.

        public void Update() {
            ValidateContext();
            throw new NotImplementedException();
        }


    }

}
