using UnityEngine;

public class GameFlags : MonoBehaviour
{
    public static GameFlags instance = new GameFlags();

    [System.Serializable]
    public class BossFlag
    {
        public string name;
        public bool defeated = false;
    }

    [SerializeField] private BossFlag[] bossFlags;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetBossDead(string bossName)
    {
        if (bossFlags == null)
        {
            Debug.LogError("BossFlags is null.");
            return;
        }
        
        foreach (var boss in bossFlags)
        {
            if (boss.name == bossName)
            {
                boss.defeated = true;
                return;
            }
        }

        Debug.LogWarning($"Boss name '{bossName}' not found in GameFlags!");
    }

    public bool IsBossDead(string bossName)
    {
        if (bossFlags == null) return false;

        foreach (var boss in bossFlags)
        {
            if (boss.name == bossName)
                return boss.defeated;
        }
        return false;
    }
}
