using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerState : Entity
{
    public static PlayerState instance;
    //public List<Item> inventory = new List<Item>();
    //private int maxInventorySlots = 20;

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

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void Update()
    {
        // PRESS T ? Heal 30 HP using a real consumable if possible
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool usedRealItem = false;

            foreach (var slot in Inventory.Instance.Slots)
            {
                // slot can be null (empty slots in your list)
                if (slot != null &&
                    slot.item != null &&
                    slot.item.IsConsumable() &&           // ? ? ? MUST HAVE () 
                    slot.item.healAmount > 0)
                {
                    if (slot.item.UseOn(this))               // Calls Item.Use(PlayerState)
                    {
                        Inventory.Instance.RemoveItem(slot.item, 1);
                        Debug.Log($"[TEST] Used {slot.item.itemName} ? +{slot.item.healAmount} HP");
                        usedRealItem = true;
                        break;
                    }
                }
            }

            if (!usedRealItem)
            {
                Heal(30);
                Debug.Log("[TEST] No healing item found ? Direct +30 HP");
            }
        }
    }
#endif
}
