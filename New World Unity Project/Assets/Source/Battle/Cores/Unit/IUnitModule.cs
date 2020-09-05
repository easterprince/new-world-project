namespace NewWorld.Battle.Cores.Unit {
    
    public interface IUnitModule<TSelf, TPresentation, TParentPresentation> :
        IConnectableCore<TSelf, TPresentation, TParentPresentation>, IOwnerPointer {}

}
