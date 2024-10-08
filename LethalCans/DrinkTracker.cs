using System;
using System.Collections.Generic;
using GameNetcodeStuff;
using UnityEngine;
using UnityEngineInternal;

namespace LethalCans
{
    class DrinksTracker
    {

        // This dict stores playerClientID's and the number of drinks associated with each ID
        public static Dictionary<int, int> drinksTracker = new Dictionary<int, int>();

        // Calcualte the number of drinks based on how a player died
        public static int getDrinkAmount(Coroner.AdvancedCauseOfDeath? causeOfDeath, int playerId)
        {
            Plugin.Instance.PluginLogger.LogDebug("Calculating Drinks");
            PlayerControllerB player = StartOfRound.Instance.allPlayerScripts[playerId];
            Vector3 deathPosition = player.transform.position;

            int spectators = calculateSpectators(deathPosition, playerId);
            Plugin.Instance.PluginLogger.LogDebug($"Specators: {spectators}");
            // This gets the death tag from coroner
            var deathTag = ((Coroner.AdvancedCauseOfDeath)causeOfDeath).GetLanguageTag();
            int medium = 1;

            int difficulty = medium;

            // For V0.1 we can just use a switch statement, later we will add a proper config file that can be edited but that requires some work to read and write to
            switch (deathTag)
            {
                // Generic deaths
                #region GenericDeaths
                case "DeathBludgeoning":
                case "DeathGravity":
                case "DeathBlast":
                case "DeathStrangulation":
                case "DeathSuffocation":
                case "DeathMauling":
                case "DeathGunshots":
                case "DeathCrushing":
                case "DeathDrowning":
                case "DeathAbandoned":
                case "DeathElectrocution":
                case "DeathKicking":       // New in v45
                case "DeathBurning":       // New in v50
                case "DeathStabbing":      // New in v50
                case "DeathFan":           // New in v50
                case "DeathInertia":       // New in v55
                case "DeathSnipped":       // New in v55
                    return (3 * difficulty) + spectators;
                #endregion

                // Custom causes (enemies)
                #region CustomCausesEnemies
                case "DeathEnemyBaboonHawk":
                    return (10 * difficulty) + spectators;
                case "DeathEnemyBracken":
                case "DeathEnemyBunkerSpider":
                case "DeathEnemyCircuitBees":
                case "DeathEnemyCoilHead":
                case "DeathEnemyEarthLeviathan":
                case "DeathEnemySnareFlea":
                    return (2 * difficulty) + spectators;
                case "DeathEnemyEyelessDog":
                case "DeathEnemyGhostGirl":
                case "DeathEnemyJester":
                case "DeathEnemyThumper":
                case "DeathEnemyForestGiantEaten":
                    return (6 * difficulty) + spectators;
                case "DeathEnemyHoarderBug":
                case "DeathEnemyHygrodere":
                case "DeathEnemyLassoMan":
                case "DeathEnemySporeLizard":
                case "DeathEnemyForestGiantDeath":
                    return (10 * difficulty) + spectators;
                #endregion

                // Enemies from v45
                #region EnemiesV45
                case "DeathEnemyMaskedPlayerWear":
                    return (10 * difficulty) + spectators;
                case "DeathEnemyMaskedPlayerVictim":
                case "DeathEnemyNutcrackerKicked":
                case "DeathEnemyNutcrackerShot":
                    return (6 * difficulty) + spectators;
                #endregion

                // Enemies from v50
                #region EnemiesV50
                case "DeathEnemyButlerStab":
                case "DeathEnemyButlerExplode":
                case "DeathEnemyMaskHornets":
                case "DeathEnemyTulipSnakeDrop":
                case "DeathEnemyOldBirdRocket":
                case "DeathEnemyOldBirdStomp":
                case "DeathEnemyOldBirdCharge":
                case "DeathEnemyOldBirdTorch":
                    return (4 * difficulty) + spectators;
                #endregion

                // Enemies from v55
                #region EnemiesV55
                case "DeathEnemyKidnapperFox":
                case "DeathEnemyBarber":
                    return (4 * difficulty) + spectators;
                #endregion

                // Enemies from v60
                #region EnemiesV60
                case "DeathEnemyManeater":
                    return (4 * difficulty) + spectators;
                #endregion

                // Custom causes (player)
                #region CustomCausesPlayer
                case "DeathPlayerJetpackGravity":
                case "DeathPlayerJetpackBlast":
                case "DeathPlayerLadder":
                case "DeathPlayerMurderShovel":
                case "DeathPlayerMurderStopSign":
                case "DeathPlayerMurderYieldSign":
                case "DeathPlayerMurderKnife":
                case "DeathPlayerEasterEgg":
                case "DeathPlayerMurderShotgun":
                case "DeathPlayerQuicksand":
                case "DeathPlayerStunGrenade":
                    return (4 * difficulty) + spectators;
                #endregion

                // Custom causes (player vehicles)
                #region CustomCausesPlayerVehicles
                case "DeathPlayerCruiserDriver":
                case "DeathPlayerCruiserPassenger":
                case "DeathPlayerCruiserExplodeBystander":
                case "DeathPlayerCruiserRanOver":
                    return (4 * difficulty) + spectators;
                #endregion 

                // Custom causes (pits)
                #region CustomCausesPits
                case "DeathPitGeneric":
                case "DeathPitFacilityPit":
                case "DeathPitFacilityCatwalkJump":
                case "DeathPitMinePit":
                case "DeathPitMineCave":
                case "DeathPitMineElevator":
                    return (4 * difficulty) + spectators;
                #endregion

                // Custom causes (other)
                #region CustomCausesOther
                case "DeathOtherDepositItemsDesk":
                case "DeathOtherItemDropship":
                case "DeathOtherLandmine":
                case "DeathOtherTurret":
                case "DeathOtherLightning":
                case "DeathOtherMeteor":
                case "DeathOtherSpikeTrap":
                case "DeathOtherOutOfBounds":
                    Debug.Log("Player died due to dropship.");
                    return (10 * difficulty) + spectators;
                #endregion

                // Unknown death cause
                case "DeathUnknown":
                    return (4 * difficulty) + spectators;

                // Default case if no match
                default:
                    return 2 + spectators;
            }
        }


