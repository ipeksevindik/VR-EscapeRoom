using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SliceEnemy : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public Material crossSectionMaterial;
    public float cutForce = 2000;
    public LayerMask sliceableLayer;
    public ParticleSystem Blood;
    EnemyAI Enemy;

    void Start()
    {
        Enemy = FindObjectOfType<EnemyAI>();

    }

    void FixedUpdate()
    {
        
    }

    public void CallSlice()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            Debug.Log("deniyom");
            ParticleSystem b = Instantiate(Blood);
            GameObject target = hit.transform.gameObject;
            Slice(target);

        }
    }
  

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if(hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            SetupSlicedComponent(upperHull);

            GameObject loverHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedComponent(loverHull);


            //Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }


    
}
