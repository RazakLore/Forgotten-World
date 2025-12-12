using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot(Item item, int quantity = 1)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public bool IsFull => quantity >= Inventory.MaxStack;
    public bool HasSpace => quantity < Inventory.MaxStack;
}

public class Inventory : MonoBehaviour
{
    public const int MaxSlots = 20;
    public const int MaxStack = 99;
    public static Inventory Instance;

    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>(MaxSlots);

    public IReadOnlyList<InventorySlot> Slots => slots;
    public int SlotCount => slots.Count;
    public bool IsFull => slots.Count >= MaxSlots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

            // Ensure we start with exactly MaxSlots capacity (optional visual in inspector)
        while (slots.Count < MaxSlots)
            slots.Add(null);
    }

    /// <summary>
    /// Adds an item to inventory. Returns true if ALL amount was added.
    /// </summary>
    public bool AddItem(Item item, int amount = 1)
    {
        if (item == null || amount <= 0) return false;

        int remaining = amount;

        // Phase 1: Stack into existing slots
        foreach (var slot in slots)
        {
            if (slot != null && slot.item == item && slot.HasSpace)
            {
                int space = MaxStack - slot.quantity;
                int add = Mathf.Min(space, remaining);
                slot.quantity += add;
                remaining -= add;

                if (remaining <= 0)
                    return true;
            }
        }

        // Phase 2: Fill empty slots
        while (remaining > 0 && slots.Count < MaxSlots)
        {
            int add = Mathf.Min(remaining, MaxStack);
            slots.Add(new InventorySlot(item, add));
            remaining -= add;
        }

        // If we get here and remaining > 0 ? inventory full
        return remaining <= 0;
    }

    /// <summary>
    /// Removes amount of item. Returns true if successful.
    /// </summary>
    public bool RemoveItem(Item item, int amount = 1)
    {
        if (item == null || amount <= 0) return false;

        int remaining = amount;

        for (int i = slots.Count - 1; i >= 0; i--)
        {
            var slot = slots[i];
            if (slot == null) continue;

            if (slot.item == item)
            {
                if (slot.quantity > remaining)
                {
                    slot.quantity -= remaining;
                    return true;
                }
                else
                {
                    remaining -= slot.quantity;
                    slots.RemoveAt(i); // Remove entire slot
                }
            }
        }

        return remaining <= 0;
    }

    // Convenience: Remove by name (only for save/load or rare cases)
    public bool RemoveItemByName(string itemName, int amount = 1)
    {
        var item = ItemDatabase.Instance?.GetItemByName(itemName);
        if (item != null)
            return RemoveItem(item, amount);
        return false;
    }

    public bool HasItem(Item item, int amount = 1)
    {
        int total = 0;
        foreach (var slot in slots)
            if (slot?.item == item)
                total += slot.quantity;
        return total >= amount;
    }

    public int GetQuantity(Item item)
    {
        int total = 0;
        foreach (var slot in slots)
            if (slot?.item == item)
                total += slot.quantity;
        return total;
    }

    // === SAVE / LOAD (your system - now works perfectly) ===
    [Serializable]
    public class InventorySaveData
    {
        public List<string> itemNames = new();
        public List<int> quantities = new();
    }

    public void SaveInventory()
    {
        var data = new InventorySaveData();

        foreach (var slot in slots)
        {
            if (slot != null && slot.item != null)
            {
                data.itemNames.Add(slot.item.itemName);
                data.quantities.Add(slot.quantity);
            }
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerInventory", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory(Func<string, Item> itemResolver)
    {
        slots.Clear();

        string json = PlayerPrefs.GetString("PlayerInventory", "");
        if (string.IsNullOrEmpty(json)) return;

        var data = JsonUtility.FromJson<InventorySaveData>(json);

        for (int i = 0; i < data.itemNames.Count; i++)
        {
            Item item = itemResolver?.Invoke(data.itemNames[i]);
            if (item != null)
            {
                slots.Add(new InventorySlot(item, data.quantities[i]));
            }
        }

        // Fill remaining slots with null for inspector clarity
        while (slots.Count < MaxSlots)
            slots.Add(null);
    }

    // Optional: Clear all
    public void Clear() => slots.Clear();
}