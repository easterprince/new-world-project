using NewWorld.Cores.Battle.Unit.Conditions;
using NewWorld.Cores.Battle.Unit.Conditions.Attacks;
using NewWorld.Cores.Battle.Unit.Conditions.Motions;
using NewWorld.Cores.Battle.Unit.Durability;
using NewWorld.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NewWorld.Controllers.MetaData {
    
    public class ConditionDescriptor {

        // Delegate.

        private delegate string Extractor(IConditionPresentation condition);


        // Constant.

        private const char delimiter = '#';


        // Static.

        private readonly static Dictionary<string, Extractor> tokensToExtractor;

        static ConditionDescriptor() {
            tokensToExtractor = new Dictionary<string, Extractor> {
                ["DAMAGE_PER_SECOND"] =
                    (condition) => ((condition as IAttackConditionPresentation)?.DamagePerSecond ?? Damage.Zero).ToString(),
                ["ATTACK_TARGET"] =
                    (condition) => (condition as IAttackConditionPresentation)?.Target?.Name ?? "unknown",
                ["MOVEMENT_PER_SECOND"] =
                    (condition) => ((condition as IMotionConditionPresentation)?.MovementPerSecond ?? 0f).ToString(),
                ["MOTION_DESTINATION"] =
                    (condition) => ((condition as IMotionConditionPresentation)?.Destination)?.ToString() ?? "unknown"
            };
        }


        // Fields.

        private readonly NamedId id;
        private readonly string name;
        private readonly List<Extractor> extractors;


        // Constructor.

        public ConditionDescriptor(NamedId id, string name, string descriptionTemplate) {
            
            // Assign fields.
            this.id = id;
            this.name = name;

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
