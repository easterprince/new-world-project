using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NewWorld.Battlefield.UI.UnitPanel {
    
    public class UnitPortraitController : MonoBehaviour, IPointerClickHandler {

        // Fields.

        private UnityEvent pointerClickEvent;


        // Properties.

        public UnityEvent PointerClickEvent => pointerClickEvent;


        // Life cycle.

        private void Awake() {
            pointerClickEvent = new UnityEvent();
        }


        // Event handlers.

        public void OnPointerClick(PointerEventData eventData) {
            pointerClickEvent.Invoke();
        }


    }

}
