using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

#nullable enable

namespace LethalCans.Patches


    // We are using Coroner as a dependancy and want to patch their CauseOfDeath broadcast for calculating our drink totals
    [HarmonyPatch(typeof(Coroner.NetworkRPC))]
    [HarmonyPatch("BroadcastCauseOfDeathClientRpc")]
    class BroadcastCauseOfDeathClientRpcPatch {
        // Postfix patch so it runs after Coroner has determined the cause of death
        public static void Postfix(int playerClientId, string? codLanguageTag, bool forceOverride)
        {   
            AdvancedCauseOfDeath? causeOfDeath = AdvancedCauseOfDeath.Fetch(codLanguageTag);
            // getDrinkAmount(player)  
            // setDrinkAmount(player, drinks)
            if(causeOfDeath != null)
            {
                int drinkAmount = DrinksTracker.drinksTracker[playerClientId];
                DrinksTracker.setDrinkAmount(playerClientId, drinkAmount);
                Debug.Log($"Player {playerClientId} died due to {codLanguageTag}, adding {drinkAmount} drinks.");                
            }

        }
    }