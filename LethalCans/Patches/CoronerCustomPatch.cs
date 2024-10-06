using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using Coroner;
using BepInEx;
using StaticNetcodeLib;
using System;


namespace LethalCans.Patches
{
    public static class NetworkRPBPatch
    {
        public static void FindCoronerAssembly()
        {
            // Get all currently loaded assemblies
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    // Try to get the NetworkRPC type from the current assembly
                    Type networkRPCType = assembly.GetType("Coroner.NetworkRPC");
                    if (networkRPCType != null)
                    {
                        Plugin.Instance.PluginLogger.LogDebug($"Found NetworkRPC type in assembly: {assembly.FullName}");

                        // If found, proceed with patching
                        ApplyPatch(assembly, networkRPCType);
                        return;
                    }
                }
                catch
                {
                    // In case of any errors, just continue with the next assembly
                    continue;
                }
            }

            Plugin.Instance.PluginLogger.LogError("Could not find NetworkRPC type in any loaded assembly.");
        }

        public static void ApplyPatch(Assembly targetAssembly, Type networkRPCType)
        {

            var harmony = new Harmony("com.eelflakes.lethalcans");

            // Get the method information for BroadcastCauseOfDeathClientRpc using reflection
            MethodInfo originalMethod = AccessTools.Method(networkRPCType, "BroadcastCauseOfDeathClientRpc");

            if (originalMethod == null)
            {
                Plugin.Instance.PluginLogger.LogError("BroadcastCauseOfDeathClientRpc method not found.");
                return;
            }

            // Create a MethodInfo for the postfix patch
            MethodInfo postfixMethod = typeof(NetworkRPBPatch).GetMethod(nameof(BroadcastCauseOfDeathClientRpcPostfix), BindingFlags.Static | BindingFlags.NonPublic);
            if (postfixMethod == null)
            {
                Plugin.Instance.PluginLogger.LogError("Postfix method not found.");
                return;
            }

            Plugin.Instance.PluginLogger.LogError("BroadcastCauseOfDeathClientRpc method found!.");
            // Patch the method
            harmony.Patch(originalMethod, postfix: new HarmonyMethod(postfixMethod));
        }

        // The postfix method that will be called after the original method
        private static void BroadcastCauseOfDeathClientRpcPostfix(int playerClientId, string codLanguageTag, bool forceOverride)
        {
            Plugin.Instance.PluginLogger.LogDebug("Player died and must drink.");
            AdvancedCauseOfDeath? causeOfDeath = AdvancedCauseOfDeath.Fetch(codLanguageTag);
            if (causeOfDeath != null)
            {
                int drinkAmount = DrinksTracker.getDrinkAmount(causeOfDeath);
                DrinksTracker.setDrinkAmountDeath(playerClientId, drinkAmount);
                Plugin.Instance.PluginLogger.LogDebug("Player died and must drink.");
                Plugin.Instance.PluginLogger.LogDebug(drinkAmount);
            }
        }
    }
}
