using NewWorld.Battle.Cores.Map;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Body {

    public class BodyModule : UnitModuleBase<BodyModule, BodyPresentation, UnitPresentation> {

        // Enumerator.

        public enum CollisionMode {
            Nothing = 0,
            Surface
        }


        // Fields.

        // Stable properties.
        private CollisionMode collidesWith;

        // Dynamic properties.
        private Vector3 position;
        private Vector3 velocity;
        private Quaternion rotation;


        // Constructors.

        public BodyModule() {
            collidesWith = CollisionMode.Nothing;
            position = Vector3.zero;
            velocity = Vector3.zero;
            rotation = Quaternion.identity;
        }

        public BodyModule(BodyModule other) {
            collidesWith = other.collidesWith;
            position = other.position;
            velocity = other.velocity;
            rotation = other.rotation;
        }


        // Properties.

        public CollisionMode CollidesWith {
            get => collidesWith;
            set => collidesWith = value;
        }

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

        public override BodyModule Clone() {
            return new BodyModule(this);
        }


        // Updating.

        public void Update() {
            ValidateContext();

            // Adjust position relatively to surface.
            if (collidesWith == CollisionMode.Surface) {
                LiftAboveSurface(ref position, Context.Map);
            }

        }


        // Movement applying.

        public void Move(MovementAction movement) {

            // Calculate new position.
            Vector3 newPosition = position + movement.PositionChange;
            if (collidesWith == CollisionMode.Surface && !(Context is null)) {
                LiftAboveSurface(ref newPosition, Context.Map);
            }

            // Adjust velocity.
            if (movement.AdjustVelocity && !(Context is null)) {
                velocity = (newPosition - position) / Context.GameTimeDelta;
            }

            // Adjust rotation.
            if (movement.AdjustRotation) {
                rotation = Quaternion.LookRotation(newPosition - position);
            }

            // Adjust position.
            position = newPosition;

        }

        public void Rotate(RotationAction rotation) {
            this.rotation = rotation.Rotation;
        }


        // Support methods.

        private void LiftAboveSurface(ref Vector3 position, MapPresentation map) {
            float surfaceHeight = map.GetHeight(position);
            position.y = Mathf.Max(position.y, surfaceHeight);
        }


    }

}
