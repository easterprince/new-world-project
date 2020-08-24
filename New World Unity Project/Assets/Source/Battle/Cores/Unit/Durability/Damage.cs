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
            set => damageValue = Mathf.Max(value);
        }


        // Operators.

        public static Damage operator * (float factor, Damage damage) {
            return new Damage(factor * damage.damageValue);
        }

        public static Damage operator * (Damage damage, float factor) {
            return new Damage(damage.damageValue * factor);
        }

        public static Damage operator / (Damage damage, float divisor) {
            return new Damage(damage.damageValue / divisor);
        }


    }

}
