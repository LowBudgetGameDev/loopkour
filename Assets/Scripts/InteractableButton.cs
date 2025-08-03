using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToOpen;

    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isPressed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ButtonPressable" && !isPressed)
        {
            Destroy(objectToOpen);
            isPressed = true;
            spriteRenderer.sprite = pressedSprite;
            SoundManager.Instance.PlaySound(SoundManager.Sound.GateOpen);
        }
    }
}
