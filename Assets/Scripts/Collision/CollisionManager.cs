using System.Collections;
using System.Collections.Generic;
using SpatialPartition;
using UnityEngine;

public enum ESpatialPartitionOption
{
    None = 0,
    GRID = 1,
    COUNT = 2
    // TODO : put your own choice
};

public class CollisionManager : MonoBehaviour
{
    private ESpatialPartitionOption m_option;
    private uint m_collisionCount = 0u;

    private SPGridGeneric<Sphere> _grid;

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
        if (m_option == ESpatialPartitionOption.GRID)
            _grid.DrawGizmos();
    }

    private void Start()
    {
        _grid = new(2, 2);
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
        else if (m_option == ESpatialPartitionOption.GRID)
        {
            // TODO : re-create your spatial partition
            // TODO : query it

            _grid.UpdateBoxes(spheres, s => s.transform.position);

            foreach (var sphereA in spheres)
            {
                var neighbors = _grid.GetNeighbors(sphereA.position);
                foreach (var neighborSphere in neighbors)
                {
                    m_collisionCount++;
                    CollisionDetection.ApplyCollisionResolution(sphereA, neighborSphere);
                }
            }
        }
    }
}
