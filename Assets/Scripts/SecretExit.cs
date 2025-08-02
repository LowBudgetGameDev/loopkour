using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SecretExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FunctionTimer.Create(() =>
        {
            GameSceneManager.NextScene();
        }, 1f);
    }
}
