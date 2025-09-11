using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSimulator : MonoBehaviour
{

    public List<Ball> balls = new();
    public float friction = 0.99f;
    public SpriteRenderer poolTable;

    void Start()
    {

    }

    void Update()
    {
        //HandleInput();
        float dt = Time.deltaTime;

        StepPhysics(dt);
        SyncTransforms();
    }


    /*
        void HandleInput()
        {
            if (cue == null) return;
            Vector2 mouseWorld = ScreenToWorld(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                // Solo si clic cerca de la blanca y está casi quieta
                if ((mouseWorld - cue.pos).sqrMagnitude <= (ballRadius * 20f) * (ballRadius * 20f)
                    && cue.vel.sqrMagnitude < 0.0001f)
                {
                    aiming = true;
                    aimStartWorld = mouseWorld;
                    aimCurrentWorld = mouseWorld;
                }
            }
            if (aiming)
            {
                aimCurrentWorld = mouseWorld;
                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 dir = (aimCurrentWorld - aimStartWorld);
                    Vector2 v = dir * shotPower;
                    if (v.magnitude > maxShotSpeed) v = v.normalized * maxShotSpeed;
                    cue.vel = v;
                    aiming = false;
                }
            }
        }
        */

    void StepPhysics(float dt)
    {
        // Mover bolas
        foreach (var b in balls)
        {
            b.position += b.velocity * dt;
            b.velocity *= friction;

        }
        for (int i = 0; i < balls.Count; i++)
        {
            for (int j = i + 1; j < balls.Count; j++)
            {
                Ball A = balls[i];
                Ball B = balls[j];
                ResolveBallCollision(A, B);
                ResolveWallColission(B);
            }
        }
    }

    void ResolveBallCollision(Ball a, Ball b)
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Vector2 diff = b.position - a.position;
            float dist = diff.magnitude;
            float minDist = a.radius + b.radius;

            if (dist < minDist) // hay colisi�n
            {
                Vector2 normal = diff.normalized;
                Vector2 tangent = new(-normal.y, normal.x);

                float dpTanA = Vector2.Dot(a.velocity, tangent);
                float dpTanB = Vector2.Dot(b.velocity, tangent);

                float dpNormA = Vector2.Dot(a.velocity, normal);
                float dpNormB = Vector2.Dot(b.velocity, normal);

                float newNormA = dpNormB;
                float newNormB = dpNormA;

                a.velocity = tangent * dpTanA + normal * newNormA;
                b.velocity = tangent * dpTanB + normal * newNormB;

                // Separar para evitar solapamiento
                float overlap = 0.5f * (minDist - dist);
                a.position -= normal * overlap;
                b.position += normal * overlap;
            }
        }
    }

    void ResolveWallColission(Ball b)
    {
        for (int i = 0; i < balls.Count; i++)
        {
            
        }
    }

    void SyncTransforms()
    {
        foreach (var b in balls)
        {
            b.transform.position = b.position;
        }
    }
}
