using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FakeSecretExit : MonoBehaviour
{
    [SerializeField] private int numLoopsNeeded = 3;
    [SerializeField] private LevelLoop levelLoop;
    [SerializeField] private TextMeshPro numLoopsText;
    [SerializeField] private Transform particles;

    private float updateTimer;
    private float updateTimerMax = 0.2f;

    private void Awake()
    {
        Instantiate(particles, transform);
    }

    private void Update()
    {
        updateTimer -= Time.deltaTime;

        if (updateTimer < 0f)
        {
            UpdateText();
            updateTimer += updateTimerMax;
        }
    }

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

    private void UpdateText()
    {
        numLoopsText.SetText((numLoopsNeeded - levelLoop.GetNumberOfLoops()).ToString());
    }
}
