namespace ScavMP.Shared;

public enum PacketType : byte
{
    EntitySystem,
    Serialized,
}

public class JoinPacket
{
    public string UserName { get; set; }
    public ulong GameHash { get; set; }
}
