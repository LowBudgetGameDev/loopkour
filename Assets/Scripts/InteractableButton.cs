using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToOpen;

    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ButtonPressable")
        {
            Destroy(objectToOpen);
            spriteRenderer.sprite = pressedSprite;
        }
    }
}
