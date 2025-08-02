using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private BoxCollider2D laserCollider;
    [SerializeField] private SpriteRenderer laserSpriteRenderer;

    private void Awake()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, -transform.up, 1000f);

        laserSpriteRenderer.size = new Vector2(1f, raycastHit.distance);
        laserCollider.size = new Vector2(0.5f, raycastHit.distance);

        laserCollider.transform.position = transform.position + 0.5f * -transform.up * raycastHit.distance;
    }
}
