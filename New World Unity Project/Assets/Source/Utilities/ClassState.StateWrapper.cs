namespace NewWorld.Utilities {

    public partial class ClassState<TDelta> {
        
        // Wrapper.

        public class StateWrapper : ClassWrapper<ClassState<TDelta>> {

            // Constructor.

            public StateWrapper(ClassState<TDelta> state) : base(state) {}


            // Methods.

            public StateWrapper Transit(out TDelta delta) => Wrapped.Transit(out delta)?.Wrapper; 


        }


    }

}
