using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Prefabs")]
    public GameObject playerPrefab;

    [HideInInspector] public GameObject playerInstance;

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
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("StartingScene");
    }

    public void LoadGame(string savedScene)
    {
        SceneManager.LoadScene(savedScene);
    }

    public void EnsurePlayerExists()
    {
        //Spawn the player for testing purposes, spawn later after the game has been started or loaded etc
        if (playerInstance == null && playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(playerInstance);
        }
    }
}
