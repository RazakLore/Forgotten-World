using UnityEngine;
using TMPro;

public class PlayerMenu : MonoBehaviour
{
    public static PlayerMenu instance;

    [Header("UI References")]
    public GameObject menuPanel;   // assign MenuPanel
    public RectTransform arrow;    // assign Arrow Image RectTransform
    public TextMeshProUGUI[] options; // assign ItemsText, SpellsText, MiscText

    private int currentSelection = 0;
    private bool isOpen = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            menuPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }

        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentSelection = (currentSelection - 1 + options.Length) % options.Length;
            UpdateArrow();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentSelection = (currentSelection + 1) % options.Length;
            UpdateArrow();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection();
        }
    }

    private void ToggleMenu()
    {
        isOpen = !isOpen;
        menuPanel.SetActive(isOpen);

        if (isOpen)
        {
            currentSelection = 0;
            UpdateArrow();
        }
    }

    private void UpdateArrow()
    {
        // Move arrow next to the currently selected option
        arrow.position = new Vector3(
            arrow.position.x,
            options[currentSelection].transform.position.y,
            arrow.position.z
        );
    }

    private void ConfirmSelection()
    {
        string selected = options[currentSelection].text;
        Debug.Log($"Selected {selected} from menu");
        // Here you can open a detailed tab or future inventory/spells/etc.
    }
}
