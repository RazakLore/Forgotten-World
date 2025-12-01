using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public GameObject menuCanvas;
    [SerializeField] private GameObject pauseMenuPanel;
    private bool pauseMenuPanelActive = false;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMPro.TMP_Text dialogueText;

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

        //UIStateController.CurrentState = opening ? UIState.Paused : UIState.Gameplay;   // Doesnt really do anything
    }

    public void ShowDialogue(string text)
    {
        dialoguePanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        dialogueText.text = text;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        dialogueText.text = "";
    }
}
