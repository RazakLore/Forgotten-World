using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AreaConnector : MonoBehaviour
{
    [Header("Where does this door go?")]
    [SerializeField] private string doorID;
    [SerializeField] private string targetID;
    [SerializeField] private string targetScene;

    public string TargetID { get { return targetID; } }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Send the targetID to the game manager

            //SceneManager.MoveGameObjectToScene(collision.gameObject, SceneManager.GetSceneByName(targetScene));
            GameManager.instance.targetDoorID = targetID;
            StartCoroutine(LoadYourAsyncScene(collision.gameObject));
            //SceneManager.LoadScene(targetScene);
            //collision.gameObject.transform.position = GameObject.Find(targetID + "Spawn").transform.position;
        }
    }

    IEnumerator LoadYourAsyncScene(GameObject player)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(targetScene));

        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);

        PlayerPrefs.SetString("SCENE_SPAWN_POINT", targetScene);

        // Place the player at the desired spawn point
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("DoorSpawnPoint");

        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }

        //player.transform.position = GameObject.Find(targetID + "Spawn").transform.position; // DOESNT WORK
    }
}