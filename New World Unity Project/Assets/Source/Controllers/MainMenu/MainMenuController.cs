using NewWorld.Controllers.MetaData;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Controllers.MainMenu {
    
    public class MainMenuController : MonoBehaviour {

        // Static.

        private static bool initializedDescriptors = false;


        // Life cycle.

        private void Start() {

            if (!initializedDescriptors) {
                Descriptors.AddDescriptors(new ConditionDescriptor[] {
                    new ConditionDescriptor(
                        NamedId.Get("SimpleMotion"),
                        "Simple motion",
                        "Moving to #MOTION_DESTINATION#",
                        "DefaultMotion"
                    ),
                    new ConditionDescriptor(
                        NamedId.Get("SimpleAttack"),
                        "Simple attack",
                        "Attacking #ATTACK_TARGET#",
                        "DefaultAttack"
                    ),
                    new ConditionDescriptor(
                        NamedId.Get("SimpleCollapse"),
                        "Simple collapse",
                        "Collapsing, until extinction #EXTINCTION_TIME# s",
                        "DefaultCollapse"
                    ),
                    new ConditionDescriptor(
                        NamedId.Get("SimpleIdle"),
                        "Simple idling",
                        "Idle",
                        null
                    )
                });
                initializedDescriptors = true;
            }

        }


    }

}
