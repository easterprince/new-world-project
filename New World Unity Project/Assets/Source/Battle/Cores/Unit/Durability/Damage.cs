using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Durability {
    
    public struct Damage {

        // Fields.

        private float damageValue;


        // Constructors.

        public Damage(float damageValue) : this() {
            DamageValue = damageValue;
        }


        // Properties.

        public float DamageValue {
            get => damageValue;
            set => damageValue = Mathf.Max(damageValue, 0f);
        }

    
    }

}
