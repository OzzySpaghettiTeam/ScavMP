using System;
using System.Collections;
using HarmonyLib;
using ScavMP.Shared;

namespace ScavMP;

[HarmonyPatch(typeof(WorldGeneration))]
public class Patch_WorldGeneration
{
    [HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.WorldPreprocess))]
    static bool WorldPreprocess_Prefix(WorldGeneration __instance)
    {
        UnityEngine.Random.InitState(InternalRand.Instance.WorldSeed + 1);
        return true;
    }

    [HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.WorldCreateBackground))]
    static bool WorldCreateBackground_Prefix(WorldGeneration __instance)
    {
        UnityEngine.Random.InitState(InternalRand.Instance.WorldRng.Next());
        return true;
    }

    [HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.WorldGenerateTerrain))]
    static bool WorldGenerateTerrain_Prefix(WorldGeneration __instance)
    {
        UnityEngine.Random.InitState(InternalRand.Instance.WorldRng.Next());
        return true;
    }

    [HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.WorldGenerateWorldBorders))]
    static bool WorldGenerateWorldBorders_Prefix(WorldGeneration __instance)
    {
        UnityEngine.Random.InitState(InternalRand.Instance.WorldRng.Next());
        return true;
    }

    [HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.WorldPlacePlayer))]
    static bool WorldPlacePlayer_Prefix(WorldGeneration __instance)
    {
        UnityEngine.Random.InitState(InternalRand.Instance.WorldRng.Next());
        return true;
    }

    [HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.WorldPlaceEntities))]
    static bool WorldPlaceEntities_Prefix(WorldGeneration __instance)
    {
        UnityEngine.Random.InitState(InternalRand.Instance.WorldRng.Next());
        return true;
    }

    [HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.FinishWorldGeneration))]
    static bool FinishWorldGeneration_Prefix(WorldGeneration __instance)
    {
        UnityEngine.Random.InitState(InternalRand.Instance.WorldRng.Next());
        return true;
    }
}
