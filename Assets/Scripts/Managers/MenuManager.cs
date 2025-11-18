using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public GameObject menuCanvas;
    [SerializeField] private GameObject pauseMenuPanel;
    private bool pauseMenuPanelActive = false;

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

    private void Update()
    {
        if (!UIStateController.IsGameplay)
            return;

        if (Input.GetKeyUp(KeyCode.Escape))
            TogglePause();
    }

    private void TogglePause()
    {
        bool opening = !pauseMenuPanelActive;

        pauseMenuPanel.SetActive(opening);
        pauseMenuPanelActive = opening;

        UIStateController.CurrentState = opening ? UIState.Paused : UIState.Gameplay;
    }
}
