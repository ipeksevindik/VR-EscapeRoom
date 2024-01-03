using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashlightOnActivate : MonoBehaviour
{
    private Light spotlight;

    void Start()
    {

        spotlight = GetComponent<Light>();
        spotlight.enabled = false;
    }



    void Update()
    {

    }


    public void lightOnOff()
    {
        ChangeLightStatus(!spotlight.enabled);

    }

    public void ChangeLightStatus(bool pShouldActive)
    {
        spotlight.enabled= pShouldActive;
    }
}
