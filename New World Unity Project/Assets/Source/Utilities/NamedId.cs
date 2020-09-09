using System;
using System.Collections.Concurrent;

namespace NewWorld.Utilities {

    public struct NamedId : IEquatable<NamedId> {

        // Static fields.

        private const string defaultName = "Unknown";
        private readonly static object writeLock = new object();
        private readonly static ConcurrentDictionary<string, int> nameToId;

        static NamedId() {
            nameToId = new ConcurrentDictionary<string, int>();
            Get(defaultName);
        }


        // Fields.

        private int id;
        private string name;


        // Static properties.

        public static NamedId Default => new NamedId();


        // Properties.

        public string Name => name ?? defaultName;


        // Creation.

        public static NamedId Get(string name) {
            if (name == null || name.Length == 0) {
                name = defaultName;
            }
            if (!nameToId.TryGetValue(name, out int id)) {
                lock (writeLock) {
                    id = nameToId.Count;
                    id = nameToId.GetOrAdd(name, id);
                }
            }
            var namedId = new NamedId {
                id = id,
                name = name
            };
            return namedId;
        }


        // Comparison.

        public override int GetHashCode() {
            return id.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj is NamedId other) {
                return id == other.id;
            }
            return false;
        }

        public bool Equals(NamedId other) {
            return id == other.id;
        }

        public static bool operator ==(NamedId first, NamedId second) {
            return first.id == second.id;
        }

        public static bool operator !=(NamedId first, NamedId second) {
            return first.id != second.id;
        }


        // ToString().

        public override string ToString() {
            return $"{Name} ({id})";
        }


    }

}
