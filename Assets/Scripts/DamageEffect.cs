using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class DamageEffect : MonoBehaviour
{
    public float intensity = 0;

    Volume volume;
    Vignette vignette;

    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);

        if (!vignette)
        {
            print("error, vignette empty");
        }
        else
        {
            vignette.active = false;
        }

    }

    void Update()
    {
        
    }

    public IEnumerator TakeDamageEffect()
    {
        intensity = 0.8f;

        vignette.active = true;
        vignette.intensity.Override(0.4f);

       // yield return new WaitForSeconds(0.4f);

        while (intensity > 0)
        {
            intensity -= 0.01f;
            if (intensity < 0) intensity = 0;

            vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(0.1f);
        }

        vignette.active = false;
        yield break;

    }
}
