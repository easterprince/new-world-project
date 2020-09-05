using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Durability {
    
    public class DamageCausingAction : UnitAction {

        // Fields.

        private readonly Damage damage;


        // Constructors.

        public DamageCausingAction(Damage damage) {
            this.damage = damage;
        }


        // Properties.

        public Damage Damage => damage;


    }

}
