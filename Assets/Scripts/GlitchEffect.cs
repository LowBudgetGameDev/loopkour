using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlitchEffect : MonoBehaviour
{
    public static GlitchEffect Instance { get; private set; }

    private Volume volume;

    private ChromaticAberration abberation;
    private Vignette vignette;

    private float glitchTimer;
    private float glitchTimerTotal;

    private float startingAbberation = 1f;
    private float startingVignette = 0.45f;

    private void Awake()
    {
        Instance = this;

        volume = GetComponent<Volume>();

        volume.profile.TryGet(out ChromaticAberration chromaticAberration);

        abberation = chromaticAberration;

        volume.profile.TryGet(out Vignette vignette);

        this.vignette = vignette;
    }

    public void GlitchScreen(float time)
    {
        abberation.intensity.value = startingAbberation;
        vignette.intensity.value = startingVignette;

        glitchTimer = time;
        glitchTimerTotal = time;
    }

    private void Update()
    {
        if (glitchTimer > 0f)
        {
            glitchTimer -= Time.deltaTime;

            abberation.intensity.value = Mathf.Lerp(startingAbberation, 0f, 1 - (glitchTimer / glitchTimerTotal));
            vignette.intensity.value = Mathf.Lerp(startingVignette, 0f, 1 - (glitchTimer / glitchTimerTotal));
        }
    }
}
