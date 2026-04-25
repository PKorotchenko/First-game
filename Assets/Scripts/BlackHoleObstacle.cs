using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BlackHoleObstacle : ObstacleBase
{
    [Header("Attraction")]
    public float pullStrength = 20f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.TryGetComponent<Rigidbody2D>(out Rigidbody2D body))
        {
            Vector2 direction = ((Vector2)transform.position - body.position).normalized;
            body.AddForce(direction * pullStrength * Time.deltaTime, ForceMode2D.Force);
        }
    }
}
