namespace NewWorld.Utilities {

    public partial class ClassState<TDelta> {
        
        // Wrapper.

        public class StateWrapper : ClassWrapper<ClassState<TDelta>> {

            // Constructor.

            public StateWrapper(ClassState<TDelta> state) : base(state) {}


            // Properties.

            public bool IsLatest => Wrapped.IsLatest;


            // Methods.

            public StateWrapper Transit(out TDelta delta) => Wrapped.Transit(out delta)?.Wrapper; 


        }


    }

}
