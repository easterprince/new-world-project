using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Unit.Core {

    public class UnitCore : UnitModule<UnitCore, UnitController> {

        /*
        // Game logic modules.
        private UnitIntelligence intelligence = null;
        private UnitDurability durability = null;
        private UnitCondition currentCondition = null;
        private readonly HashSet<UnitAbility> abilities = new HashSet<UnitAbility>();

        // Actions.
        private readonly List<GameAction> unprocessedActions = new List<GameAction>();


        // Properties.

        public UnitIntelligence Intelligence => intelligence;
        public UnitDurability Durability => durability;
        public UnitCondition CurrentCondition => currentCondition;
        public ICollection<UnitAbility> Abilities {
            get {
                var abilityPresentations = new UnitAbility[abilities.Count];
                abilities.CopyTo(abilityPresentations, 0);
                return abilityPresentations;
            }
        }


        // Informational methods.

        public TAbility GetAbility<TAbility>()
            where TAbility : UnitAbility {

            foreach (var ability in abilities) {
                if (ability is TAbility found) {
                    return found;
                }
            }

            return default;
        }


        // Life cycle.

        public IEnumerable<GameAction> Update(ownPassport) {


            // Process unprocessed actions.
            foreach (var gameAction in unprocessedActions) {
                ProcessGameAction(gameAction, true);
            }
            unprocessedActions.Clear();

            // Update on durability.
            if (durability != null) {
                durability.Update(ownPassport, out ForceCondition forceCondition);
                if (forceCondition != null) {
                    ProcessGameAction(forceCondition, false);
                }
            }

            // Ask behaviour for orders.
            if (intelligence != null) {
                intelligence.Act(ownPassport, out CancelCondition cancelCondition, out UseAbility useAbility);
                if (cancelCondition != null) {
                    ProcessGameAction(cancelCondition, false);
                }
                if (useAbility != null) {
                    ProcessGameAction(useAbility, false);
                }
            }

            // Receive and process actions from used ability.
            if (currentCondition != null) {
                var actions = currentCondition.Update(ownPassport);
                if (currentCondition.Status == UnitCondition.StatusType.Exited) {
                    currentCondition.Disconnect(ownPassport);
                    currentCondition = null;
                }
                ProcessGameActions(actions, false);
            }

        }
        */

    }

}
