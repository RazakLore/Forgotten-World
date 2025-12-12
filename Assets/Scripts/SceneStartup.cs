using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartup : MonoBehaviour
{
    [Header("Temporary NPC")]
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform npcSpawnPoint;
    [SerializeField] private string gameFlagID;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  // When this object is enabled, so that the scene has been loaded first, subscribe to this method
        HandleTempNPCSpawn();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // When this object is disabled, so that the scene has unloaded first, unsubscribe from this method
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayStart());
        if (GameManager.instance == null) return;

        string targetDoorID = GameManager.instance.targetDoorID;
        if (string.IsNullOrEmpty(targetDoorID)) return;

        GameObject player = GameObject.FindWithTag("Player");   // Get the player
        player.GetComponent<RandomEncounter>().GetEncounterTable();

        GameObject spawnPoint = GameObject.FindGameObjectWithTag("DoorSpawnPoint");     // Debug just to grab the ONLY spawn point in the scene, change to search by ID and tag later?
        Debug.Log(spawnPoint.name);

        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;  // Set the player's position to the spawn point's position
            Debug.Log("NOT NULL");
        }

        // Find the targeted door
        //AreaConnector[] doors = FindObjectsByType<AreaConnector>(FindObjectsSortMode.None);
        //foreach (AreaConnector d in doors)
        //{
        //    if (d.TargetID == targetDoorID)
        //    {
        //        //GameObject player = GameObject.FindWithTag("Player");
        //        if (player != null)
        //        {
        //            player.transform.position = d.transform.position;       // It just doesnt send to the correct d pos, also pos are not on same origin??
        //        }
        //        break;
        //    }
        //}
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1f);
    }

    private void HandleTempNPCSpawn()
    {
        // If no Temporary NPC is in this scene, ignore the rest of this function
        if (npcPrefab == null || npcSpawnPoint == null || string.IsNullOrEmpty(gameFlagID))
            return;

        bool isDead = GameFlags.instance.IsBossDead(gameFlagID);

        if (!isDead)
        {
            Instantiate(npcPrefab, npcSpawnPoint.position, npcSpawnPoint.rotation);
        }
    }
}
