using UnityEngine;

public class TimedButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToOpen;
    [SerializeField] private float timeTillClose = 20f;

    [SerializeField] private Sprite unpressedSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ButtonPressable")
        {
            objectToOpen.SetActive(false);
            spriteRenderer.sprite = pressedSprite;
            SoundManager.Instance.PlaySound(SoundManager.Sound.GateOpen);

            FunctionTimer.Create(() =>
            {
                objectToOpen.SetActive(true);
                spriteRenderer.sprite = unpressedSprite;
                SoundManager.Instance.PlaySound(SoundManager.Sound.GateClose);

            }, timeTillClose);
        }
    }
}
