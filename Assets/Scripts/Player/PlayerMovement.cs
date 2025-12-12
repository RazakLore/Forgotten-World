using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    [SerializeField] private float moveSpeed = 1.3f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private RandomEncounter randomEncounter;
    private Animator anim;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
        randomEncounter = GetComponent<RandomEncounter>();

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
  
    void Update()
    {
        if (!canMove)
        {
            movement = Vector2.zero;
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        anim.SetFloat("MoveX", movement.x);
        anim.SetFloat("MoveY", movement.y);

        if (randomEncounter.HasEncounterTable)
            randomEncounter.HandleEncounterTimer(movement.x, movement.y);
    }

    private void FixedUpdate()
    {
        if (canMove)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}