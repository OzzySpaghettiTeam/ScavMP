using HarmonyLib;

namespace ScavMP;

[HarmonyPatch(typeof(Body), "FixedUpdate")]
public static class BodyFixedUpdatePatch
{
    static bool Prefix(Body __instance)
    {
        if (BodyRegistry.IsNetworked(__instance))
            return false;
        return true;
    }
}

[HarmonyPatch(typeof(PlayerCamera), "HandleInput")]
public static class PlayerCameraHandleInputPatch
{
    static void Postfix(PlayerCamera __instance)
    {
        // если тело под сетевым контролем — перезаписываем moveDir
        // который HandleInput только что выставил
        if (BodyRegistry.TryGetPawn(__instance.body, out var pawn))
        {
            __instance.body.moveDir = pawn.NetworkedMoveDir;
        }
    }
}
