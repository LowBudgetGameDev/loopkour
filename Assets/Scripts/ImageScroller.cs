using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeedX;

    private RawImage image;

    private void Awake()
    {
        image = GetComponent<RawImage>();
    }

    private void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(scrollSpeedX, 0f) * Time.deltaTime, image.uvRect.size);
    }
}
