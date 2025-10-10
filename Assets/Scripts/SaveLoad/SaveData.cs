using System;

[Serializable]  // Turn into JSON
public class SaveData
{
    public string sceneName;
    public float playerX;
    public float playerY;
    public float playerZ;

    // Later, add things like
    // public int playerHP;
    // public int playerLevel;
    // public List<string> inventoryItems;

    // level up requirement doubles every levelup
}
