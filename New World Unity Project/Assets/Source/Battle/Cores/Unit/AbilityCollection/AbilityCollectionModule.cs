using NewWorld.Battle.Cores.Unit.Abilities;
using NewWorld.Battle.Cores.Unit.Abilities.Attacks;
using NewWorld.Battle.Cores.Unit.Abilities.Motions;
using NewWorld.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NewWorld.Battle.Cores.Unit.AbilityCollection {

    public class AbilityCollectionModule :
        UnitModuleBase<AbilityCollectionModule, AbilityCollectionPresentation, UnitPresentation> {

        // Fields.

        private readonly Dictionary<IAbilityPresentation, IAbilityModule> abilities;
        private readonly Dictionary<MotionAbilityPresentation, MotionAbility> motions;
        private readonly Dictionary<AttackAbilityPresentation, AttackAbility> attacks;


        // Constructors.

        public AbilityCollectionModule() {
            abilities = new Dictionary<IAbilityPresentation, IAbilityModule>();
            motions = new Dictionary<MotionAbilityPresentation, MotionAbility>();
            attacks = new Dictionary<AttackAbilityPresentation, AttackAbility>();
        }

        public AbilityCollectionModule(AbilityCollectionModule other) : this() {
            foreach (var ability in other.attacks.Values) {
                AddAbility(ability);
            }
            foreach (var ability in other.motions.Values) {
                AddAbility(ability);
            }
        }


        // Properties.

        public List<IAbilityPresentation> Abilities => new List<IAbilityPresentation>(abilities.Keys);
        public List<MotionAbilityPresentation> Motions => new List<MotionAbilityPresentation>(motions.Keys);
        public List<AttackAbilityPresentation> Attacks => new List<AttackAbilityPresentation>(attacks.Keys);


        // Cloning.

        public override AbilityCollectionModule Clone() {
            return new AbilityCollectionModule(this);
        }


        // Presentation generation.

        private protected override AbilityCollectionPresentation BuildPresentation() {
            return new AbilityCollectionPresentation(this);
        }


        // Modifying.

        public void AddAbility(AttackAbility ability) {
            var cloned = ability.Clone();
            abilities[cloned.Presentation] = cloned;
            attacks[cloned.Presentation] = cloned;
            cloned.Connect(Presentation);
        }

        public void AddAbility(MotionAbility ability) {
            var cloned = ability.Clone();
            abilities[cloned.Presentation] = cloned;
            motions[cloned.Presentation] = cloned;
            cloned.Connect(Presentation);
        }


        // Usage.

        public void UseAbility(AttackUsageAction usage) {
            ValidateContext();
            if (attacks.TryGetValue(usage.Ability, out AttackAbility ability)) {
                ability.Use(usage.Target);
            }
        }

        public void UseAbility(MotionUsageAction usage) {
            ValidateContext();
            if (motions.TryGetValue(usage.Ability, out MotionAbility ability)) {
                ability.Use(usage.Destination);
            }
        }


    }

}
