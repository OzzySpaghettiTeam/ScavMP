using System;

namespace ScavMP.Shared;

public static class ScavMPTypes
{
    static bool _registered = false;

    public static void Register()
    {
        if (_registered)
            throw new Exception("ScavMP Types are already registered");
        RuntimeEntityTypesMap.Instance.Register(100, e => new BaseExpie(e));
        RuntimeEntityTypesMap.Instance.Register(101, e => new BaseExpieController(e));
        RuntimeEntityTypesMap.Instance.Register(102, e => new UnityPhysicsManager(e));
        RuntimeEntityTypesMap.Instance.Register(
            102,
            e => new BaseWorld(e).Init(WorldGeneration.world)
        );
        _registered = true;
    }
}
