using System.Collections.Generic;
using UnityEngine;

public class PlayerState : Entity
{
    public static PlayerState instance;
    public List<Item> inventory = new List<Item>();
    private int maxInventorySlots = 20;

    private int maxLvl, maxLvlStatValue;

    private const float xpK = 0.969f; // Curve height
    private const float xpP = 3.489f; // Exponential curve steepness

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

    private int TotalXpForLevel(int L)
    {
        if (L <= 1) return 0;
        return Mathf.RoundToInt(xpK * Mathf.Pow(L, xpP));
    }

    private int XPToNextFromCurrentLevel()
    {
        return TotalXpForLevel(lvl + 1) - TotalXpForLevel(lvl);
    }

    public void AddXP(int amount)
    {
        if (lvl >= 99)
            return;

        currentXp += amount;

        // Loop in the case of one big XP drop causes multiple level-ups (I.e metal slimes or tough bosses)
        while (lvl < 99 && currentXp >= TotalXpForLevel(lvl + 1))
        {
            LevelUp();
        }

        Debug.Log(XPToNextFromCurrentLevel());
    }

    private void LevelUp()
    {
        lvl++;

        maxHp += Random.Range(3, 6);
        atk += Random.Range(1, 3);
        agi += Random.Range(1, 4);
        def += Random.Range(1, 3); 
        if (lvl >= 3)
            maxMp += Random.Range(2, 5);

        currentHp = maxHp;
        currentMp = maxMp;

        CheckLevelUpStats();
    }

    public void CheckLevelUpStats()
    {
        if (lvl >= 99)
            currentXp = 0;

        maxHp = Mathf.Min(maxHp, 999);
        maxMp = Mathf.Min(maxMp, 999);
        atk = Mathf.Min(atk, 999);
        agi = Mathf.Min(agi, 999);
        def = Mathf.Min(def, 999);
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
    }

    public bool AddItem(Item newItem)
    {
        if (inventory.Count >= 20)
            return false; // ui message saying bag full

        inventory.Add(newItem);
        return true;
    }

    public void RemoveItem(Item item)
    {
        if (inventory.Contains(item))
            inventory.Remove(item);
    }
}
