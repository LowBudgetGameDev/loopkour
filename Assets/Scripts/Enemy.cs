using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private LayerMask groundLayer = 64;

    private new Rigidbody2D rigidbody2D;

    private new Collider2D collider2D;

    private float floorCheckDist = 0.02f;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

        rigidbody2D.linearVelocityX = movementSpeed;
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        Vector2 leftPos = new Vector2(collider2D.bounds.min.x, collider2D.bounds.min.y);
        Vector2 rightPos = new Vector2(collider2D.bounds.max.x, collider2D.bounds.min.y);

        bool groundOnLeft = Physics2D.Raycast(leftPos, Vector2.down, floorCheckDist, groundLayer);
        bool groundOnRight = Physics2D.Raycast(rightPos, Vector2.down, floorCheckDist, groundLayer);

        if (!groundOnLeft)
        {
            rigidbody2D.linearVelocityX = movementSpeed;
        }
        else if (!groundOnRight)
        {
            rigidbody2D.linearVelocityX = -movementSpeed;
        }

        Vector2 leftSidePos = new Vector2(collider2D.bounds.min.x, collider2D.bounds.center.y);
        Vector2 rightSidePos = new Vector2(collider2D.bounds.max.x, collider2D.bounds.center.y);

        bool wallOnLeft = Physics2D.Raycast(leftSidePos, Vector2.left, floorCheckDist, groundLayer);
        bool wallOnRight = Physics2D.Raycast(rightSidePos, Vector2.right, floorCheckDist, groundLayer);

        if (wallOnLeft)
        {
            rigidbody2D.linearVelocityX = movementSpeed;
        }
        else if (wallOnRight)
        {
            rigidbody2D.linearVelocityX = -movementSpeed;
        }

        float rotationAngle = rigidbody2D.linearVelocityX > 0 ? 0 : 180;
        transform.eulerAngles = new Vector3(0f, rotationAngle, 0f);
    }
}
