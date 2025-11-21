using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] protected string npcName;
    [SerializeField] protected int moveSpeed;
    [SerializeField] protected bool isStaticNPC;

    // Set the NPC name in editor for each prefab
    // Give them a move speed for any unique npc (like children move faster. elderly move slower)
    // Is the NPC a shopkeeper or king? If so, they are static and do not move
    // Change direction sprite based on enum direction, left right up down
    // Interaction method that accesses the root objects Dialogue script? When collision stay with player interaction box, enable exclamation point above head, when space or enter pressed run dialogue

    private void Move()
    {
        //We need a timer, a random number generator for the enum value, the animation, the agent.setdestination 16 pixels away
        if (!isStaticNPC)
        {

        }
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
