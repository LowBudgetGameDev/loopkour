using UnityEngine;

public class FakeTiles : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody2D;

    private float lastYVelocity;

    private void FixedUpdate()
    {
        if (playerRigidbody2D == null) return;

        lastYVelocity = playerRigidbody2D.linearVelocityY;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerRigidbody2D == null) return;

        if (collision.transform == playerRigidbody2D.transform)
        {
            if (Mathf.Abs(lastYVelocity) > 0.1f) Destroy(gameObject);
        }
    }
}
