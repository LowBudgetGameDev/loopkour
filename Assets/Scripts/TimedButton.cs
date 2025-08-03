using UnityEngine;

public class TimedButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToOpen;
    [SerializeField] private float timeTillClose = 20f;

    [SerializeField] private Sprite unpressedSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isPressed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ButtonPressable" && !isPressed)
        {
            objectToOpen.SetActive(false);
            isPressed = true;
            spriteRenderer.sprite = pressedSprite;
            SoundManager.Instance.PlaySound(SoundManager.Sound.GateOpen);

            FunctionTimer.Create(() =>
            {
                objectToOpen.SetActive(true);
                isPressed = false;
                spriteRenderer.sprite = unpressedSprite;
                SoundManager.Instance.PlaySound(SoundManager.Sound.GateClose);

            }, timeTillClose);
        }
    }
}
