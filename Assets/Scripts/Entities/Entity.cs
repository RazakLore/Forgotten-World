using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // STATS
    // HP
    // MP
    // ATK
    // DEF
    // AGI
    // LEVEL
    // LIST OF ABILITIES

    protected int HP;
    protected int MP;
    protected int ATK;
    protected int DEF;
    protected int AGI;          // Agility algorithm in battle will determine who moves first and potentially their evasion as well
    protected int LVL;          // Track the level of the player and the recommended level for enemies, can tailor stat growth algorithm around this
    protected int XP;           // For player - add XP, for enemy - value of XP to give to player at end of battle
    //protected List</*CLASS*/> ABILITIES = new List</*CLASS*/>;        // Have a base list for abilities that in the individual enemy class we can add unique abilities to

    // ------------------

    // BASE FUNCTIONS
    //Attack
    //Defend
    //Use Ability
    //Flee / ALLOWED OR NOT AND FREQUENCY

    protected void Attack()
    {

    }

    protected void Defend()
    {

    }

    protected void UseAbility()
    {
    
    }

    protected void Flee()
    {

    }
}
