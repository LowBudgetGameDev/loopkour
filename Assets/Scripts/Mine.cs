using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private Transform explodeParticles;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.Instance.PlaySoundType(SoundManager.SoundType.Explosion);
        Instantiate(explodeParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
