using UnityEngine;

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

            //Spawn the player for testing purposes, spawn later after the game has been started or loaded etc
            if (playerInstance == null && playerPrefab != null)
            {
                playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                DontDestroyOnLoad(playerInstance);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
