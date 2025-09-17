using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PoolSimulator : MonoBehaviour
{
    public bool aiming;
    public Vector2 aimStartWorld;
    public Vector2 aimCurrentWorld;

    public float shotPower;

    public float maxShotSpeed = 30;

    public TMPro.TMP_Text magnitudeText;

    [SerializeField] Ball whiteBall;

    public List<Ball> balls = new();

    public List<BoxCollision> walls = new();
    [SerializeField] float bounceFactor = 0.8f;
    public float friction = 0.99f;
    public SpriteRenderer poolTable;

    void Start()
    {

    }

    void Update()
    {
        HandleInput();
        float dt = Time.deltaTime;

        StepPhysics(dt);
        SyncTransforms();
        magnitudeText.text = whiteBall.velocity.magnitude.ToString();
    }

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
                ResolveWallColission(A);
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
        for (int j = 0; j < walls.Count; j++)
        {
            if (walls[j].CheckCollision(b.gameObject.transform.position) == true)
            {
                Vector2 d = b.transform.position;
                Vector2 n = walls[j].transform.position;
                n.Normalize();
                float dot = Vector2.Dot(d, n);
                Vector2 result = d -2f * dot * n;
                b.velocity = result * bounceFactor;
                Debug.Log("he colisionado");
            }
        }
    }

    void SyncTransforms()
    {
        foreach (var b in balls)
        {
            b.transform.position = b.position;
        }
    }

    void HandleInput()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            // Solo si clic cerca de la blanca y está casi quieta
            if ((mouseWorld - whiteBall.position).sqrMagnitude <= (whiteBall.radius * 5f) * (whiteBall.radius * 5f)
                && whiteBall.velocity.sqrMagnitude < 0.0001f)
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
                whiteBall.velocity = v;
                aiming = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(aimStartWorld, aimCurrentWorld);
    }
}
