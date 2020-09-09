using NewWorld.Battle.Cores.Unit.Abilities;
using NewWorld.Battle.Cores.Unit.Abilities.Attacks;
using NewWorld.Battle.Cores.Unit.Abilities.Motions;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.AbilityCollection {

    public class AbilityCollectionModule :
        UnitModuleBase<AbilityCollectionModule, AbilityCollectionPresentation, UnitPresentation> {

        // Fields.

        private readonly Dictionary<IAbilityPresentation, IAbilityModule> abilities;
        private readonly Dictionary<IMotionAbilityPresentation, IMotionAbility> motions;
        private readonly Dictionary<IAttackAbilityPresentation, IAttackAbility> attacks;


        // Constructors.

        public AbilityCollectionModule() {
            abilities = new Dictionary<IAbilityPresentation, IAbilityModule>();
            motions = new Dictionary<IMotionAbilityPresentation, IMotionAbility>();
            attacks = new Dictionary<IAttackAbilityPresentation, IAttackAbility>();
        }

        public AbilityCollectionModule(AbilityCollectionModule other) : this() {
            foreach (var ability in other.abilities.Values) {
                AddAbility(ability);
            }
        }


        // Properties.

        public List<IAbilityPresentation> Abilities => new List<IAbilityPresentation>(abilities.Keys);
        public List<IMotionAbilityPresentation> Motions => new List<IMotionAbilityPresentation>(motions.Keys);
        public List<IAttackAbilityPresentation> Attacks => new List<IAttackAbilityPresentation>(attacks.Keys);


        // Cloning.

        public override AbilityCollectionModule Clone() {
            return new AbilityCollectionModule(this);
        }


        // Presentation generation.

        private protected override AbilityCollectionPresentation BuildPresentation() {
            return new AbilityCollectionPresentation(this);
        }


        // Modifying.

        public void AddAbility(IAbilityModule ability) {
            var cloned = ability.Clone();
            abilities[cloned.Presentation] = cloned;
            cloned.Connect(Presentation);
            if (cloned is IMotionAbility motion) {
                motions[motion.Presentation] = motion;
            }
            if (cloned is IAttackAbility attack) {
                attacks[attack.Presentation] = attack;
            }
        }


        // Usage.

        public void UseAbility(IAttackAbilityPresentation abilityPresentation, UnitPresentation target) {
            if (attacks.TryGetValue(abilityPresentation, out IAttackAbility ability)) {
                ability.Use(target);
            }
        }

        public void UseAbility(IMotionAbilityPresentation abilityPresentation, Vector3 destination) {
            if (motions.TryGetValue(abilityPresentation, out IMotionAbility ability)) {
                ability.Use(destination);
            }
        }


    }

}
