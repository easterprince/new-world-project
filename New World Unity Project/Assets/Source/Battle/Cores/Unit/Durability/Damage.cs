using NewWorld.Utilities;
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
            set => damageValue = Floats.SetPositive(value);
        }

        public bool IsZero => damageValue == 0;


        // Static properties.

        public static Damage Zero => new Damage();


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
