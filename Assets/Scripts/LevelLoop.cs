using UnityEngine;

public class LevelLoop : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private OneWayCamera playerCamera;

    [Space]

    [SerializeField] private Transform levelStart;
    [SerializeField] private Transform levelEnd;

    private void Update()
    {
        if (playerTransform.position.x > levelEnd.position.x)
        {
            playerTransform.position = levelStart.position;
            playerCamera.ResetCamera(levelStart.position);
        }
    }
}
