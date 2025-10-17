using UnityEngine;

public abstract class Ability : ScriptableObject
{
    // In here we want to have a function for every ability in the game that can be a list for every ability in entity class
    public string abilityName;
    public int MPCost;
    public string description;

    public abstract void UseAbility(Entity user, Entity target);
}

[CreateAssetMenu(fileName = "AttackAbility", menuName = "JRPG/Abilities/Attack")]
public class AttackAbility : Ability
{
    public int power;

    public override void UseAbility(Entity user, Entity target)
    {
        int damage = Mathf.Max(1, user.ATK * power - target.DEF);
        target.TakeDamage(damage);
    }
}

[CreateAssetMenu(fileName = "HealAbility", menuName = "JRPG/Abilities/Heals")]
public class HealAbility : Ability
{
    [SerializeField] private int healAmount;

    public override void UseAbility(Entity user, Entity target)
    {
        target.Heal(healAmount);
    }
}

[CreateAssetMenu(fileName = "BuffAbility", menuName = "JRPG/Abilities/Buffs")]
public class BuffAbility : Ability
{
    [SerializeField] private int amount;
    [SerializeField] private int turns;
    [SerializeField] private string chosenStat;

    public override void UseAbility(Entity user, Entity target)
    {
        target.ApplyBuff(chosenStat, amount, turns);
    }
}
