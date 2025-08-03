using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private Transform ragdollTransform;

    private void OnDestroy()
    {
        if (!Application.isPlaying || !gameObject.scene.isLoaded) return;

        Transform ragdoll = Instantiate(ragdollTransform, transform.position, Quaternion.identity);

        ragdoll.GetComponent<Ragdoll>().ApplyForce(new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized * 200f); // Multiply by large number to be noticable

        FunctionTimer.Create(() =>
        {
            GameSceneManager.ReloadScene();
        }, 3f);

        SoundManager.Instance.PlaySoundType(SoundManager.SoundType.Death);
        CinemachineShake.Instance.ShakeCamera(2f, 0.25f);
    }
}
