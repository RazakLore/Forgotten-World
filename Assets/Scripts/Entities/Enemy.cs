using UnityEngine;

public class Enemy : Entity
{
    // This class will serve as the base for all the enemies.
    // We will override all the stats from Entity in individual prefabs. Our functions should draw stats without having to be edited ideally.
    // We have to add abilities to the list in the prefab inspector.
    // ATTACK PATTERNS are also ideal to have, so I can easily preprogram battle logic for enemies. I.e. t1 is attack t2 is defend t3 is attack t4 is flee.
    // We can probably make a base attack pattern to inherit from. Probably use a state machine to see each pattern and the last action can change state to pattern 2 for example.
    // Randomise the patterns by having some actions provide a 50/50 to continue or change pattern?

    private void Awake()
    {
        //maxHp = MAXHP;
        //currentHP = maxHP;
        //maxMP = MAXMP;
        //currentMP = maxMP;
        //currentATK = ATK;
        //currentAGI = AGI;
    }
}
