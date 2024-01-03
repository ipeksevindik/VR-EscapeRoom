using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Vitals;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterData : MonoBehaviour
{
    public int Live = 2;
    public Health Health { get; private set; }
    public void Damage(float amount)
    {
        Health.Decrease(amount);
        HealthHud.text = Health.Value.ToString();
    }
    public void Heal(float amount) => Health.Increase(amount);

    public void StartHealthRegeneration() => Health.Regeneration.StartRegeneration();

    public void StopHealthRegeneration() => Health.Regeneration.StopRegeneration();

    public float sound = 0.3f;
    public AudioSource source;

    public AudioClip ready;
    public AudioClip begin;
    public AudioClip loser;
    public AudioClip final_round;
    public AudioClip lost;

    public GameObject pistol;

    public TextMeshProUGUI HealthHud;


    public void Awake()
    {
        Health = GetComponent<Health>();
        volume = GetComponentInChildren<Volume>();
        volume.profile.TryGet(out _vignette);

        source.PlayDelayed(4);
        source.PlayOneShot(ready, sound);
        
 

    }

    public void Update()
    {

    }

    public Transform SpawnPoint;

    Vignette _vignette;

    public Volume volume;

    public float size = 0;

    public void PlayerRespawn()
    {
        source.PlayOneShot(loser, sound);
        StartCoroutine(playSoundAfterFour());
        gameObject.transform.position = SpawnPoint.position;
        gameObject.transform.rotation = SpawnPoint.rotation;

    }
    
    IEnumerator playSoundAfterFour()
    {
        yield return new WaitForSeconds(2f);
        source.PlayOneShot(final_round, sound);

    }


    public IEnumerator RespwanVignette()
    {

        size = 0f;

        _vignette.active= true;
        _vignette.color.Override(Color.black);

        while(size <= 0)
        {
            size += 0.01f;
            if (size > 1) size = 1;

            _vignette.intensity.Override(size);

            yield return new WaitForSeconds(5f);

        }

        _vignette.color.Override(Color.red);
        _vignette.active = false;


    }

    public void PlayerDie()
    {
        Live--;
        if (Live > 0)
        {
            PlayerRespawn();
            StartCoroutine(RespwanVignette());
            Heal(100);
            pistol.active = true;
            

        }
        if(Live == 0)
        {
            Debug.Log("deneme");
            source.PlayDelayed(4);
            source.PlayOneShot(lost, sound);
            SceneTransitionManager.singleton.GoToScene(0);
           

        }

    }

}
