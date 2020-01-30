﻿using UnityEngine;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class ForceCondition : GeneralUnitUpdate {

        // Fields.

        private readonly Condition condition;


        // Properties.

        public Condition Condition => condition;


        // Constructor.

        public ForceCondition(Condition condition) : base(condition.Owner) {
            this.condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
        }


    }


}
