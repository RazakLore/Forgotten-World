using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private string[] dialogueLines;
    private int currentIndex = 0;
    private bool inDialogue = false;

    public void BeginDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        inDialogue = true;
        currentIndex = 0;

        UIStateController.CurrentState = UIState.Dialogue;
        Debug.Log(dialogueLines[currentIndex]);
        //DialogueUI.Instance.Show(dialogueLines[currentIndex]);
    }

    private void Update()
    {
        if (!inDialogue) return;

        // Space or Enter moves dialogue
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            AdvanceDialogue();
        }
    }

    private void AdvanceDialogue()
    {
        currentIndex++;

        // No more dialogue ? exit
        if (currentIndex >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }
        Debug.Log(dialogueLines[currentIndex]);
        //DialogueUI.Instance.Show(dialogueLines[currentIndex]);
    }

    private void EndDialogue()
    {
        inDialogue = false;
        GetComponent<NPC>().TurnStaticOff();
        //DialogueUI.Instance.Hide();

        UIStateController.CurrentState = UIState.Gameplay;
    }
    // This class should hopefully be simple. We fill in the dialogueLines in the inspector for each instance of an NPC, so its unique.
    // Now, we need to switch to the Dialogue UI enum we set up earlier so the player and the NPC can't move.
    // After this, we need to enable the dialogue box in the UI. There will be a text field there that we will change to be the current dialogueLines item.
    // On Space or Enter pressed, we go from dialogueLines item 0 to item 1. And then repeat until we reach the final dialogue. When there is no next item, we reset to item 0 and exit out this dialogue instance.
}
