using UnityEngine;

public class StationObstacle : ObstacleBase
{
    [Header("Pathing")]
    public float bobAmplitude = 1.2f;
    public float bobSpeed = 1.1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        if (!moving)
            return;

        float offset = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + offset, transform.position.z);
    }
}
