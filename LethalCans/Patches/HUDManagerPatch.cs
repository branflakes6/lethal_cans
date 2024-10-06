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
                {//for alive players
                    /* TextMeshProUGUI textMesh = __instance.statsUIElements.playerNotesText[playerIndex];
                    string aliveDrinks = DrinkTracker.calculateTotalDeaths();
                    textMesh.text += "Drinks: " + aliveDrinks + "\n"; */
                    continue;

                }
                else { //For Dead Players
                    TextMeshProUGUI textMesh = __instance.statsUIElements.playerNotesText[playerIndex];
                    string drinks = DrinksTracker.drinkAmountsToString((int) playerController.playerClientId);
                    textMesh.text += "Drinks: " + drinks + "\n";
                }
            }
            DrinksTracker.clearDrinkAmounts();
        }
    }
}
