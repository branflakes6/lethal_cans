using System;
using GameNetcodeStuff;
using TMPro;
using HarmonyLib;
using UnityEngine;
using Coroner;

namespace LethalCans.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    [HarmonyPatch("FillEndGameStats")]
    [HarmonyPriority(Priority.Low)]
    class HUDManagerAddDrinkAmountsPatch
    {
        public static void Postfix(HUDManager __instance)
        {
            // Loop through each player, get their drink amounts and add it to their death string
            for (int playerIndex = 0; playerIndex < __instance.statsUIElements.playerNotesText.Length; playerIndex++)
            {
                PlayerControllerB playerController = __instance.playersManager.allPlayerScripts[playerIndex];
                Coroner.AdvancedCauseOfDeath? cod = Coroner.API.GetCauseOfDeath(playerController);
                Plugin.Instance.PluginLogger.LogDebug(playerController.playerUsername);
                Plugin.Instance.PluginLogger.LogDebug(playerIndex);
                Plugin.Instance.PluginLogger.LogDebug(cod);
                Plugin.Instance.PluginLogger.LogDebug(((Coroner.AdvancedCauseOfDeath)cod).GetLanguageTag());

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
                    int death_drinks;
                    if (((Coroner.AdvancedCauseOfDeath)cod).GetLanguageTag() == "DeathGravity")
                    {
                        death_drinks = 15;
                    }
                    else if (((Coroner.AdvancedCauseOfDeath)cod).GetLanguageTag() == "DeathUnknown")
                    {
                        death_drinks = 5;
                    }
                    else
                    {
                        death_drinks = DrinksTracker.getDrinks((int)playerController.playerClientId);
                    }
                    total_drinks += death_drinks;
                    textMesh.text = "Drinks: " + total_drinks.ToString() + "\n" + textMesh.text;

                }
            }
            DrinksTracker.clearDrinkAmounts();
            HUDManager.Instance.endgameStatsAnimator.speed = 0.5f;
        }
    }
}
