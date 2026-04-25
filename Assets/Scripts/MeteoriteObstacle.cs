using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeteoriteObstacle : ObstacleBase
{
    [Header("Dense Zone")]
    public float densityForce = 8f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.TryGetComponent<Rigidbody2D>(out Rigidbody2D body))
        {
            body.AddForce(Vector2.down * densityForce * Time.deltaTime, ForceMode2D.Force);
        }
    }
}
