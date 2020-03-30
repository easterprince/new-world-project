using NewWorld.Battlefield.Unit;
using UnityEngine;

namespace NewWorld.Battlefield.UnitSystem {

    public class UnitAddition : UnitSystemUpdate {

        // Fields.

        private readonly UnitTemplate description;


        // Properties.

        public UnitTemplate Description => description;


        // Constructor.

        public UnitAddition(UnitTemplate description) : base() {
            this.description = description ?? throw new System.ArgumentNullException(nameof(description));
        }


    }

}
