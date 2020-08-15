using System;

namespace NewWorld.Battle.Cores.Unit.Behaviours.Relocations {

    public class RelocationBehaviour : BehaviourModuleBase<RelocationBehaviour, RelocationPresentation, RelocationGoal> {

        // Cloning.

        private protected override RelocationBehaviour ClonePartially() {
            return new RelocationBehaviour();
        }


        // Presentation generation.

        private protected override RelocationPresentation BuildPresentation() {
            return new RelocationPresentation(this);
        }


        // Acting.

        private protected override void OnAct(out GoalStatus goalStatus) {
            throw new NotImplementedException();
        }


    }

}
