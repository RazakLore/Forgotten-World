using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private int playerHP;
    [SerializeField] private int currentHP; // This tracks the value of HP from the max of playerHP, tracking damage done to player
    [SerializeField] private int playerMP;
    [SerializeField] private int currentMP;
    [SerializeField] private int playerATK;
    [SerializeField] private int currentATK; // In the case we want debuffs, this could be a nice value to at least write down first to remember
    [SerializeField] private int playerAGI;
    [SerializeField] private int currentAGI;
    // THESE WILL BE OVERRIDES INSTEAD, INHERIT FROM A BASE ENTITY CLASS FOR BOTH PLAYER AND ALL MONSTERS
    //
}
