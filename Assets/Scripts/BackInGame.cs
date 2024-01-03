using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BackInGame : MonoBehaviour
{

    public GameObject newPosition;
    public GameObject Player;
    
    public GameObject portal;

    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(OnPush);

    }

    private void OnPush(SelectEnterEventArgs arg0)                                                                        
    {
        int num = UnityEngine.Random.Range(1, 3);

        switch(num)
        {
            case 1:
                BackToTheGame();
                break;
            case 2:
                TogglePortalOpen();
                break;
        }
       
    }

    void Update()
    {
        
    }

    public void BackToTheGame()
    {
        Player.transform.position = newPosition.transform.position;
        
    }

    public void TogglePortalOpen()
    {
        portal.SetActive(true);
    }
}
