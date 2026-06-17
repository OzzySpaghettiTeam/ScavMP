using LiteEntitySystem;

namespace ScavMP.Shared;

public class BaseWorld : SingletonEntityLogic
{
    private WorldGeneration _worldGen;
    private SyncVar<int> _seed;

    public BaseWorld(EntityParams entityParams)
        : base(entityParams) { }

    public BaseWorld Init(WorldGeneration worldGen)
    {
        _worldGen = worldGen;
        return this;
    }

    public void GenWorld()
    {
        InternalRand.Instance.WorldSeed = _seed;
        _worldGen.StartCoroutine(_worldGen.GenerateWorld());
    }
}
