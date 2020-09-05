namespace NewWorld.Utilities {

    public partial class ObjectState<TDelta> {

        // Fields.

        private ObjectState<TDelta> next = null;
        private TDelta delta = default;
        private readonly StateWrapper wrapper;


        // Constructor.

        public ObjectState() {
            wrapper = new StateWrapper(this);
        }


        // Properties.

        public StateWrapper Wrapper => wrapper;
        public bool IsLatest => (next is null);


        // Methods.

        public ObjectState<TDelta> Transit(out TDelta delta) {
            delta = this.delta;
            return next;
        }

        public ObjectState<TDelta> BuildTransition(TDelta delta) {
            this.delta = delta;
            next = new ObjectState<TDelta>();
            return next;
        }


    }

}
