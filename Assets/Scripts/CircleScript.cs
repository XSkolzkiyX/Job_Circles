using UnityEngine;

public class CircleScript : MonoBehaviour
{
    public float speed;

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
        if (transform.position.y < -10f) Destroy(gameObject);
    }
}