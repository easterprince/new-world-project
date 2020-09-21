using NewWorld.Controllers.MetaData;
using NewWorld.Cores.Battle.Unit.Behaviours.Offensives;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
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

                // Add goals descriptors.
                Descriptors.ForGoals.Add(new GoalDescriptor[] {
                    new GoalDescriptor(
                        NamedId.Get("OffensiveGoal"),
                        (goal) => {
                            string name = (goal as OffensiveGoal)?.Target?.Name ?? null;
                            return $"Destroy {name ?? "unknown target"}";
                        }
                    ),
                    new GoalDescriptor(
                        NamedId.Get("RelocationGoal"),
                        (goal) => {
                            string destination = null;
                            if (goal is RelocationGoal relocationGoal) {
                                destination = relocationGoal.Destination.ToString();
                            }
                            return $"Relocate to {destination ?? "unknown position"}";
                        }
                    ),
                    new GoalDescriptor(
                        NamedId.Get("IdleGoal"),
                        (goal) => "Do nothing"
                    )
                });

            }

        }


    }

}
