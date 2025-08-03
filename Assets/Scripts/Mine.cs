using UnityEngine;

public class Mine : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.Instance.PlaySoundType(SoundManager.SoundType.Explosion);
        Destroy(gameObject);
    }
}
