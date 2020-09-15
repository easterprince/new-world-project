using NewWorld.Utilities;
using System;
using System.Collections.Generic;

namespace NewWorld.Controllers.MetaData {
    
    public static class Descriptors {

        // Dictionary wrapper class.

        public class DescriptorDictionaryWrapper<TDescriptor> : ObjectWrapper<Dictionary<NamedId, TDescriptor>> {

            // Constructor.

            public DescriptorDictionaryWrapper(Dictionary<NamedId, TDescriptor> wrapped) : base(wrapped) {
                if (!wrapped.ContainsKey(NamedId.Default)) {
                    throw new InvalidOperationException("Underlying dictionary should contain default key!");
                }
            }


            // Properties.

            public TDescriptor this[NamedId id] {
                get {
                    if (!Wrapped.TryGetValue(id, out var descriptor)) {
                        descriptor = Wrapped[NamedId.Default];
                    }
                    return descriptor;
                }
            }


        }


        // Fields.

        // Descriptors.
        private static readonly Dictionary<NamedId, ConditionDescriptor> conditions;

        // Descriptor dictionary wrappers.
        private static readonly DescriptorDictionaryWrapper<ConditionDescriptor> forConditions;


        // Constructor.

        static Descriptors() {

            // Initialize descriptors.
            conditions = new Dictionary<NamedId, ConditionDescriptor> {
                [NamedId.Default] = ConditionDescriptor.Default
            };

            // Initialize wrappers.
            forConditions = new DescriptorDictionaryWrapper<ConditionDescriptor>(conditions);

        }


        // Properties.

        public static DescriptorDictionaryWrapper<ConditionDescriptor> ForConditions => forConditions;


        // Methods.

        public static void AddDescriptors(IEnumerable<ConditionDescriptor> descriptors) {
            foreach (var descriptor in descriptors) {
                if (descriptor is null) {
                    continue;
                }
                conditions[descriptor.Id] = descriptor;
            }
        }


    }

}
