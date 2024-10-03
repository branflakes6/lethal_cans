using System;
using System.Collections.Generic;
using GameNetcodeStuff;

#nullable enable

namespace LethalCans

class DrinksTracker {

    // This dict stores playerClientID's and the number of drinks associated with each ID
    public static Dictionary<int, int> drinksTracker = new Dictionary <int, int>();

    // Calcualte the number of drinks based on how a player died
    public static int getDrinkAmount(AdvancedCauseOfDeath? causeOfDeath) {

        // This gets the death tag from coroner
        var deathTag = ((AdvancedCauseOfDeath) causeOfDeath).GetLanguageTag();

        // For V0.1 we can just use a switch statement, later we will add a proper config file that can be edited but that requires some work to read and write to
        switch(deathTag) 
            {
            case "DeathEnemyBaboonHawk":
                return 1;
            case "DeathEnemyBracken":
                return 2
            }
        return 0
    }

    // Sets or updates the amount of drinks for a player
    public static void setDrinkAmount(int playerClientId, int drinks){

    }
    
    // Calculate how many players witnessed a players death
    public  int calculateSpectators(){
        // Loop through each player and call witnessedEvent() to check if they saw the event

    }

    // Determine if a player witnessed a death
    public  bool witnessedEvent(){
        // Compare players current field of view with the coordinates of the event
        // public bool HasLineOfSightToPosition(Vector3 pos, float width = 45f, int range = 60, float proximityAwareness = -1f)

    }

    // Takes a playerID and returns the amount of drinks they have as a string
    public static string drinkAmountsToString(int playerClientId){


    }
}