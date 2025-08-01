using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToOpen;
    [SerializeField] private LayerMask buttonPressable;

    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & buttonPressable) != 0)
        {
            Destroy(objectToOpen);
            spriteRenderer.sprite = pressedSprite;
        }
    }
}
