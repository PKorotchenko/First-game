using UnityEngine;

public class AsteroidObstacle : ObstacleBase
{
    [Header("Asteroid")]
    public float rotationSpeed = 25f;

    protected override void Update()
    {
        base.Update();
        if (moving)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }
}
