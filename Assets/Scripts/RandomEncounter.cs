using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    // Check the scene is a scene where monsters are present

    // Have a function that starts a battle
    // Have a function that handles the timer
    // Timer that is randomly chosen between a range that only counts up when player input is held down, thread into player input?
    // Maybe take this class and run the timer function in player update, then run start battle to take control in here?
    [Header("Random Encounter Variables")]
    [SerializeField] private float randomTimer;
    [SerializeField] private int randomTimerThreshold;
    [SerializeField] private int randomTimerRangeMin = 4;
    [SerializeField] private int randomTimerRangeMax = 9;

    [Header("References")]
    [SerializeField] private BattleManager battleManager;
    //[SerializeField] private GameObject battlePanel;
    [SerializeField] private EncounterTable encounterTable;

    private bool canTrigger = true;

    private void Start()
    {
        randomTimerThreshold = Random.Range(randomTimerRangeMin, randomTimerRangeMax);
        encounterTable = FindFirstObjectByType<EncounterTable>();
        battleManager = BattleManager.instance;
    }

    public void HandleEncounterTimer(float moveX, float moveY)
    {
        bool isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveY) > 0.1f;

        if (isMoving) // And if in a dangerous scene
        {
            randomTimer += Time.deltaTime;

            if (randomTimer > randomTimerThreshold)
            {
                TriggerEncounter();
            }
        }
    }

    private void TriggerEncounter()
    {
        canTrigger = false;
        
        randomTimer = 0f;
        randomTimerThreshold = Random.Range(randomTimerRangeMin, randomTimerRangeMax);
        GameObject chosenPrefab = encounterTable.GetRandomEnemy();

        if (battleManager != null )
        {
            battleManager.InstantiateBattle(chosenPrefab);
        }
        else
        {
            Debug.LogWarning("No battle manager was found!");
        }

        PlayerMovement.instance.SetCanMove(false);
    }

    public void ResumeEncounterDetection()
    {
        canTrigger = true;
        //if (battlePanel.activeInHierarchy)
        //    battlePanel.SetActive(false);
        
    }
}
