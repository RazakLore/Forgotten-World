using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    // When a battle starts, I want to instantiate the prefab of the randomised enemy based on the map.
    // I want to include the player's stats as well. Then through here handle player's inputs for the battle.
    // For the enemies, when the turn begins they will follow an attack pattern based on their individual functions.
    // This is the toughest part, but I need to determine turn order based on agility and then somehow thread the player and enemy into each other's functions to receive their damage.
    // 
    public static BattleManager instance;
    private PlayerState player;
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private RawImage enemyDisplay;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSFX;

    private Queue<Entity> turnQueue = new Queue<Entity>();
    private bool battleActive = false;

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

        //battlePanel = GameObject.Find("BattleMenus");
    }

    public void InstantiateBattle(GameObject enemyPrefab)
    {
        GameObject newEnemyObj = Instantiate(enemyPrefab);

        player = PlayerState.instance;
        enemy = newEnemyObj.GetComponent<Enemy>();
        //enemyDisplay = GameObject.Find("EnemySprite").GetComponent<RawImage>();
        enemyDisplay.texture = enemy.EnemySprite;

        //Initialize the enemy
        enemy.InitializeEnemySpawn();

        //Enable the battle UI object
        BattleUI.instance.Show();

        //Relinquish control from player?

        //Set up the turn queue, agility based
        DetermineTurnOrder();

        battleActive = true;
        StartCoroutine(ProcessTurn());
    }

    private void EndBattle()
    {
        Debug.Log("Battle has ended!");
        battleActive = false;

        if (player.HP <= 0)
        {
            Debug.Log(enemy.ENTNAME + " won the battle!");
            // Player dies
            // Respawn at church and /2 gold
            player.HalvePlayerGold();
            BattleUI.instance.Hide();
            Destroy(enemy.gameObject);
            //fade to black and respawn
            return;
        }
        else if (enemy.HP <= 0)
        {
            Debug.Log(player.ENTNAME + " won the battle!");
            // Enemy dies
            player.PlayerGainXPGold(enemy.XP, enemy.GOLD);
            GetStat statUpdater = FindFirstObjectByType<GetStat>().GetComponent<GetStat>();
            statUpdater.GrabTheStats();
            CleanupBattleUI();
            return;
        }
        else
        {
            Debug.Log("Somebody ran away!");
            // - Player or enemy ran away
            CleanupBattleUI();
        }
        
    }

    private void CleanupBattleUI()
    {
        Destroy(enemy.gameObject);  // Remove the enemy once any previous condition is fulfilled
        enemy = null;
        BattleUI.instance.Hide();
        PlayerMovement.instance.SetCanMove(true);
        UIStateController.CurrentState = UIState.Gameplay;
    }

    private void DetermineTurnOrder()
    {
        // player goes first if agi > enemy agi
        turnQueue.Clear();

        if (player.AGI >= enemy.AGI)
        {
            turnQueue.Enqueue(player);
            turnQueue.Enqueue(enemy);
        }
        else
        {
            turnQueue.Enqueue(enemy);
            turnQueue.Enqueue(player);
        }
    }

    private IEnumerator ProcessTurn()
    {
        yield return BattleMessageLog.Instance.ShowMessage($"A {enemy.ENTNAME} appeared!");
        while (battleActive)
        {
            Entity actor = turnQueue.Dequeue();

            if (actor == player)
            {
                yield return StartCoroutine(PlayerTurn());
            }
            else
            {
                // Enemy's turn
                yield return StartCoroutine(EnemyTurn());
            }

            // Requeue the actor
            turnQueue.Enqueue(actor);

            // Check if the battle ended
            if (player.HP <= 0 || enemy.HP <= 0)
            {
                EndBattle();
                yield break;
            }
        }
        yield return BattleMessageLog.Instance.ShowMessage("");
    }

    private IEnumerator PlayerTurn()
    {
        // Ensure UI shows current stats and selection starts at first button
        BattleUI.instance.UpdateStats();
        BattleUI.instance.ResetSelectionToFirst();

        int action = -1;

        // Poll the UI each frame until player confirms (HandleMenuInput returns index or -1)
        while (action == -1)
        {
            action = BattleUI.instance.HandleMenuInput(); // returns -1 while choosing, or 0/1 when confirmed
            yield return null;
        }

        // Execute the chosen action
        switch (action)
        {
            case 0: // Fight
                Debug.Log("Player chooses FIGHT");
                yield return BattleMessageLog.Instance.ShowMessage($"{player.ENTNAME} attacks!");
                int dmg = player.CalculateDamageAgainst(enemy);
                enemy.TakeDamage(dmg);
                GetComponent<AudioSource>().PlayOneShot(attackSFX);
                yield return BattleMessageLog.Instance.ShowMessage($"{enemy.ENTNAME} takes {dmg} damage!");
                BattleUI.instance.UpdateStats();
                // Optional: small delay for readability/animation
                //yield return new WaitForSeconds(0.4f);
                yield return BattleMessageLog.Instance.ShowMessage(""); // Clear the action message
                break;

            case 1: // Flee
                Debug.Log("Player chooses FLEE");
                yield return BattleMessageLog.Instance.ShowMessage($"{player.ENTNAME} runs away!");
                // Implement flee chance if you want; for now just end battle as before
                EndBattle();
                yield break;
        }
    }

    private IEnumerator EnemyTurn()
    {
        yield return BattleMessageLog.Instance.ShowMessage($"{enemy.ENTNAME} attacks!");
        //yield return new WaitForSeconds(1f);

        int dmg = player.CalculateDamageAgainst(player);
        player.TakeDamage(dmg);
        GetComponent<AudioSource>().PlayOneShot(attackSFX);
        yield return BattleMessageLog.Instance.ShowMessage($"{player.ENTNAME} takes {dmg} damage!");

        //yield return new WaitForSeconds(0.5f);
        yield return BattleMessageLog.Instance.ShowMessage("");
    }
}
