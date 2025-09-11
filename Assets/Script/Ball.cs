using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector2 position;
    public Vector2 velocity;
    public float radius = 0.5f;
    public Transform VisualReference; // referencia al objeto visual

    void Awake()
    {
        VisualReference = this.transform;
        position = new Vector2(transform.position.x, transform.position.y);
    }
}
