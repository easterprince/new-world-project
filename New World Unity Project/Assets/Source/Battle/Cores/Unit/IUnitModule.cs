namespace NewWorld.Battle.Cores.Unit {
    
    public interface IUnitModule<TSelf, TPresentation, TParentPresentation> :
        IConnectableCore<TSelf, TPresentation, TParentPresentation>, IOwnerPointer
        where TPresentation : class, IOwnerPointer
        where TParentPresentation : class, IOwnerPointer {}

}
