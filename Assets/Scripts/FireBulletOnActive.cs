using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class FireBulletOnActive : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float FireSpeed = 30;
    private EnemyAI Enemy;

    public ParticleSystem Blood;
   
    public AudioClip shoot;
    public AudioSource source;


    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
        EnemyAI Enemy = new EnemyAI();
        source = GetComponent<AudioSource>();
        Blood = GetComponentInChildren<ParticleSystem>();

    }

    void Update()
    {
        
    }

    public void FireBullet(ActivateEventArgs args)
    {
        GameObject spawnedBullet = Instantiate(bullet);
        spawnedBullet.transform.position = spawnPoint.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward* FireSpeed;
        source.PlayOneShot(shoot);

        Destroy(spawnedBullet, 6);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            ParticleSystem b = Instantiate(Blood);
            Enemy = collision.transform.GetComponent<EnemyAI>();
            b.transform.position = collision.GetContact(0).point;
            b.Play(true);
            Destroy(b, 1f);

            float result = Enemy.EnemyDamage();
            
            if(result <=0)
            {
                Debug.Log(Enemy.name);
                Enemy.AttackerCurrentState = EnemyAI.AttackerStates.Die;
            }

           
        }
    }


}
