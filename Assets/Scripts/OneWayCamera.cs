using UnityEngine;

public class OneWayCamera : MonoBehaviour
{
    private float resetTimer;

    private float maxX;

    private void Start()
    {
        maxX = transform.position.x;
    }

    private void LateUpdate()
    {
        if (resetTimer > 0f)
        {
            resetTimer -= Time.deltaTime;
            return;
        }

        float currentX = transform.position.x;

        if (currentX > maxX)
        {
            maxX = currentX;
        }

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Max(transform.position.x, maxX);
        transform.position = clampedPos;
    }

    public void ResetCamera(Vector3 position)
    {
        maxX = position.x;
        transform.position = position;
        resetTimer = 0.5f;
    }
}
