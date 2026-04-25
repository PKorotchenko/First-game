using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float thrustForce = 8f;
    public float maxFallSpeed = -12f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool controlsEnabled = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if (!controlsEnabled)
            return;

        if (IsTapInput())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    private bool IsTapInput()
    {
        if (Input.GetMouseButtonDown(0))
            return true;

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                    return true;
            }
        }

        return false;
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * thrustForce, ForceMode2D.Impulse);
    }

    public void ResetState()
    {
        transform.position = startPosition;
        rb.velocity = Vector2.zero;
        controlsEnabled = false;
    }

    public void EnableControls(bool enabled)
    {
        controlsEnabled = enabled;
        if (!enabled)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance?.GameOver();
        }
    }
}
