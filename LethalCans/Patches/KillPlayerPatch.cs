// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using HarmonyLib;
// using GameNetcodeStuff;
// using UnityEngine;

// namespace LethalCans.Patches
// {
//     [HarmonyPatch(typeof(PlayerControllerB))]
//     [HarmonyPatch("KillPlayer")]
//     public class KillPlayerPatch
//     {
//         // Postfix to run after KillPlayer method is executed
//         static void Postfix(PlayerControllerB __instance, Vector3 bodyVelocity, bool spawnBody, CauseOfDeath causeOfDeath, int deathAnimation, Vector3 positionOffset)
//         {
//             if (spawnBody)
//             {
//                 // Set position of dead body
//                 Vector3 deathPosition = __instance.transform.position;
//                 int deadPlayerClientId = __instance.playerClientId;
//                 // Check for observers of body
//                 int spectators = DrinkTracker.calculateSpectators(deathPosition, deadPlayerClientId);
//             }
//         }
//     }
// }
