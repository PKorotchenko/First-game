using UnityEngine;

public abstract class ObstacleBase : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroyX = -14f;
    protected bool moving = true;

    protected virtual void Update()
    {
        if (!moving)
            return;

        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void Pause()
    {
        moving = false;
    }

    public void Resume()
    {
        moving = true;
    }
}
