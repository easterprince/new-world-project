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
            foreach (var ability in other.abilities.Values) {
                AddAbility(ability);
            }
        }


        // Properties.

        public IEnumerable<IAbilityPresentation> Abilities => Enumerables.GetAll(abilities.Keys);
        public IEnumerable<MotionAbilityPresentation> Motions => Enumerables.GetAll(motions.Keys);
        public IEnumerable<AttackAbilityPresentation> Attacks => Enumerables.GetAll(attacks.Keys);


        // Cloning.

        public override AbilityCollectionModule Clone() {
            throw new System.NotImplementedException();
        }


        // Presentation generation.

        private protected override AbilityCollectionPresentation BuildPresentation() {
            return new AbilityCollectionPresentation(this);
        }


        // Modifying.

        public void AddAbility(IAbilityModule ability) {
            var cloned = ability.Clone();
            abilities[cloned.Presentation] = cloned;
            if (cloned is MotionAbility motion) {
                motions[motion.Presentation] = motion;
            }
            if (cloned is AttackAbility attack) {
                attacks[attack.Presentation] = attack;
            }
        }


    }

}
