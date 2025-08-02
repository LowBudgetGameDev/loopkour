using UnityEngine;

public class LevelLoop : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private OneWayCamera playerCamera;

    [Space]

    [SerializeField] private Transform levelStart;
    [SerializeField] private Transform levelEnd;

    private int numLoops;

    private void Update()
    {
        if (playerTransform == null) return;

        if (playerTransform.position.x > levelEnd.position.x)
        {
            Loop();
            numLoops++;
        }
    }

    public int GetNumberOfLoops()
    {
        return numLoops;
    }

    public void Loop()
    {
        playerTransform.position = levelStart.position;
        playerCamera.ResetCamera(levelStart.position);
    }
}
