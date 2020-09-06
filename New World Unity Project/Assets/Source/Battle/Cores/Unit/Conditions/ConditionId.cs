using System;
using System.Collections.Generic;

namespace NewWorld.Battle.Cores.Unit.Conditions {
    
    public struct ConditionId : IEquatable<ConditionId> {

        // Static fields.

        private const string defaultStringId = "Idle";
        private readonly static Dictionary<string, int> stringToInt;

        static ConditionId() {
            stringToInt = new Dictionary<string, int>();
            Get(defaultStringId);
        }


        // Fields.

        private int intId;
        private string stringId;


        // Static properties.

        public static ConditionId Default => new ConditionId();


        // Properties.

        public string StringId => stringId ?? defaultStringId;


        // Creation.

        public static ConditionId Get(string stringId) {
            if (!stringToInt.TryGetValue(stringId, out int intId)) {
                intId = stringToInt.Count;
                stringToInt[stringId] = intId;
            }
            var id = new ConditionId {
                intId = intId,
                stringId = stringId
            };
            return id;
        }


        // Comparison.

        public override int GetHashCode() {
            return intId.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj is ConditionId other) {
                return intId == other.intId;
            }
            return false;
        }

        public bool Equals(ConditionId other) {
            return intId == other.intId;
        }

        public static bool operator == (ConditionId first, ConditionId second) {
            return first.intId == second.intId;
        }

        public static bool operator != (ConditionId first, ConditionId second) {
            return first.intId != second.intId;
        }


        // ToString().

        public override string ToString() {
            return $"{StringId} ({intId})";
        }


    }

}