        public static void clearDrinkAmounts()
        {
            foreach (PlayerControllerB player in StartOfRound.Instance.allPlayerScripts)
            {
                int playerId = (int)player.playerClientId;
                if (drinksTracker.ContainsKey(playerId))
                {
                    drinksTracker[playerId] = 0;
                }
                   
            }
            
        }

        // Sets or updates the amount of drinks for a player
        public static void setDrinkAmountDeath(int playerClientId, int drinks)
        {
            Debug.Log("Setting Drinks.");
            if (!drinksTracker.ContainsKey(playerClientId))
            {
                drinksTracker[playerClientId] = drinks;
            }
            else
            {
                if (drinksTracker[playerClientId] == 0)
                {
                    drinksTracker[playerClientId] = drinks;
                }
            }
        }

        // Calculate how many players witnessed a players death
        public static int calculateSpectators(Vector3 deathPosition, int deadPlayerClientId)
        {
            Plugin.Instance.PluginLogger.LogDebug("Calculating Spectators");
            int spectatorsCount = 0;
            // Get all active players

            GameNetcodeStuff.PlayerControllerB[] allPlayers = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>();

            Plugin.Instance.PluginLogger.LogDebug(allPlayers);

            // Loop through each player

            foreach (GameNetcodeStuff.PlayerControllerB player in allPlayers)
            {
                Plugin.Instance.PluginLogger.LogDebug(player.playerClientId);
                Plugin.Instance.PluginLogger.LogDebug(player.playerUsername);
                if (!player.disconnectedMidGame && !player.isPlayerDead && !player.isPlayerControlled)
                {
                    continue;
                }

                // Ignore dead player
                if ((int)player.playerClientId == deadPlayerClientId)
                {
                    continue;
                }

                if (player.hasBegunSpectating)
                {
                    int spectatingPlayerId = (int) player.spectatedPlayerScript.playerClientId;
                    if (spectatingPlayerId == deadPlayerClientId)
                    {
                        spectatorsCount++;
                    }
                }
                // Call method to check for witnesses
                else if (witnessedEvent(player, deathPosition))
                {
                    spectatorsCount++;
                }
            }
            // Return the total number of spectators
            Plugin.Instance.PluginLogger.LogDebug("spectatorsCount");
            Plugin.Instance.PluginLogger.LogDebug(spectatorsCount);
            return spectatorsCount;
        }

        // Determine if a player witnessed a death
        public static bool witnessedEvent(PlayerControllerB player, Vector3 deathPosition)
        {
            Plugin.Instance.PluginLogger.LogDebug("Checking if player witnessed death");
            Plugin.Instance.PluginLogger.LogDebug(player.playerUsername);
            // Get the player's current position and forward direction
            Vector3 playerPosition = player.transform.position;
            Vector3 playerForward = player.transform.forward;
            Plugin.Instance.PluginLogger.LogDebug($"Player forward: {playerForward}");
            // Calculate the direction from the player to the death position
            Vector3 directionToDeath = (deathPosition - playerPosition).normalized;
            Plugin.Instance.PluginLogger.LogDebug($"Player directonToDeath: {directionToDeath}");
            // Is death within FOV
            float angle = Vector3.Angle(playerForward, directionToDeath);
            Plugin.Instance.PluginLogger.LogDebug($"Player forward angle: {angle}");
            if (angle < 45f) //FOV
            {
                Plugin.Instance.PluginLogger.LogDebug($"Death in FOV");
                // Optional: Check for line of sight using raycast (no walls or obstacles blocking view)
                // if (player.HasLineOfSightToPosition(deathPosition))
                // {
                //     // Player witnessed the event
                //     return true;
                // }
                return true;
            }
            // Player did not witness the event
            return false;
        }


        // Takes a playerID and returns the amount of drinks they have as a string
        public static string drinkAmountsToString(int playerClientId)
        {
            if (drinksTracker.ContainsKey(playerClientId))
            {
                int drinks = drinksTracker[playerClientId];
                if (drinks > 0)
                {
                    string drinksString = drinks.ToString();
                    return drinksString;
                }
                return "0";
            }
            return "";
        }

        /* public string calculateTotalDeaths()
        {
            Debug.Log("Calculating total deaths");

            int totalDeaths = 0;

            // Get all active players in the game
            GameNetcodeStuff.PlayerControllerB[] allPlayers = FindObjectsOfType<GameNetcodeStuff.PlayerControllerB>();

            // Loop through each player to check if they are dead
            foreach (GameNetcodeStuff.PlayerControllerB player in allPlayers)
            {
                // Check if the player is dead
                if (player.clientId == deadPlayerClientId) {totalDeaths++;
                }
            }
            // Log the total number of deaths
            Debug.Log("Total Deaths: " + totalDeaths);
            // Return the total number of dead players
            return totalDeaths.toString();
        } */
    }

}
