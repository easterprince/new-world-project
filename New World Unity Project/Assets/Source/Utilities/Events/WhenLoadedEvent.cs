using UnityEngine;
using NewWorld.Utilities.Singletons;

namespace NewWorld.Utilities.Events {

    public class WhenLoadedEvent : ConditionalEvent {

        // Constructor.

        public WhenLoadedEvent(ILoadable loadable) : base(() => loadable.Loaded) { }


    }

    public class WhenLoadedEvent<T0> : ConditionalEvent<T0> {

        // Constructor.

        public WhenLoadedEvent(ILoadable loadable) : base(() => loadable.Loaded) { }


    }

    public class WhenLoadedEvent<T0, T1> : ConditionalEvent<T0, T1> {

        // Constructor.

        public WhenLoadedEvent(ILoadable loadable) : base(() => loadable.Loaded) { }


    }


}
