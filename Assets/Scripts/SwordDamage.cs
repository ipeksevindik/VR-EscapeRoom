using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SwordDamage : MonoBehaviour
{
    public GameObject Blood;
    public AudioSource attackSound;
    public AudioSource walkSound;
    public AudioSource runSound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Enemy"))
    //    {
    //        GameObject b = Instantiate(Blood);

    //        enemyHealth -= 50;

    //        b.transform.position = collision.GetContact(0).point;
    //        b.SetActive(true);
    //        Destroy(b, 1f);

    //        if (enemyHealth == 0)
    //        {
    //            agent.isStopped = true;
    //            attackSound.Stop();
    //            walkSound.Stop();
    //            enemyAnim.SetBool("IsFallingBack", true);
    //            Invoke("Destroy", 2f);
    //        }

    //    }
    //}
}
