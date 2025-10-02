using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ball : MonoBehaviour
{
    public Vector2 position;
    public Vector2 velocity;
    public float radius = 0.5f;
    public Transform VisualReference; // referencia al objeto visual

    void Awake()
    {
        VisualReference = transform;
        position = transform.position;
        if (!transform.CompareTag("WhiteBall"))
        {
            GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        }
    }
}
