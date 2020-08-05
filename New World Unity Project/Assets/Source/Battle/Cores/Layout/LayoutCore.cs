using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Map;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battle.Cores.Layout {

    public class LayoutCore : ConnectableCoreBase<LayoutCore, LayoutPresentation, BattlefieldPresentation> {

        // Fields.

        private readonly List<LayoutRegion> regions = new List<LayoutRegion>();


        // Constructors.

        public LayoutCore() {}

        public LayoutCore(LayoutCore other) {
            var oldToNew = new Dictionary<LayoutRegion, LayoutRegion>();
            foreach (var oldRegion in other.regions) {
                var newRegion = new LayoutRegion(oldRegion.Center);
                regions.Add(newRegion);
                oldToNew[oldRegion] = newRegion;
            }
            foreach (var oldRegion in regions) {
                var newRegion = oldToNew[oldRegion];
                foreach (var pair in oldRegion.Adjacency) {
                    var oldAdjacent = pair.Key;
                    var newAdjacent = oldToNew[oldAdjacent];
                    newRegion.AddAdjacent(newAdjacent);
                }
            }
        }


        // Cloning.

        public override LayoutCore Clone() {
            return new LayoutCore(this);
        }


        // Presentation generation.

        private protected override LayoutPresentation BuildPresentation() {
            return new LayoutPresentation(this);
        }


        // Methods.

        public void SetupRegions() {
            ValidateContext();
            throw new NotImplementedException();
        }


    }

}
