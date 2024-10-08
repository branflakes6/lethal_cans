using System;
using GameNetcodeStuff;
using TMPro;
using HarmonyLib;
using UnityEngine;

namespace LethalCans.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    [HarmonyPatch("FillEndGameStats")]
    [HarmonyPriority(Priority.Low)]
    class HUDManagerAddDrinkAmountsPatch
    {
        public static void Postfix(HUDManager __instance)
        {
            Plugin.Instance.PluginLogger.LogDebug(DrinksTracker.drinksTracker);

            // Loop through each player, get their drink amounts and add it to their death string
            for (int playerIndex = 0; playerIndex < __instance.statsUIElements.playerNotesText.Length; playerIndex++)
            {
                
                PlayerControllerB playerController = __instance.playersManager.allPlayerScripts[playerIndex];
                Plugin.Instance.PluginLogger.LogDebug(playerController.playerUsername);
                Plugin.Instance.PluginLogger.LogDebug(playerIndex);

                if (!playerController.disconnectedMidGame && !playerController.isPlayerDead && !playerController.isPlayerControlled)
                {
                    continue;
                }
                else {
                    TextMeshProUGUI textMesh = __instance.statsUIElements.playerNotesText[playerIndex];
                    int total_drinks = 0;
                    if (StartOfRound.Instance.allPlayersDead)
                    {
                        total_drinks += 5;
                    }
                    int death_drinks = DrinksTracker.getDrinks((int)playerController.playerClientId);
                    total_drinks += death_drinks;
                    textMesh.text += "Drinks: " + total_drinks.ToString() + "\n";
                }
            }
            DrinksTracker.clearDrinkAmounts();
        }
    }
}
