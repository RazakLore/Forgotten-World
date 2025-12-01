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

    [SerializeField] protected string entityName;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int currentHp;
    [SerializeField] protected int maxMp;
    [SerializeField] protected int currentMp;
    [SerializeField] protected int atk;
    [SerializeField] protected int def;
    [SerializeField] protected int agi;          // Agility algorithm in battle will determine who moves first and potentially their evasion as well
    [SerializeField] protected int lvl;          // Track the level of the player and the recommended level for enemies, can tailor stat growth algorithm around this
    [SerializeField] protected int currentXp;           // For player - add XP, for enemy - value of XP to give to player at end of battle
    [SerializeField] protected int currentGold;
    protected List<Ability> ABILITIES = new List<Ability>();        // Have a base list for abilities that in the individual enemy class we can add unique abilities to

    // Now we need GET SETS for these stats to be used in battle
    public string ENTNAME => entityName;
    public int MAXHP => maxHp;
    public int HP => currentHp;
    public int MAXMP => maxMp;
    public int MP => currentMp;
    public int ATK => atk;
    public int DEF => def;
    public int AGI => agi;
    public int LVL => lvl;
    public int XP => currentXp;
    public int GOLD => currentGold;

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

    public void InitializeEnemySpawn()
    {
        currentHp = maxHp;
        currentMp = maxMp;
    }

    public void RestoreStats()
    {
        currentHp = maxHp;
        currentMp = maxMp;
    }

    public void HalvePlayerGold()
    {
        currentGold /= 2;
    }

    public int CalculateDamageAgainst(Entity target)
    {
        // Base Damage
        float baseDamage = (this.atk / 2f) - (target.def / 4f);

        // Base Damage <= 0 -> return 0 or -1
        if (baseDamage <= 0)
        {
            int roll = Random.Range(0, 2);
            return roll;
        }

        // Range
        float range = (baseDamage / 16f) + 1f;
        int min = Mathf.FloorToInt(baseDamage - range);
        int max = Mathf.CeilToInt(baseDamage + range);

        return Random.Range(min, max + 1);
    }
}
