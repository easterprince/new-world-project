using UnityEngine;

namespace NewWorld.Utilities {
    
    public struct TimeAccumulator {

        // Fields.

        private readonly float limit;
        private float accumulated;


        // Constructors.

        public TimeAccumulator(float limit, float accumulated = 0) {
            this.limit = Floats.SetPositive(limit);
            this.accumulated = Floats.LimitPositive(accumulated, this.limit);
        }

        public TimeAccumulator(TimeAccumulator other) {
            limit = other.limit;
            accumulated = other.accumulated;
        }


        // Properties.

        public float Limit => limit;
        public float Accumulated => accumulated;


        // Methods.

        public void Add(float timeDelta, out bool complete) {
            timeDelta = Floats.SetPositive(timeDelta);
            accumulated += timeDelta;
            complete = (accumulated >= limit);
            if (complete) {
                accumulated = Floats.LimitPositive(accumulated - limit, limit);
            }
        }

        public void Reset() {
            accumulated = 0;
        }

    
    }

}
