using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSimulator : MonoBehaviour
{

    public List<Ball> balls = new List<Ball>();
    public float friction = 0.99f;
    public float tableMinX = -5, tableMaxX = 5, tableMinY = -3, tableMaxY = 3;

    void Update()
    {
        float dt = Time.deltaTime;

        // Mover bolas
        foreach (var b in balls)
        {
            b.position += b.velocity * dt;
            b.velocity *= friction;

            // Colisi�n con bordes
            //if (b.position.x - b.radius < tableMinX || b.position.x + b.radius > tableMaxX)
            //    b.velocity.x *= -1;
            //if (b.position.y - b.radius < tableMinY || b.position.y + b.radius > tableMaxY)
            //    b.velocity.y *= -1;
        }

        // Colisiones entre bolas
        for (int i = 0; i < balls.Count; i++)
        {
            for (int j = i + 1; j < balls.Count; j++)
            {
                Ball A = balls[i];
                Ball B = balls[j];
                Vector2 diff = B.position - A.position;
                float dist = diff.magnitude;
                float minDist = A.radius + B.radius;

                if (dist < minDist) // hay colisi�n
                {
                    Vector2 normal = diff.normalized;
                    Vector2 tangent = new Vector2(-normal.y, normal.x);

                    float dpTanA = Vector2.Dot(A.velocity, tangent);
                    float dpTanB = Vector2.Dot(B.velocity, tangent);

                    float dpNormA = Vector2.Dot(A.velocity, normal);
                    float dpNormB = Vector2.Dot(B.velocity, normal);

                    float newNormA = dpNormB;
                    float newNormB = dpNormA;

                    A.velocity = tangent * dpTanA + normal * newNormA;
                    B.velocity = tangent * dpTanB + normal * newNormB;

                    // Separar para evitar solapamiento
                    float overlap = 0.5f * (minDist - dist);
                    A.position -= normal * overlap;
                    B.position += normal * overlap;
                }
            }
        }

        // Actualizar posici�n visual
        foreach (var b in balls)
        {
            b.transform.position = b.position;
        }
    }
}
