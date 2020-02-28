﻿using UnityEngine;

namespace NewWorld.Battlefield.Units.Conditions.Attacks {
    
    public class AttackConditionPresentation : ConditionPresentation<AttackCondition, AttackConditionPresentation> {
    
        // Constructor.

        public AttackConditionPresentation(AttackCondition presented) : base(presented) {}


        // Properties.

        override public string Description => $"Attacking {Presented.Target.name}";


    }

}