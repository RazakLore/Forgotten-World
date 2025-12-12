using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string firstSceneName = ""; // Scene to load on New Game

    public void NewGame()
    {
        // For now, just load the first scene
        SceneManager.LoadScene(firstSceneName);

        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;

        if (GameManager.instance != null)
        {
            GameManager.instance.SpawnPlayerAtSceneStart();
        }
    }

    public void LoadGame()
    {
        // Placeholder: in future, hook into save system
        Debug.Log("Load Game clicked (not implemented yet).");
    }

    public void Options()
    {
        // Placeholder: later we can show options UI
        Debug.Log("Options clicked (not implemented yet).");
    }

    public void QuitGame()
    {
        Debug.Log("Quit clicked.");
        Application.Quit();

        // Note: Quit only works in builds, not in the Unity editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
