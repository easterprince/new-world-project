﻿namespace NewWorld.Utilities.Singletons {

    public class MissingSingletonException<TSingleton> : System.Exception
        where TSingleton : ControllerSingletonBase<TSingleton> {

        // Constructor.

        public MissingSingletonException(object source) :
            base($"Object {source} of type {source.GetType()} needs instance of {typeof(TSingleton)}.") {}


    }

}
