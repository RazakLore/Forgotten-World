using UnityEngine;
using System;

[Serializable]
public class Item
{
    public string itemName;
    public bool consumable;
    public int healAmount;
    public Action<PlayerState, Enemy> UseAction; // Custom effect in battle

    public void Use(PlayerState player, Enemy enemy)
    {
        UseAction?.Invoke(player, enemy);
    }
}
