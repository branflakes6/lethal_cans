using System;
using GameNetcodeStuff;
using TMPro;

#nullable enable

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
                if (!playerController.disconnectedMidGame && !playerController.isPlayerDead && !playerController.isPlayerControlled)
                {
                    continue;
                }
                else {
                    TextMeshProUGUI textMesh = __instance.statsUIElements.playerNotesText[playerIndex];
                    var drinks = DrinksTracker.drinkAmountsToString()
                    textMesh.text += "Drinks: " + drinks + "\n"
                }
            }
        }
    } 
}
