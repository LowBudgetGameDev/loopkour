using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FakeSecretExit : MonoBehaviour
{
    [SerializeField] private int numLoopsNeeded = 3;
    [SerializeField] private LevelLoop levelLoop;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (levelLoop.GetNumberOfLoops() == numLoopsNeeded)
        {
            FunctionTimer.Create(() =>
            {
                GameSceneManager.NextScene();
            }, 1f);
        }
        else
        {
            levelLoop.Loop();
            print(numLoopsNeeded - levelLoop.GetNumberOfLoops());
        }
    }
}
