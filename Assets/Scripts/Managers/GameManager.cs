using Unity.Mathematics;
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

    [Header("Transfer Variables")]
    public string targetDoorID;

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

    public void SpawnPlayerAtSceneStart()
    {
        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab, new Vector3(-1, 23), Quaternion.identity);
            playerInstance.tag = "Player";
        }
    }

    public void LoadGame(string savedScene)
    {
        SceneManager.LoadScene(savedScene);
    }

    // How can I thread the TargetDoorID through here to be accessed in the next scene?
}
