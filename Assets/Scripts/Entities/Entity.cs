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

    protected int maxHp;
    protected int currentHp;
    protected int maxMp;
    protected int currentMp;
    protected int atk;
    protected int def;
    protected int agi;          // Agility algorithm in battle will determine who moves first and potentially their evasion as well
    protected int lvl;          // Track the level of the player and the recommended level for enemies, can tailor stat growth algorithm around this
    protected int xp;           // For player - add XP, for enemy - value of XP to give to player at end of battle
    protected List<Ability> ABILITIES = new List<Ability>();        // Have a base list for abilities that in the individual enemy class we can add unique abilities to
    
    // Now we need GET SETS for these stats to be used in battle
    public int MAXHP => maxHp;
    public int HP => currentHp;
    public int MAXMP => maxMp;
    public int MP => currentMp;
    public int ATK => atk;
    public int DEF => def;
    public int LVL => lvl;
    public int XP => xp;

    // ------------------

    // BASE FUNCTIONS
    //Attack
    //Defend
    //Use Ability
    //Flee / ALLOWED OR NOT AND FREQUENCY

    protected void Attack()
    {
        // Deal damage based on attack stat
    }

    protected void Defend()
    {
        // Reduces incoming damage by whatever percent
    }

    protected void UseAbility(int index, Entity target)
    {
        // Get the intended ability from the ability list and use it in battle to damage the opponent
        if (index < 0 || index >= ABILITIES.Count) return;
        ABILITIES[index].UseAbility(this, target);
    }

    protected void Flee()
    {
        // Determine odds to leave battle
    }

    public void TakeDamage(int damage)
    {
        currentHp = Mathf.Max(0, currentHp - damage);
    }

    public void Heal(int amount)
    {
        currentHp = Mathf.Min(maxHp, currentHp + amount);
    }

    public void ApplyBuff(string stat, int amount, int turns)
    {
        // Figure out which stat is being buffed, figure out if 1 or 2 stages are being buffed (15 or 30%)
        // If already at max stage dont add to the stat but cast the ability.
    }
}
