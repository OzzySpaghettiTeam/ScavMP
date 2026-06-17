using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using ScavMP.Shared;
using UnityEngine;

namespace ScavMP
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "05126619z.scavmp";
        public const string ModName = "Scav MP";
        public const string ModVersion = "0.0.0";

        internal static new Logger Logger;
        private readonly Harmony _harmony = new(ModGUID);
        public static Plugin Instance { get; private set; } = null!;

        void Awake()
        {
            Logger = new(new ManualLogSource(ModName));
            Instance = this;
            _harmony.PatchAll();
            RuntimeEntityTypesMap.Instance = new();
            ScavMPTypes.Register();
            Logger.Log($"Plugin {ModName} is loaded!");
        }

        void OnDestroy()
        {
            _harmony.UnpatchSelf();
            Instance = null;
        }
    }
}
