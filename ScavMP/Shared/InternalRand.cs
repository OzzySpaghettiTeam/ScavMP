using System;

namespace ScavMP.Shared;

public class InternalRand
{
    private Random _worldRng = new(1337);
    private int _worldSeed = 1337;

    // hope there will be chance to use this.
    public Random WorldRng => _worldRng;
    public int WorldSeed
    {
        get { return _worldSeed; }
        set
        {
            _worldSeed = value;
            _worldRng = new(value);
        }
    }

    public static InternalRand Instance;
}
