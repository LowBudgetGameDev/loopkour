using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
