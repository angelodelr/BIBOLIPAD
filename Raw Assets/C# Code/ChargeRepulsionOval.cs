using UnityEngine;

public class ChargeRepulsionOval : MonoBehaviour
{
    public float charge = 10f;
    public Transform parkBox;
    public float xRadius = 5f;
    public float yRadius = 3f;
    public float stopThreshold = 0.01f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = 1f;
    }

    void FixedUpdate()
    {
        if (parkBox == null) return; 

        ChargeRepulsionOval[] others = FindObjectsOfType<ChargeRepulsionOval>();
        Vector2 force = Vector2.zero;

        foreach (var other in others)
        {
            if (other == this) continue;
            Vector2 dir = rb.position - other.rb.position;
            float dist = dir.magnitude;
            if (dist > 0.01f)
                force += dir.normalized * (charge / (dist * dist));
        }

        rb.AddForce(force);

        if (rb.linearVelocity.magnitude > 0 && Vector2.Dot(rb.linearVelocity, rb.linearVelocity + force * Time.fixedDeltaTime) < rb.linearVelocity.sqrMagnitude)
        {
            rb.linearVelocity = Vector2.zero;
            Destroy(this); 
        }

        Vector2 local = rb.position - (Vector2)parkBox.position;
        if ((local.x * local.x) / (xRadius * xRadius) + (local.y * local.y) / (yRadius * yRadius) > 1f)
        {
            Vector2 clamped = local.normalized;
            clamped.x *= Mathf.Min(Mathf.Abs(clamped.x * xRadius), xRadius);
            clamped.y *= Mathf.Min(Mathf.Abs(clamped.y * yRadius), yRadius);
            rb.position = (Vector2)parkBox.position + clamped;
        }
    }
}
