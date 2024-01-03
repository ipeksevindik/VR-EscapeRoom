using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerTeleport : MonoBehaviour
{
    CharacterData chrData;

    void Start()
    {
        chrData = GetComponent<CharacterData>();
    }

    void Update()
    {
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Teleportation"))
        {
            Debug.Log("Transportation Area");
            other.transform.position = new Vector3(0, 1, -11);
        }

        else if (other.CompareTag("Healing"))
        {
            Debug.Log("Healing Area");
            chrData.StartHealthRegeneration();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Healing"))
        {
            Debug.Log("Out");
            chrData.StopHealthRegeneration();
        }
    }
}
