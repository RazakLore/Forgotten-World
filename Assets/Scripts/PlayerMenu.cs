using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    public static PlayerMenu instance;

    [Header("UI References")]
    public GameObject menuPanel;   // assign MenuPanel
    //public RectTransform arrow;    // assign Arrow Image RectTransform
    public Button[] options; // assign ItemsText, SpellsText, MiscText

    // Neighbour table
    private int[] upMap = { 0, 1, 0, 1, 2, 3 };
    private int[] downMap = { 2, 3, 4, 5, 0, 1 };
    private int[] leftMap = { 0, 0, 2, 2, 4, 4 };
    private int[] rightMap = { 1, 1, 3, 3, 5, 5 };

    [SerializeField] private int currentSelection = 0;
    [SerializeField] private PlayerMovement playerMoveReference;
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
    private void Start()
    {
        playerMoveReference = PlayerMovement.instance;
    }

    private void OnEnable()
    {
        playerMoveReference = PlayerMovement.instance;
        playerMoveReference.SetCanMove(false);
    }

    private void OnDisable()
    {
        playerMoveReference.SetCanMove(true);
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
            currentSelection = upMap[currentSelection];/*(currentSelection - 1 + options.Length) % options.Length;*/
            UpdateArrow();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentSelection = downMap[currentSelection];
            UpdateArrow();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentSelection = leftMap[currentSelection];
            UpdateArrow();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSelection = rightMap[currentSelection];
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
        options[currentSelection].Select();
        //for (int i = 0; i < options.Length; i++)
            //options[i].GetComponent<Image>().color = Color.black;

        //options[currentSelection].GetComponent<Image>().color = new Color(1, 1, 0.6f);
        // Move arrow next to the currently selected option
        //arrow.position = new Vector3(
        //    arrow.position.x,
        //    options[currentSelection].transform.position.y,
        //    arrow.position.z
        //);
    }

    private void ConfirmSelection()
    {
        string selected = options[currentSelection].GetComponentInChildren<TextMeshProUGUI>().text;
        Debug.Log(options[currentSelection].GetComponentInChildren<TextMeshProUGUI>().text);

        switch (selected)
        {
            case "Item":
                // Call OpenInventory()
                break;
            case "Spell":

                break;
            case "Equip":

                break;
            case "Stats":

                break;
            case "Records":

                break;
            case "Misc": // Just quick save instead?

                break;
        }
    }
}
