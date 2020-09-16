using NewWorld.Cores.Battle.Unit.Conditions;
using NewWorld.Cores.Battle.Unit.Conditions.Attacks;
using NewWorld.Cores.Battle.Unit.Conditions.Motions;
using NewWorld.Cores.Battle.Unit.Conditions.Others;
using NewWorld.Cores.Battle.Unit.Durability;
using NewWorld.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NewWorld.Controllers.MetaData {
    
    public class ConditionDescriptor {

        // Delegate.

        private delegate string Extractor(IConditionPresentation condition);


        // Constant.

        private const char delimiter = '#';


        // Static fields and construction.

        private readonly static ConditionDescriptor defaultDescriptor;
        private readonly static Dictionary<string, Extractor> tokensToExtractor;

        static ConditionDescriptor() {

            // Initialize condition field extractors.
            tokensToExtractor = new Dictionary<string, Extractor> {
                ["DAMAGE_PER_SECOND"] =
                    (condition) => ((condition as IAttackConditionPresentation)?.DamagePerSecond ?? Damage.Zero).ToString(),
                ["ATTACK_TARGET"] =
                    (condition) => (condition as IAttackConditionPresentation)?.Target?.Name ?? "unknown",
                ["MOVEMENT_PER_SECOND"] =
                    (condition) => ((condition as IMotionConditionPresentation)?.MovementPerSecond ?? 0f).ToString(),
                ["MOTION_DESTINATION"] =
                    (condition) => ((condition as IMotionConditionPresentation)?.Destination)?.ToString() ?? "unknown",
                ["EXTINCTION_TIME"] =
                    (condition) => ((condition as ICollapseConditionPresentation)?.TimeUntilExtinction ?? float.PositiveInfinity).ToString()
            };

            // Set default descriptor.
            defaultDescriptor = new ConditionDescriptor(NamedId.Default, "Unknown condition", "Unknown condition", null);

        }


        // Static properties.

        public static ConditionDescriptor Default => defaultDescriptor;


        // Fields.

        private readonly NamedId id;
        private readonly string name;
        private readonly int? animationHash;
        private readonly List<Extractor> extractors;


        // Constructor.

        public ConditionDescriptor(NamedId id, string name, string descriptionTemplate, string animation) {
            
            // Assign fields.
            this.id = id;
            this.name = name;
            animationHash = (animation == null ? null : (int?) Animator.StringToHash(animation));

            // Compose composer.
            extractors = new List<Extractor>();
            var substrings = descriptionTemplate.Split(delimiter);
            foreach (var substring in substrings) {
                if (substring == "") {
                    continue;
                }
                if (tokensToExtractor.TryGetValue(substring, out var extractor)) {
                    extractors.Add(extractor);
                } else {
                    extractors.Add((condition) => substring);
                }
            }
            
        }

        // Properties.

        public NamedId Id => id;
        public string Name => name;
        public int? AnimationHash => animationHash;


        // Methods.

        public string ComposeDescription(IConditionPresentation condition) {
            var stringBuilder = new StringBuilder();
            foreach (var extractor in extractors) {
                stringBuilder.Append(extractor.Invoke(condition));
            }
            return stringBuilder.ToString();
        }


    }

}
