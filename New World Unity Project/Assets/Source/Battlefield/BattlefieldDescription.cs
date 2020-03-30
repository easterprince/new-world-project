using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Unit;

namespace NewWorld.Battlefield {

    public class BattlefieldDescription {

        // Fields.

        private readonly MapDescription mapDescription;
        private readonly List<UnitTemplate> unitDescriptions;


        // Properties.

        public MapDescription MapDescription => mapDescription;
        public List<UnitTemplate> UnitDescriptions => unitDescriptions;


        // Constructor.

        public BattlefieldDescription(MapDescription mapDescription, List<UnitTemplate> unitDescriptions) {
            this.mapDescription = mapDescription;
            this.unitDescriptions = unitDescriptions;
        }


    }

}
