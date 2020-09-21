using NewWorld.Utilities;
using System;
using System.Collections.Generic;

namespace NewWorld.Controllers.MetaData {

    public static class Descriptors {

        // Dictionary wrapper class.

        public class DescriptorDictionaryWrapper<TDescriptor> : ObjectWrapper<Dictionary<NamedId, TDescriptor>>
            where TDescriptor : DescriptorBase {

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


            // Methods.

            public void Add(IEnumerable<TDescriptor> descriptors) {
                foreach (var descriptor in descriptors) {
                    if (descriptor is null) {
                        continue;
                    }
                    Wrapped[descriptor.Id] = descriptor;
                }
            }


        }


        // Fields.

        // Descriptors.
        private static readonly Dictionary<NamedId, ConditionDescriptor> conditions;
        private static readonly Dictionary<NamedId, AbilityDescriptor> abilities;
        private static readonly Dictionary<NamedId, GoalDescriptor> goals;

        // Descriptor dictionary wrappers.
        private static readonly DescriptorDictionaryWrapper<ConditionDescriptor> forConditions;
        private static readonly DescriptorDictionaryWrapper<AbilityDescriptor> forAbilities;
        private static readonly DescriptorDictionaryWrapper<GoalDescriptor> forGoals;


        // Constructor.

        static Descriptors() {

            // Initialize descriptors.
            conditions = new Dictionary<NamedId, ConditionDescriptor> {
                [NamedId.Default] = ConditionDescriptor.Default
            };
            abilities = new Dictionary<NamedId, AbilityDescriptor> {
                [NamedId.Default] = AbilityDescriptor.Default
            };
            goals = new Dictionary<NamedId, GoalDescriptor> {
                [NamedId.Default] = GoalDescriptor.Default
            };

            // Initialize wrappers.
            forConditions = new DescriptorDictionaryWrapper<ConditionDescriptor>(conditions);
            forAbilities = new DescriptorDictionaryWrapper<AbilityDescriptor>(abilities);
            forGoals = new DescriptorDictionaryWrapper<GoalDescriptor>(goals);

        }


        // Properties.

        public static DescriptorDictionaryWrapper<ConditionDescriptor> ForConditions => forConditions;
        public static DescriptorDictionaryWrapper<AbilityDescriptor> ForAbilities => forAbilities;
        public static DescriptorDictionaryWrapper<GoalDescriptor> ForGoals => forGoals;


    }

}
