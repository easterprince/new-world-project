namespace NewWorld.Utilities {

    public partial class ObjectState<TDelta> {

        // Wrapper.

        public class StateWrapper : ObjectWrapper<ObjectState<TDelta>> {

            // Constructor.

            public StateWrapper(ObjectState<TDelta> state) : base(state) {}


            // Properties.

            public bool IsLatest => Wrapped.IsLatest;


            // Methods.

            public StateWrapper Transit(out TDelta delta) => Wrapped.Transit(out delta)?.Wrapper;


        }


    }

}
