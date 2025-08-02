using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SecretExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO add actual logic when the player reaches the exit
        print("Exit found.");

        FunctionTimer.Create(() =>
        {
            GameSceneManager.NextScene();
        }, 1f);
    }
}
