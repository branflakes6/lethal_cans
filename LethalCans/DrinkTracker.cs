using System;
using System.Collections.Generic;
using GameNetcodeStuff;
using UnityEngine;

namespace LethalCans
{
    class DrinksTracker
    {

        // This dict stores playerClientID's and the number of drinks associated with each ID
        public static Dictionary<int, int> drinksTracker = new Dictionary<int, int>();

        // Calcualte the number of drinks based on how a player died
        public static int getDrinkAmount(Coroner.AdvancedCauseOfDeath? causeOfDeath)
        {
            Plugin.Instance.PluginLogger.LogDebug("Calculating Drinks");
            // This gets the death tag from coroner
            var deathTag = ((Coroner.AdvancedCauseOfDeath)causeOfDeath).GetLanguageTag();
            int medium = 1;

            int difficulty = medium;

            // For V0.1 we can just use a switch statement, later we will add a proper config file that can be edited but that requires some work to read and write to
            switch (deathTag)
            {
                // Generic deaths
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
                case "DeathKicking": // New in v45
                case "DeathBurning": // New in v50
                case "DeathStabbing": // New in v50
                case "DeathFan": // New in v50
                case "DeathInertia": // New in v55
                case "DeathSnipped": // New in v55:
                    return 3 * difficulty;

                // Custom causes (enemies)
                case "DeathEnemyBaboonHawk":
                    return 10 * difficulty;
                case "DeathEnemyBracken":
                    return 2 * difficulty;
                case "DeathEnemyBunkerSpider":
                    return 2 * difficulty;
                case "DeathEnemyCircuitBees":
                case "DeathEnemyCoilHead":
                    return 2 * difficulty;
                case "DeathEnemyEarthLeviathan":
                    return 2 * difficulty;
                case "DeathEnemyEyelessDog":
                    return 6 * difficulty;
                case "DeathEnemyGhostGirl":
                    return 6 * difficulty;
                case "DeathEnemyHoarderBug":
                    return 10 * difficulty;
                case "DeathEnemyHygrodere":
                    return 10 * difficulty;
                case "DeathEnemyJester":
                    return 6 * difficulty;
                case "DeathEnemyLassoMan":
                    return 10 * difficulty;
                case "DeathEnemySnareFlea":
                    return 2 * difficulty;
                case "DeathEnemySporeLizard":
                    return 10 * difficulty;
                case "DeathEnemyThumper":
                    return 6 * difficulty;
                case "DeathEnemyForestGiantEaten":
                    return 6 * difficulty;
                case "DeathEnemyForestGiantDeath":
                    return 10 * difficulty;

                // Enemies from v45
                case "DeathEnemyMaskedPlayerWear":
                    return 10 * difficulty;
                case "DeathEnemyMaskedPlayerVictim":
                    return 6 * difficulty;
                case "DeathEnemyNutcrackerKicked":
                    return 6 * difficulty;
                case "DeathEnemyNutcrackerShot":
                    return 6 * difficulty;

                // Enemies from v50
                case "DeathEnemyButlerStab":
                case "DeathEnemyButlerExplode":
                case "DeathEnemyMaskHornets":
                case "DeathEnemyTulipSnakeDrop":
                case "DeathEnemyOldBirdRocket":
                case "DeathEnemyOldBirdStomp":
                case "DeathEnemyOldBirdCharge":
                case "DeathEnemyOldBirdTorch":
                    return 4 * difficulty;

                // Enemies from v55
                case "DeathEnemyKidnapperFox":
                    return 4 * difficulty;
                case "DeathEnemyBarber":
                    return 4 * difficulty;

                // Enemies from v60
                case "DeathEnemyManeater":
                    return 4 * difficulty;

                // Custom causes (player)
                case "DeathPlayerJetpackGravity":
                    return 4 * difficulty;
                case "DeathPlayerJetpackBlast":
                    return 4 * difficulty;
                case "DeathPlayerLadder":
                    return 4 * difficulty;
                case "DeathPlayerMurderShovel":
                    return 4 * difficulty;
                case "DeathPlayerMurderStopSign":
                    return 4 * difficulty;
                case "DeathPlayerMurderYieldSign":
                    return 4 * difficulty;
                case "DeathPlayerMurderKnife":
                    return 4 * difficulty;
                case "DeathPlayerEasterEgg":
                    return 4 * difficulty;
                case "DeathPlayerMurderShotgun":
                    return 4 * difficulty;
                case "DeathPlayerQuicksand":
                    return 4 * difficulty;
                case "DeathPlayerStunGrenade":
                    return 4 * difficulty;

                // Custom causes (player vehicles)
                case "DeathPlayerCruiserDriver":
                case "DeathPlayerCruiserPassenger":
                case "DeathPlayerCruiserExplodeBystander":
                case "DeathPlayerCruiserRanOver":
                    return 4 * difficulty;

                // Custom causes (pits)
                case "DeathPitGeneric":
                case "DeathPitFacilityPit":
                case "DeathPitFacilityCatwalkJump":
                case "DeathPitMinePit":
                case "DeathPitMineCave":
                case "DeathPitMineElevator":
                    return 4 * difficulty;

                // Custom causes (other)
                case "DeathOtherDepositItemsDesk":
                case "DeathOtherItemDropship":
                case "DeathOtherLandmine":
                case "DeathOtherTurret":
                case "DeathOtherLightning":
                case "DeathOtherMeteor":
                case "DeathOtherSpikeTrap":
                case "DeathOtherOutOfBounds":
                    Debug.Log("Player died due to dropship.");
                    return 10 * difficulty;

                // Unknown death cause
                case "DeathUnknown":
                    return 4 * difficulty;

                // Default case if no match
                default:
                    return 2;
            }
        }







