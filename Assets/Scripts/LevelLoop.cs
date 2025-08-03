using UnityEngine;

public class LevelLoop : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private OneWayCamera playerCamera;

    [Space]

    [SerializeField] private Transform levelStart;
    [SerializeField] private Transform levelEnd;

    private int numLoops = 0;

    private void Update()
    {
        if (playerTransform == null) return;

        if (playerTransform.position.x > levelEnd.position.x)
        {
            Loop(false);
            numLoops++;
            SoundManager.Instance.PlaySound(SoundManager.Sound.Glitch);
        }
    }

    public int GetNumberOfLoops()
    {
        return numLoops;
    }

    public void Loop(bool isError = true)
    {
        playerTransform.position = levelStart.position;
        playerCamera.ResetCamera(levelStart.position);
        GlitchEffect.Instance.GlitchScreen(1f);
        if (isError) SoundManager.Instance.PlaySound(SoundManager.Sound.Error);
    }
}
