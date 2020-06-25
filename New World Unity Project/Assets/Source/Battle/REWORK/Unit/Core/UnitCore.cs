using NewWorld.Battlefield.Unit.Abilities;
using NewWorld.Battlefield.Unit.Conditions;
using NewWorld.Battlefield.Unit.Controller;
using NewWorld.Battlefield.Unit.Durability;
using NewWorld.Battlefield.Unit.Intelligence;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Unit.Core {

    public class UnitCore : UnitModuleBase<UnitCore, UnitController, UnitCorePresentation> {

        // Submodules.
        private UnitIntelligence intelligence = null;
        private UnitDurability durability = null;
        private UnitCondition currentCondition = null;
        private readonly HashSet<UnitAbility> abilities = new HashSet<UnitAbility>();

   
        // Properties.

        public UnitIntelligencePresentation Intelligence => intelligence.Presentation;
        public UnitDurabilityPresentation Durability => durability.Presentation;
        public UnitConditionPresentation CurrentCondition => currentCondition.Presentation;

        public IEnumerable<UnitAbilityPresentation> Abilities {
            get {
                var abilityPresentations = new UnitAbilityPresentation[abilities.Count];
                int inserted = 0;
                foreach (var ability in abilities) {
                    abilityPresentations[inserted] = ability.Presentation;
                    ++inserted;
                }
                return abilityPresentations;
            }
        }


        // Constructor.

        public UnitCore() {}


        // Presentation building.

        override private protected UnitCorePresentation BuildPresentation() {
            return new UnitCorePresentation(this);
        }


        // Updating.

        public void Update() {
            ValidateOwnership();

            // Update on durability.
            if (durability != null) {
                durability.Update(out UnitCondition forceCondition);
                if (forceCondition != null) {
                    ChangeCondition(forceCondition, forceChange: true);
                }
            }

            // Ask intelligence for orders.
            if (intelligence != null) {
                intelligence.Ask(out bool cancelCondition, out AbilityUsage? abilityUsage);
                if (cancelCondition) {
                    ChangeCondition(null, forceChange: false);
                }
                if (abilityUsage != null) {
                    UseAbility(abilityUsage.Value);
                }
            }

            // Update on condition.
            if (currentCondition != null) {
                if (currentCondition.Status == UnitCondition.StatusType.NotEntered) {
                    currentCondition.Enter();
                }
                if (currentCondition.Status != UnitCondition.StatusType.Exited) {
                    currentCondition.Update();
                }
                if (currentCondition.Status == UnitCondition.StatusType.Exited) {
                    currentCondition.Disconnect();
                    currentCondition = null;
                }
            }

        }


        // Regulating.



        public void ForceCondition(UnitCondition newCondition) => ChangeCondition(newCondition, forceChange: true);

        public void StopCondition(UnitConditionPresentation currentCondition) x


        // Support.

        private void ChangeCurrentCondition(UnitCondition newCondition, bool forceChange) {
            if (newCondition != null && (newCondition.Connected || newCondition.Status != UnitCondition.StatusType.NotEntered)) {
                return;
            }
            if (currentCondition != null) {
                currentCondition.Stop(forceChange);
                if (currentCondition.Status == UnitCondition.StatusType.Exited) {
                    currentCondition.Disconnect();
                    currentCondition = null;
                }
            }
            if (newCondition != null && currentCondition == null) {
                currentCondition = newCondition;
                currentCondition.Connect(this);
                currentCondition.Enter();
            }
        }

        private void StopCondition(UnitConditionPresentation conditionPresentation, bool forceStop) {
            if (currentCondition == null || currentCondition.Presentation != conditionPresentation) {
                return;
            }
            if (currentCondition.CanBeCancelled || forceStop) {
                if (currentCondition.Status == UnitCondition.StatusType.NotEntered) {
                    currentCondition.Disconnect();
                }
            }
        }

        private void UseAbility(AbilityUsage abilityUsage) {
            if (abilityUsage.Ability == null) {
                return;
            }
            UnitAbility ability = FindAbility(abilityUsage.Ability);
            if (ability != null) {
                var newCondition = ability.Use(abilityUsage.ParameterSet);
                ChangeCondition(newCondition, forceChange: false);
            }
        }

        private UnitAbility FindAbility(UnitAbilityPresentation abilityPresentation) {
            foreach (UnitAbility ability in abilities) {
                if (ability.Presentation == abilityPresentation) {
                    return ability;
                }
            }
            return null;
        } 


    }

}
