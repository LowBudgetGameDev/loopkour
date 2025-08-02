using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Rigidbody2D torsoRigidbody2D;

    public void ApplyForce(Vector2 force)
    {
        torsoRigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }
}
