using NewWorld.Utilities.Events;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NewWorld.Battle.Controllers.UI {

    public class PointerInterceptorController : MonoBehaviour, IPointerClickHandler {

        // Event types.

        public class PointerEvent : ControllerEvent<PointerEventData> {}


        // Fields.

        private readonly PointerEvent clickEvent = new PointerEvent();


        // Properties.

        public PointerEvent.EventWrapper ClickEvent => clickEvent.Wrapper;


        // Life cycle.

        private void OnDestroy() {
            clickEvent.Clear();
        }


        // Events.

        public void OnPointerClick(PointerEventData pointerEventData) {
            clickEvent.Invoke(pointerEventData);
        }


    }

}
