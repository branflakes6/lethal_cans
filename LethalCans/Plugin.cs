using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using System;
using System.IO;
using System.Reflection;
using static BepInEx.BepInDependency;
using LethalCans.Patches;


namespace LethalCans
{
    public static class PluginInfo
    {
        public const string PLUGIN_ID = "LethalCans";
        public const string PLUGIN_NAME = "Lethal Cans";
        public const string PLUGIN_AUTHOR = "Branflakes & Eel Salesman";
        public const string PLUGIN_VERSION = "1.0.0";
        public const string PLUGIN_GUID = "com.eelflakes.lethalcans";
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public PluginLogger PluginLogger;

        private void LateUpdate()
        {

            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
            NetworkRPBPatch.FindCoronerAssembly();
            PluginLogger.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} ({PluginInfo.PLUGIN_GUID}) is loaded!");
            enabled = false;

        }

        private void Awake()
        {
            Instance = this;

            if (Logger == null)
            {
                throw new NullReferenceException("Logger is not initialized.");
            }

            PluginLogger = new PluginLogger(Logger);
        }
    }



    public class PluginLogger
    {
        private readonly ManualLogSource manualLogSource;

        public PluginLogger(ManualLogSource manualLogSource)
        {
            this.manualLogSource = manualLogSource;
        }

        public void LogFatal(object data) => manualLogSource.LogFatal(data);
        public void LogError(object data) => manualLogSource.LogError(data);
        public void LogWarning(object data) => manualLogSource.LogWarning(data);
        public void LogMessage(object data) => manualLogSource.LogMessage(data);
        public void LogInfo(object data) => manualLogSource.LogInfo(data);
        public void LogDebug(object data) => manualLogSource.LogDebug(data);
    }
}

