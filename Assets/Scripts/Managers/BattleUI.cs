using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance;

    [Header("Panels")]
    [SerializeField] private GameObject battlePanel;

    [Header("Player Stats UI")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;

    [Header("Action Buttons")]
    [SerializeField] private GameObject fightButton;
    [SerializeField] private GameObject fleeButton;
    [SerializeField] private GameObject miscButton;

    private int selectedIndex = 0;
    private GameObject[] buttons;
    public System.Action<int> OnActionChosen;
    public BattleMessageLog messageLog;

    public bool waitingForPlayerInput = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        buttons = new GameObject[] { fightButton, fleeButton, miscButton };

        HighlightButton(0);
        battlePanel.SetActive(false);
    }

    // -------------------------
    // PUBLIC METHODS
    // -------------------------

    public void Show()
    {
        battlePanel.SetActive(true);
        UpdateStats();
    }

    public void Hide()
    {
        battlePanel.SetActive(false);
    }

    public void UpdateStats()
    {
        hpText.text = PlayerState.instance.HP.ToString();
        mpText.text = PlayerState.instance.MP.ToString();

    }

    // Reset the selection to first action button, whenever player turn starts
    public void ResetSelectionToFirst()
    {
        selectedIndex = 0;
        HighlightButton(selectedIndex);
    }

    // Called every frame *during the player's turn only*
    public int HandleMenuInput()
    {
        waitingForPlayerInput = true;

        if (Input.GetKeyDown(KeyCode.W))
            MoveSelection(-1);

        if (Input.GetKeyDown(KeyCode.S))
            MoveSelection(+1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            waitingForPlayerInput = false;
            ConfirmSelection();
            return selectedIndex; // 0 = Fight, 1 = Flee
        }

        return -1;  // no selection yet
    }

    public void ConfirmSelection()
    {
        OnActionChosen?.Invoke(selectedIndex);
    }

    // -------------------------
    // VISUAL HIGHLIGHTING
    // -------------------------

    private void MoveSelection(int dir)
    {
        selectedIndex = Mathf.Clamp(selectedIndex + dir, 0, buttons.Length - 1);
        HighlightButton(selectedIndex);
    }

    private void HighlightButton(int index)
    {
        // Example simple highlight:
        for (int i = 0; i < buttons.Length; i++)
        {
            var text = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            text.color = (i == index ? Color.yellow : Color.white);
        }
    }
}
