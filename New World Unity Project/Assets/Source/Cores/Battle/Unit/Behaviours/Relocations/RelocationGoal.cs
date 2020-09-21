using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Behaviours.Relocations {

    public class RelocationGoal : UnitGoal {

        // Fields.

        private readonly Vector3 destination;
        private readonly float admissibleDistance;


        // Constructor.

        public RelocationGoal(Vector3 destination, float admissibleDistance = 0.1f) {
            this.destination = destination;
            this.admissibleDistance = Mathf.Max(0f, admissibleDistance);
        }


        // Properties.

        public Vector3 Destination => destination;
        public float AdmissibleDistance => admissibleDistance;

        public override NamedId Id => NamedId.Get("RelocationGoal");


    }

}
