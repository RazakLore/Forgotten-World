using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Data")]
    [SerializeField] protected string npcName;
    [SerializeField] protected int moveSpeed;
    [SerializeField] protected bool isStaticNPC;

    [Header("Interaction")]
    [SerializeField] private GameObject exclamationMark;

    private bool isMoving = false;
    private Vector2 targetPos;
    private Direction currentDirection = Direction.Down;

    // Set the NPC name in editor for each prefab
    // Give them a move speed for any unique npc (like children move faster. elderly move slower)
    // Is the NPC a shopkeeper or king? If so, they are static and do not move
    // Change direction sprite based on enum direction, left right up down
    // Interaction method that accesses the root objects Dialogue script? When collision stay with player interaction box, enable exclamation point above head, when space or enter pressed run dialogue

    private void Update()
    {//We need a timer, a random number generator for the enum value, the animation, the agent.setdestination 16 pixels away
        if (!isStaticNPC && !isMoving)
        {
            StartCoroutine(Wander());
        }

        if (exclamationMark.activeSelf && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            StartDialogue();
        }
        else
            return;
    }

    private IEnumerator Wander()
    {
        isMoving = true;

        Direction dir = GetRandomDirection();
        currentDirection = dir;
        Vector2 dirVector = DirectionToVector(dir);

        Vector3 newPos = transform.position + (Vector3)dirVector * 1;

        // Check collision before moving
        if (!Physics2D.OverlapCircle(newPos, 0.2f))
        {
            yield return MoveTo(newPos);
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }

        isMoving = false;
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = transform.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        yield return new WaitForSeconds(Random.Range(0.3f, 1f));
    }

    private Direction GetRandomDirection()
    {
        int r = Random.Range(0, 4);

        return r switch
        {
            0 => Direction.Up,
            1 => Direction.Down,
            2 => Direction.Left,
            _ => Direction.Right
        };
    }

    private Vector2 DirectionToVector(Direction d)
    {
        return d switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.zero
        };
    }

    // ----------- Interaction System -----------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerInteraction"))
        {
            //playerInFront = true;
            //waitingForInput = true;
            exclamationMark.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerInteraction"))
        {
            //playerInFront = false;
            //waitingForInput = false;
            exclamationMark.SetActive(false);
        }
    }

    private void StartDialogue()
    {
        //waitingForInput = false;
        exclamationMark.SetActive(false);
        GetComponent<Dialogue>().BeginDialogue();
        isStaticNPC = true;
        //DialogueSystem.Instance.StartDialogue(npcName);
    }

    public void TurnStaticOff() // Make it one of those ? toggles
    {
        isStaticNPC = false;
    }


    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
