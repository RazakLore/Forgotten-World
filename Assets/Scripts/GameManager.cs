using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Prefabs")]
    public GameObject playerPrefab;

    [HideInInspector] public GameObject playerInstance;
    private SaveData pendingSaveData;
    private string pendingConnectorID; // stores door ID until the scene is loaded

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

    // How can I thread the TargetDoorID through here to be accessed in the next scene?
}
