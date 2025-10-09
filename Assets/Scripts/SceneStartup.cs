using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartup : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.instance == null) return;

        string targetDoorID = GameManager.instance.targetDoorID;
        if (string.IsNullOrEmpty(targetDoorID)) return;

        GameObject player = GameObject.FindWithTag("Player");

        //GameObject spawnPoint = GameObject.FindGameObjectWithTag("DoorSpawnPoint");

        //if (spawnPoint != null)
        //{
        //    player.transform.position = spawnPoint.transform.position;
        //}
            // Find the targeted door
            //AreaConnector[] doors = FindObjectsByType<AreaConnector>(FindObjectsSortMode.None);
            //foreach (AreaConnector d in doors)
            //{
            //    if (d.TargetID == targetDoorID)
            //    {
            //        GameObject player = GameObject.FindWithTag("Player");
            //        if (player != null)
            //        {
            //            player.transform.position = d.transform.position;       // It just doesnt send to the correct d pos, also pos are not on same origin??
            //        }
            //        break;
            //    }
            //}
        }
}
