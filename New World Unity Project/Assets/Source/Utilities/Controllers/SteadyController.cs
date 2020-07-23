using System;
using UnityEngine;

namespace NewWorld.Utilities.Controllers {
    
    public class SteadyController : MonoBehaviour {

        // Fields.

        private bool calledStart = false;
        private bool finishedStart = false;


        // Properties.

        public bool Fixed => calledStart;
        public bool Started => finishedStart;


        // Life cycle.

        public void Start() {
            if (calledStart) {
                return;
            }
            calledStart = true;
            OnStart();
            finishedStart = true;
        }


        // Methods.

        private protected virtual void OnStart() {}

        private protected void ForceBeingStarted() {
            calledStart = true;
            finishedStart = true;
        }

        private protected void ValidateBeingNotFixed() {
            if (calledStart) {
                throw new InvalidOperationException("Operation is forbidden after script started (after Start() was called).");
            }
        }

        private protected void ValidateBeingStarted() {
            if (!finishedStart) {
                throw new InvalidOperationException("Operation is forbidden before script finished starting (before Start() finished).");
            }
        }


    }

}
