using NewWorld.Battle.Cores.Battlefield;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Bodies {

    public class Body : UnitModuleCoreBase<Body, BodyPresentation, UnitPresentation> {

        // Fields.

        // Main properties.
        private Vector3 position;
        private Vector3 velocity;
        private Quaternion rotation;

        // Planned changes.
        private float timeChange;


        // Constructors.

        public Body() {
            position = Vector3.zero;
            velocity = Vector3.zero;
            rotation = Quaternion.identity;
            timeChange = 0;
        }

        public Body(Body other) {
            position = other.position;
            velocity = other.velocity;
            rotation = other.rotation;
            timeChange = other.timeChange;
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

        public Quaternion Rotation {
            get => rotation;
            set => rotation = value;
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
            timeChange = Context.GameTimeDelta;
        }


        // Force applying.

        public void ApplyMovement(MovementAction movement) {
            velocity = movement.Velocity;
            position += velocity * timeChange;
            if (movement.AdjustRotation) {
                rotation = Quaternion.LookRotation(velocity);
            }
        }


    }

}
