using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    Rigidbody2D rb;
    Vector2 moveInput;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }


    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY += 1f;
        if (Input.GetKey(KeyCode.S)) moveY -= 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;

        moveInput = new Vector2(moveX, moveY);
        if (moveInput.magnitude > 1f)
            moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}