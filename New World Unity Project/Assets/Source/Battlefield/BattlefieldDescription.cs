using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units;

namespace NewWorld.Battlefield {

    public class BattlefieldDescription {

        // Fields.

        private readonly MapDescription mapDescription;
        private readonly List<UnitDescription> unitDescriptions;


        // Properties.

        public MapDescription MapDescription => mapDescription;
        public List<UnitDescription> UnitDescriptions => unitDescriptions;


        // Constructor.

        public BattlefieldDescription(MapDescription mapDescription, List<UnitDescription> unitDescriptions) {
            this.mapDescription = mapDescription;
            this.unitDescriptions = unitDescriptions;
        }


    }

}
