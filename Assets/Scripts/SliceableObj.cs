using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceableObj : MonoBehaviour
{
    SliceEnemy slice;
    void Start()
    {
        slice = FindObjectOfType<SliceEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Sword"))
            slice.CallSlice();
    }
    

}
