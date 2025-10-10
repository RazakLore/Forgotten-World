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
            SceneManager.LoadScene(targetScene);
            //StartCoroutine(LoadYourAsyncScene(collision.gameObject));
            //SceneManager.LoadScene(targetScene);
            //collision.gameObject.transform.position = GameObject.Find(targetID + "Spawn").transform.position;
        }
    }

    //IEnumerator LoadYourAsyncScene(GameObject player)
    //{
        // Set the current Scene to be able to unload it later
        //Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        //Debug.Log("scene loading");
        // Wait until the last operation fully loads to return anything
        //while (!asyncLoad.isDone)
        //{

        //    yield return null;
        //    Debug.Log("scene loaded");
        //}

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        //SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(targetScene));

        // Unload the previous Scene
        //SceneManager.UnloadSceneAsync(currentScene);

        //Debug.Log("Scene is unloaded");

        //PlayerPrefs.SetString("SCENE_SPAWN_POINT", targetScene);

    //}
}