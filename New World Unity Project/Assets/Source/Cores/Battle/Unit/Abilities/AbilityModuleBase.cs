using NewWorld.Cores.Battle.Unit.AbilityCollection;
using NewWorld.Utilities;

namespace NewWorld.Cores.Battle.Unit.Abilities {

    public abstract class AbilityModuleBase<TSelf, TAbilityPresentation> :
        UnitModuleBase<TSelf, TAbilityPresentation, AbilityCollectionPresentation>, IAbilityModule
        where TSelf : IAbilityModule
        where TAbilityPresentation : class, IAbilityPresentation {

        // Fields.

        private NamedId id;


        // Constructor.

        protected AbilityModuleBase(NamedId id) {
            this.id = id;
        }

        protected AbilityModuleBase(AbilityModuleBase<TSelf, TAbilityPresentation> other) {
            id = other.id;
        }


        // Properties.

        public NamedId Id => id;
        IAbilityPresentation ICore<IAbilityModule, IAbilityPresentation>.Presentation => Presentation;


        // Cloning.

        IAbilityModule ICore<IAbilityModule, IAbilityPresentation>.Clone() => Clone();


    }

}
