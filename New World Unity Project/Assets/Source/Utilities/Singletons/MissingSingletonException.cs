using UnityEngine;

namespace NewWorld.Utilities.Singletones {
    
    public class MissingSingletonException<Singleton> : System.Exception
        where Singleton : SceneSingleton<Singleton> {

        // Constructor.

        public MissingSingletonException(object source) :
            base($"Object {source} of type {source.GetType()} needs instance of {typeof(Singleton)}.") {}


    }

}
