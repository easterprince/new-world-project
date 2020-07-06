using NewWorld.Battlefield.Unit.Controller;

namespace NewWorld.Battlefield.Unit {
    
    public interface IParentModule<TSelf>
        where TSelf : IParentModule<TSelf> {

        // Properties.

        UnitController Owner { get; }


        // Methods.

        void Attach(IChildModule<TSelf> module);

        void Detach(IChildModule<TSelf> module);

        bool HasChild(IChildModule<TSelf> module);


    }

}
