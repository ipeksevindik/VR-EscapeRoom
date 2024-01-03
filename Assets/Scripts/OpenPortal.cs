using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;


public class OpenPortal : MonoBehaviour
{

    public AudioSource source;
    public AudioClip youWin;

    void Start()
    {
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            SceneTransitionManager.singleton.GoToScene(0);
            StartCoroutine(WinSound());
        }
    }

    public IEnumerator WinSound()
    {
        yield return new WaitForSeconds(3);
        source.PlayOneShot(youWin);

    }


}
