﻿using NewWorld.Battle.Cores.Unit;
using UnityEngine;

namespace NewWorld.Battle.Cores.UnitSystem {
    
    public class UnitAdditionAction : UnitSystemAction {

        // Fields.

        private readonly UnitCore unit;
        private readonly Vector2Int position;


        // Properties.

        public UnitCore Unit => unit;
        public Vector2Int Position => position;


        // Constructor.

        public UnitAdditionAction(UnitCore unit, Vector2Int position) {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
            this.position = position;
        }


    }

}