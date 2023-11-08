using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionDetection
{
    public static void GetNormalAndPenetration(Sphere s1, Sphere s2, out Vector3 normal, out float penetration)
    {
        Vector3 d = (s1.Center - s2.Center);
        normal = d.normalized;
        penetration = s1.Radius + s2.Radius - d.magnitude;
    }

    public static void GetNormalAndPenetration(Sphere s, PlaneCollider p, out Vector3 normal, out float penetration)
    {
        normal = p.Normal;

        float offset = p.Offset;
        float distance = Vector3.Dot(s.Center, normal);
        penetration = s.Radius + offset - distance;
    }

    public static void ApplyCollisionResolution(Sphere s1, Sphere s2)
    {
        float totalInvMass = s1.invMass + s2.invMass;
        if (totalInvMass == 0)
        {
            return;
        }

        float invTotalInvMass = 1.0f / totalInvMass;

        Vector3 normal;
        float penetration;
        GetNormalAndPenetration(s1, s2, out normal, out penetration);

        if (penetration < 0)
        {
            return;
        }

        float deltaPos = penetration;
        float deltaPosA = deltaPos * s1.invMass * invTotalInvMass;
        float deltaPosB = deltaPos * s2.invMass * invTotalInvMass;
        s1.position += deltaPosA * normal;
        s2.position -= deltaPosB * normal;

        Vector3 relativeVelocity = s1.velocity - s2.velocity;
        float separatingVelocity = Vector3.Dot(relativeVelocity, normal);

        if (separatingVelocity < 0.0f)
        {
            const float restitution = 1.0f;
            float newSeparatingVel = -separatingVelocity * restitution;
            float deltaVelocity = newSeparatingVel - separatingVelocity;
            float deltaVelA = deltaVelocity * s1.invMass * invTotalInvMass;
            float deltaVelB = deltaVelocity * s2.invMass * invTotalInvMass;
            s1.velocity += deltaVelA * normal;
            s2.velocity -= deltaVelB * normal;
        }
    }

    public static void ApplyCollisionResolution(Sphere s, PlaneCollider p)
    {
        float totalInvMass = s.invMass + p.invMass;
        if (totalInvMass == 0)
        {
            return;
        }

        float invTotalInvMass = 1.0f / totalInvMass;

        Vector3 normal;
        float penetration;
        GetNormalAndPenetration(s, p, out normal, out penetration);

        if (penetration < 0)
        {
            return;
        }

        float deltaPos = penetration;
        float deltaPosA = deltaPos * s.invMass * invTotalInvMass;
        float deltaPosB = deltaPos * p.invMass * invTotalInvMass;
        s.position += deltaPosA * normal;
        p.position -= deltaPosB * normal;

        Vector3 relativeVelocity = s.velocity - p.velocity;
        float separatingVelocity = Vector3.Dot(relativeVelocity, normal);
        if (separatingVelocity < 0.0f)
        {
            const float restitution = 1.0f;
            float newSeparatingVel = -separatingVelocity * restitution;
            float deltaVelocity = newSeparatingVel - separatingVelocity;

            float deltaVelA = deltaVelocity * s.invMass * invTotalInvMass;
            float deltaVelB = deltaVelocity * p.invMass * invTotalInvMass;
            s.velocity += deltaVelA * normal;
            p.velocity -= deltaVelB * normal;
        }
    }
}
