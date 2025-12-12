using UnityEngine;
using System;

public enum ItemType
{
    Consumable,
    Weapon,
    Armour,
    Accessory,
    KeyItem
}

[CreateAssetMenu(fileName = "New Item", menuName = "JRPG/Item")]
public class Item : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName = "New Item";
    [TextArea] public string description = "";
    public ItemType itemType;

    [Header("Stacking")]
    public int maxStack = 99;

    [Header("Consumable Settings")]
    public int healAmount = 0;
    public int mpRestoreAmount = 0;

    [Header("Equipment Settings (only if Equipment")]
    public int attackBonus = 0;
    public int defenceBonus = 0;
    
    [Header("Shop Prices")]
    public int buyPrice = 50;
    public int sellPrice = 25;

    public bool UseOn(PlayerState target)
    {
        if (itemType != ItemType.Consumable)
        {
            Debug.Log($"{itemName} is not a consumable!");
            return false;
        }

        bool used = false;

        if (healAmount > 0)
        {
            target.Heal(healAmount);
            used = true;
        }

        if (mpRestoreAmount > 0)
        {
            target.RestoreMP(mpRestoreAmount);
            used = true;
        }

        return used;
    }

    public bool Equip(PlayerState player)
    {
        if (itemType == ItemType.Weapon)
        {
            //return player.EquipItem(this);
        }

        Debug.Log($"{itemName} is not equippable!");
        return false;
    }

    public bool IsConsumable() => itemType == ItemType.Consumable;
    public bool IsEquippable() => itemType == ItemType.Weapon || itemType == ItemType.Armour || itemType == ItemType.Accessory;
}
