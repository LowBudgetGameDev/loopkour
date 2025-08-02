using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer = 64;

    [SerializeField] private BoxCollider2D laserCollider;
    [SerializeField] private SpriteRenderer laserSpriteRenderer;

    private void Awake()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, -transform.up, 1000f, groundLayer);

        laserSpriteRenderer.size = new Vector2(1f, raycastHit.distance);
        laserCollider.size = new Vector2(0.5f, raycastHit.distance);

        laserCollider.transform.position = transform.position + 0.5f * -transform.up * raycastHit.distance;
    }

    private void Update()
    {
        Vector2 boxSize = transform.eulerAngles.z == 0 ? new Vector2(0.75f, 0.05f) : new Vector2(0.05f, 0.75f); // There will only ever be 0 and 90 degrees so this is good enough

        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, boxSize, 0f, -transform.up, 1000f, groundLayer);

        laserSpriteRenderer.size = new Vector2(1f, raycastHit.distance - 0.06f);
        laserCollider.size = new Vector2(0.5f, raycastHit.distance - 0.06f);

        laserCollider.transform.position = transform.position + 0.5f * -transform.up * (raycastHit.distance - 0.06f);
    }
}
