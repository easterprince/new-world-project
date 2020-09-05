namespace NewWorld.Utilities {

    public partial class ClassState<TDelta> {

        // Fields.

        private ClassState<TDelta> next = null;
        private TDelta delta = default;
        private readonly StateWrapper wrapper;


        // Constructor.

        public ClassState() {
            wrapper = new StateWrapper(this);
        }


        // Properties.

        public StateWrapper Wrapper => wrapper;
        public bool IsLatest => (next is null);


        // Methods.

        public ClassState<TDelta> Transit(out TDelta delta) {
            delta = this.delta;
            return next;
        }

        public ClassState<TDelta> BuildTransition(TDelta delta) {
            this.delta = delta;
            next = new ClassState<TDelta>();
            return next;
        }


    }

}
