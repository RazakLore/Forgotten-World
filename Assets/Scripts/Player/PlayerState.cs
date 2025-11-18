using UnityEngine;

public class PlayerState : Entity
{
    public static PlayerState instance;

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

        entityName = ENTNAME;
        maxHp = 15;
        currentHp = maxHp;
        maxMp = 0;
        currentMp = maxMp;
        atk = 7;
        agi = 5;
        lvl = 1;
        currentXp = 0;
        currentGold = 0;
    }

}
