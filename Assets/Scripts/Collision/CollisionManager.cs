using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESpatialPartitionOption
{
    None = 0,
    COUNT = 1
    // TODO : put your own choice
};

public class CollisionManager : MonoBehaviour
{
    private ESpatialPartitionOption m_option;
    private uint m_collisionCount = 0u;

    public void CycleSpacePartition()
    {
        m_option = (ESpatialPartitionOption) (((uint) m_option + 1u) % (uint) ESpatialPartitionOption.COUNT);
    }

    private void OnGUI()
    {
        GUILayout.Label("Collisions test : " + m_collisionCount);
    }

    private void OnDrawGizmos()
    {
        // TODO : debug draw if you want
    }

    private void Start()
    {
        // TODO : you can initialize here
    }

    private void FixedUpdate()
    {
        m_collisionCount = 0u;

        Sphere[] spheres = FindObjectsOfType<Sphere>();
        PlaneCollider[] colliders = FindObjectsOfType<PlaneCollider>();

        for(int i = 0; i < spheres.Length; i++)
        {
            Sphere sphereA = spheres[i];
            foreach(PlaneCollider planeCollider in colliders)
            {
                CollisionDetection.ApplyCollisionResolution(sphereA, planeCollider);
            }
        }

        if (m_option == ESpatialPartitionOption.None)
        {
            for(int i = 0; i < spheres.Length; i++)
            {
                Sphere sphereA = spheres[i];
                for(int j = i+1; j < spheres.Length; j++)
                {
                    Sphere sphereB = spheres[j];
                    m_collisionCount++;
                    CollisionDetection.ApplyCollisionResolution(sphereA, sphereB);
                }
            }
        }
        //else if (m_option == ESpatialPartitionOption.SOMETHING)
        {
            // TODO : re-create your spatial partition
            // TODO : query it
        }
    }
}
