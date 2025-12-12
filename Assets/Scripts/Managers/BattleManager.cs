using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private AudioClip battleMusic;
    private AudioClip overworldMusic;

    private Queue<Entity> turnQueue = new Queue<Entity>();
    private bool battleActive = false;
    private int playerChosenAction;
    private int actionStep = 0;

    // Boss stuff
    private bool bossBattle = false;
    private BossTrigger currentBossTrigger = null;
    private string currentBossFlag = "";

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

    public void InstantiateBattle(GameObject enemyPrefab)                                               // This is a normal battle
    {
        SetupBattle(enemyPrefab, false, null, "");
    }

    public void InstantiateBossBattle(GameObject boss, BossTrigger trigger, string bossFlagName = "")   // This is a boss battle
    {
        SetupBattle(boss, true, trigger, bossFlagName);                                                 // and boss battles need flags
    }

    private void SetupBattle(GameObject enemyPrefab, bool isBoss, BossTrigger trigger, string bossFlagName)
    {
        GameObject newEnemyObj = Instantiate(enemyPrefab);      // Instantiate the fed in prefab

        player = PlayerState.instance;                          // The player is the static player
        enemy = newEnemyObj.GetComponent<Enemy>();              // Grab the enemy class from the prefab
        enemyDisplay.texture = enemy.EnemySprite;               // Set the enemy display to the enemy class's texture

        enemy.InitializeEnemySpawn();                           // Set enemy's health and mp maxed

        BattleUI.instance.Show();                               // Display the battle UI
        DetermineTurnOrder();                                   // Based on agility, determine who will go first

        battleActive = true;                                    // The battle IS active

        bossBattle = isBoss;                                    // Quick section to set all the boss relevant variables if any
        currentBossTrigger = trigger;
        currentBossFlag = bossFlagName;

        // Disable player movement
        PlayerMovement.instance.SetCanMove(false);
        UIStateController.CurrentState = UIState.Battle;

        overworldMusic = FindFirstObjectByType<SceneStartup>().GetComponent<AudioSource>().clip;            // Set the music to the battle theme
        AudioSource battleAudioSource = FindFirstObjectByType<SceneStartup>().GetComponent<AudioSource>();
        battleAudioSource.clip = battleMusic;
        battleAudioSource.Play();
        actionStep = 0;

        StartCoroutine(ProcessTurn());                          // Start the battle
    }

    private void DetermineTurnOrder()
    {
        turnQueue.Clear();              // Clear the turn order

        if (player.AGI >= enemy.AGI)    // If the player is faster than or tied with the enemy
        {
            turnQueue.Enqueue(player);  // Queue the player first
            turnQueue.Enqueue(enemy);   // Queue the enemy second
        }
        else
        {
            turnQueue.Enqueue(enemy);   // Or vice versa
            turnQueue.Enqueue(player);
        }

        // To fix the bug, we could enqueue in first position for both a ChoosingTurn class (if it has to be a class)?
        // Other option is to track the actions with an int, once int = 2 inside the if playerchosenaction == -1, reset to 0
    }

    private IEnumerator ProcessTurn()
    {
        yield return BattleMessageLog.Instance.ShowMessage($"A {enemy.ENTNAME} appeared!");

        // If the player is NOT first, allow them to choose an action for the turn still
        yield return StartCoroutine(PlayerChoosing());  // Without this, the battle falls apart. Enemy moves first before player can take turn.
        Debug.Log(turnQueue.Peek()); // This lets us see the turn queue

        while (battleActive)
        {
            Entity actor = turnQueue.Dequeue();
            Debug.Log(turnQueue.Peek());
            if (playerChosenAction == -1 && actionStep == 2)       // Before every turn after T1, the player must choose their action
            {
                actionStep = 0;
                yield return StartCoroutine(PlayerChoosing());
            }

            if (actor == player)
            {
                yield return StartCoroutine(PlayerTurn(playerChosenAction));    // If the actor this current turn is the player, do player choice
                playerChosenAction = -1;                                        // Reset the player's chosen action for reselection above
            }
            else
            {
                yield return StartCoroutine(EnemyTurn());                       // If not the player, then actor is the enemy. Do enemy turn.
            }

            // Requeue the actor if alive
            if (actor.HP > 0)
                turnQueue.Enqueue(actor);

            actionStep++;   // Count up 1, from 0. Will go to 1 after first action, 2 after second action, which forces player choose

            Debug.Log(turnQueue.Peek());

            // Check win/loss
            if (player.HP <= 0 || enemy.HP <= 0)
            {
                EndBattle();
                yield break;
            }

            // Optional small delay so messages don't flood
            yield return null;
        }
        yield return BattleMessageLog.Instance.ShowMessage("");
    }

    private void EndBattle()
    {
        Debug.Log("Battle has ended!");
        battleActive = false;

        if (player.HP <= 0)         // Player has died
        {
            Debug.Log(enemy.ENTNAME + " won the battle!");
            player.HalvePlayerGold();                           // Halve the player's gold
            BattleUI.instance.Hide();                           // Hide the battle UI
            Destroy(enemy.gameObject);                          // Destroy the enemy prefab
            //fade to black and respawn

            //Temporary measure to make sure death has consequence in the upload
            if (!Application.isEditor)
                Application.Quit();

                return;
        }
        else if (enemy.HP <= 0)     // Enemy has died
        {
            Debug.Log(player.ENTNAME + " won the battle!");
            player.AddXP(enemy.XP);                             // Player class method that adds XP to their current XP
            player.AddGold(enemy.GOLD);                         // Same thing but for Gold

            GetStat[] statUpdaters = FindObjectsByType<GetStat>(FindObjectsSortMode.None);  // Update the battle stats and pause menu stats
            foreach (GetStat stat in statUpdaters)                                          // by looping through everything that has the class
                stat.GrabTheStats();

            if (bossBattle && currentBossTrigger != null)                                   // If it was a boss, run the boss defeated method
                currentBossTrigger.OnBossDefeated();

            CleanupBattleUI();
            return;
        }
        else
        {
            Debug.Log("Somebody ran away!");
            CleanupBattleUI();
        }

        bossBattle = false;
        currentBossTrigger = null;
        currentBossFlag = "";
    }

    private void CleanupBattleUI()
    {
        Destroy(enemy.gameObject);  // Remove the enemy once any previous condition is fulfilled
        enemy = null;
        BattleUI.instance.Hide();
        PlayerMovement.instance.SetCanMove(true);
        UIStateController.CurrentState = UIState.Gameplay;

        AudioSource battleAudioSource = FindFirstObjectByType<SceneStartup>().GetComponent<AudioSource>();
        battleAudioSource.clip = overworldMusic; battleAudioSource.Play();
    }

    private IEnumerator PlayerChoosing()
    {
        // Ensure UI shows current stats and selection starts at first button
        BattleUI.instance.UpdateStats();
        BattleUI.instance.ResetSelectionToFirst();
        yield return BattleMessageLog.Instance.ShowMessage("Select your action.");

        playerChosenAction = -1;

        // Poll the UI each frame until player confirms (HandleMenuInput returns index or -1)
        while (playerChosenAction == -1)
        {
            playerChosenAction = BattleUI.instance.HandleMenuInput(); // returns -1 while choosing, or 0/1 when confirmed
            
            if (playerChosenAction == 1)                              // Override for running away, has to happen before any actions
            {
                yield return StartCoroutine(AttemptFlee());
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator PlayerTurn(int action)
    {
        // Execute the chosen action
        switch (action)
        {
            case 0: // Fight
                Debug.Log("Player chooses FIGHT");
                yield return BattleMessageLog.Instance.ShowMessage($"{player.ENTNAME} attacks!");
                int dmg = player.CalculateDamageAgainst(enemy);
                enemy.TakeDamage(dmg);
                GetComponent<AudioSource>().PlayOneShot(attackSFX);
                StartCoroutine(FlashUIRawImage(enemyDisplay));
                yield return BattleMessageLog.Instance.ShowMessage($"{enemy.ENTNAME} takes {dmg} damage!");
                BattleUI.instance.UpdateStats();
                yield return BattleMessageLog.Instance.ShowMessage(""); // Clear the action message
                break;

            case 1: // Flee
                Debug.Log("Player chooses FLEE");

                if (bossBattle)
                {
                    Debug.Log("Player can't escape!");
                    yield return BattleMessageLog.Instance.ShowMessage($"You can't escape!");
                    break;
                }
                yield return BattleMessageLog.Instance.ShowMessage($"{player.ENTNAME} runs away!");
                // Implement flee chance if you want; for now just end battle as before
                EndBattle();
                yield break;

            case 2: //Heal
                yield return BattleMessageLog.Instance.ShowMessage($"{player.ENTNAME} heals!");
                if (player.MP < 3)
                {
                    yield return BattleMessageLog.Instance.ShowMessage($"Not enough MP!");
                }
                else
                    player.Heal(Random.Range(15, 20));
                BattleUI.instance.UpdateStats();
                yield break;
        }
    }

    private IEnumerator EnemyTurn()
    {
        yield return BattleMessageLog.Instance.ShowMessage($"{enemy.ENTNAME} attacks!");
        //yield return new WaitForSeconds(1f);

        int dmg = enemy.CalculateDamageAgainst(player);
        player.TakeDamage(dmg);
        GetComponent<AudioSource>().PlayOneShot(attackSFX);
        yield return BattleMessageLog.Instance.ShowMessage($"{player.ENTNAME} takes {dmg} damage!");

        //yield return new WaitForSeconds(0.5f);
        yield return BattleMessageLog.Instance.ShowMessage("");
    }

    private IEnumerator AttemptFlee()
    {
        float chance = Mathf.Clamp01((float)player.AGI / (player.AGI + enemy.AGI));

        if (bossBattle)
        {
            Debug.Log("Player can't escape!");
            yield return BattleMessageLog.Instance.ShowMessage($"You can't escape!");
            yield break;
        }

        if (Random.value <= chance)
        {
            yield return BattleMessageLog.Instance.ShowMessage("You fled successfully!");
            battleActive = false;
            EndBattle();
            yield break;
        }
        else
        {
            yield return BattleMessageLog.Instance.ShowMessage("Flee failed!");
            // Important: set an action so the turn system continues
            playerChosenAction = 3;
        }
    }

    private IEnumerator FlashUIRawImage(RawImage img)
    {
        Material mat = img.material;

        // Flash white
        mat.SetColor("_Tint", Color.white * 3f);

        yield return new WaitForSeconds(0.1f);

        // Reset (normal color)
        mat.SetColor("_Tint", Color.white);
    }
}
