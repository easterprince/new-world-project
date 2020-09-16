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

                // Add condition descriptors.
                Descriptors.ForConditions.Add(new ConditionDescriptor[] {
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

                // Add ability descriptors.
                Descriptors.ForAbilities.Add(new AbilityDescriptor[] {
                    new AbilityDescriptor(
                        NamedId.Get("DirectMotion"),
                        "Direct motion",
                        "Directly move to specified position."
                    ),
                    new AbilityDescriptor(
                        NamedId.Get("DirectAttack"),
                        "Direct attack",
                        "Directly attack specified target in range."
                    )
                });
            
            }

        }


    }

}
