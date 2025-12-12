using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private Enemy bossStats;
    [SerializeField] private string bossFlagName;

    [Header("Optional Requirements")]
    [SerializeField] private Item requiredItem; // Null = No requirement
    [SerializeField] private string requiredFlagName;
    [SerializeField] private bool requiredFlagValue = true;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            OnPlayerEntered();
    }

    public void OnPlayerEntered()
    {
        if (triggered) return;
        

        if (requiredItem != null && !PlayerHasItem(requiredItem)) // If required item is not null
        {
            Debug.Log("The player is missing " + requiredItem);
            return;
        }

        if (!string.IsNullOrWhiteSpace(requiredFlagName))
        {
            bool isFlagSet = GameFlags.instance.IsBossDead(requiredFlagName);
            if (isFlagSet != requiredFlagValue)
            {
                Debug.Log("Required game flag " + requiredFlagName + " is not set to " + requiredFlagValue);
                return;
            }
        }

        triggered = true;
        // Tell battle manager to start the boss battle
        BattleManager.instance.InstantiateBossBattle(boss, this, bossFlagName);
    }

    private bool PlayerHasItem(Item item)
    {
        foreach (var i in Inventory.Instance.Slots)
        {
            if (i.item.itemName == item.itemName)
                return true;
        }
        return false;
    }

    public void OnBossDefeated()
    {
        GameFlags.instance.SetBossDead(bossFlagName);
        Destroy(gameObject);
    }

    public void OnBossFailed()
    {
        triggered = false;
        bossStats.RestoreStats();
    }
}
