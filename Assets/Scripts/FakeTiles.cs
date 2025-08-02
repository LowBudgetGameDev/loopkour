using UnityEngine;

public class FakeTiles : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody2D;

    private float lastYVelocity;

    private void FixedUpdate()
    {
        lastYVelocity = playerRigidbody2D.linearVelocityY;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == playerRigidbody2D.transform)
        {
            if (Mathf.Abs(lastYVelocity) > 0.1f) Destroy(gameObject);
        }
    }
}
