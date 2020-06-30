using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Durability {
    
    public class DamageCausingAction : UnitAction {

        // Fields.

        private readonly float damage;


        // Constructors.

        public DamageCausingAction(float damage) {
            this.damage = Mathf.Max(damage, 0);
        }


        // Properties.

        public float Damage => damage;


    }

}
