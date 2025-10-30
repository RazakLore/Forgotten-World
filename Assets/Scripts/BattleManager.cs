using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // When a battle starts, I want to instantiate the prefab of the randomised enemy based on the map.
    // I want to include the player's stats as well. Then through here handle player's inputs for the battle.
    // For the enemies, when the turn begins they will follow an attack pattern based on their individual functions.
    // This is the toughest part, but I need to determine turn order based on agility and then somehow thread the player and enemy into each other's functions to receive their damage.
    // 
    public static BattleManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartBattle()
    {
        Debug.Log("Battle has started!");
        
        // Create an instance from a prefab here!
        // This function should take in parameters for the randomly chosen enemy in RandomEncounter, as that class contains the info of which scene to pick enemies from
        // Instantiate a prefab based on the parameter
    }

    private void EndBattle()
    {
        Debug.Log("Battle has ended!");
    }
}
