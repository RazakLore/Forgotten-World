using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private Enemy bossStats;
    [SerializeField] private string bossFlagName;

    [Header("Optional Item Requirement")]
    [SerializeField] private Item requiredItem; // Null = No requirement
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