        // Sets or updates the amount of drinks for a player
        public static void setDrinkAmount(int playerClientId, int drinks)
        {
            Debug.Log("Setting Drinks.");
            if (drinksTracker.ContainsKey(playerClientId))
            {
                drinksTracker[playerClientId] += drinks;
            }
            else
            {
                drinksTracker[playerClientId] = drinks;
            }
        }






        // Calculate how many players witnessed a players death
        public int calculateSpectators(Vector3 deathPosition, int deadPlayerClientId)
        {
            Debug.Log("Calculating Spectators");
            int spectatorsCount = 0;
            // Get all active players
            //GameNetcodeStuff.PlayerControllerB[] allPlayers = FindObjectsOfType<PlayerController>();
            // Loop through each player
            //foreach (GameNetcodeStuff.PlayerControllerB player in allPlayers)
            //{
            //    // Ignore dead player
            //    if (player.clientId == deadPlayerClientId){ continue;}

            //    // Call method to check for witnesses
            //    if (witnessedEvent(player, deathPosition))
            //    {   spectatorsCount++;  }
            //}
            if (spectatorsCount > 0)
            {
                DrinksTracker.setDrinkAmount(deadPlayerClientId, spectatorsCount);
            }
            // Return the total number of spectators
            Debug.Log("spectatorsCount");
            Debug.Log(spectatorsCount);
            return spectatorsCount;
        }






        // Determine if a player witnessed a death
        public bool witnessedEvent(PlayerControllerB player, Vector3 deathPosition)
        {
            // Get the player's current position and forward direction
            Vector3 playerPosition = player.transform.position;
            Vector3 playerForward = player.transform.forward;
            // Calculate the direction from the player to the death position
            Vector3 directionToDeath = (deathPosition - playerPosition).normalized;
            // Is death within FOV
            float angle = Vector3.Angle(playerForward, directionToDeath);
            if (angle < 45f) //FOV
            {
                //// Optional: Check for line of sight using raycast (no walls or obstacles blocking view)
                //if (HasLineOfSightToPosition(deathPosition, player))
                //{
                //    // Player witnessed the event
                //    return true;
                //}
            }
            // Player did not witness the event
            return false;
        }






        // Takes a playerID and returns the amount of drinks they have as a string
        public string drinkAmountsToString(int playerClientId)
        {
            if (drinksTracker.ContainsKey(playerClientId))
            {
                int drinks = drinksTracker[playerClientId];
                if (drinks > 0)
                {
                    string drinksString = drinks.ToString();
                    return drinksString;
                }
                return "";
            }
            return "";
        }
    }

}
