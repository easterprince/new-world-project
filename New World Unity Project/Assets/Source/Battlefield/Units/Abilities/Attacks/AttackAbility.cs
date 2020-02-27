﻿using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities.Attacks {

    public abstract class AttackAbility : Ability<AttackAbilityPresentation> {

        // Static.

        public static object FormParameterSet(UnitController target) {
            return target;
        }


        // Fields.

        private readonly float attackPower;
        private readonly float attackSpeed;
        private readonly float attackTime;


        // Properties.

        public float AttackPower => attackPower;
        public float AttackSpeed => attackSpeed;
        public float AttackTime => attackTime;


        // Constructor.

        public AttackAbility(float attackPower = 1, float attackSpeed = 1, float attackTime = 0.5f) : base() {
            this.attackPower = Mathf.Max(attackPower, 0);
            this.attackSpeed = Mathf.Max(attackSpeed, 0);
            this.attackTime = Mathf.Clamp(attackTime, 0f, 1f);
            Presentation = new AttackAbilityPresentation(this);
        }


    }

}
