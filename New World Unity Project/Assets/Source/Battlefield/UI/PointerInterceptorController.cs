using UnityEngine;
using UnityEngine.EventSystems;
using NewWorld.Utilities.Singletons;
using NewWorld.Utilities.Events;
using UnityEngine.Events;

namespace NewWorld.Battlefield.UI {

    public class PointerInterceptorController : SceneSingleton<PointerInterceptorController>, IPointerClickHandler {

        // Event types.

        public class PointerEvent : UnityEvent<PointerEventData> {}


        // Fields.

        private PointerEvent clickEvent;


        // Properties.

        public PointerEvent ClickEvent => clickEvent;


        // Life cycle.

        override private protected void Awake() {
            base.Awake();
            clickEvent = new PointerEvent();
        }


        // Events.

        public void OnPointerClick(PointerEventData pointerEventData) {
            clickEvent.Invoke(pointerEventData);
        }


    }

}
