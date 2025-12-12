using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "JRP/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public static ItemDatabase Instance;

    [SerializeField] private List<Item> allItems = new List<Item>();
    private Dictionary<string, Item> lookup;

    public void Initialise()
    {
        Instance = this;
        lookup = new Dictionary<string, Item>();
        foreach (var item in allItems)
        {
            if (!string.IsNullOrEmpty(item.itemName))
                lookup[item.itemName] = item;
        }    
    }

    public Item GetItemByName(string name)
    {
        if (lookup == null)
            Initialise();
        lookup.TryGetValue(name, out Item item);
        return item;
    }

    /// <summary> Call this from a GameManager on startup.</summary>
    [RuntimeInitializeOnLoadMethod]
    private static void AutoInit()
    {
        var db = Resources.Load<ItemDatabase>("ItemDatabase");  // Put in the Resources folder
        db?.Initialise();
    }
}
