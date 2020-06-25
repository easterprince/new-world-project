using UnityEngine;

namespace NewWorld.Battlefield.Unit {
    
    public interface IChildModule<TParent>
        where TParent : IParentModule<TParent> {
    
        // Properties.

        bool Connected { get; }


        // Methods.

        void Connect(TParent parent);

        void Disconnect();


    }

}
