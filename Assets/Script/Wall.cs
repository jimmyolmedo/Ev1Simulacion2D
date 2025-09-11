using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Wall : MonoBehaviour
{
    public Vector2 bounds;

    void Awake()
    {
        bounds = GetComponent<SpriteRenderer>().sprite.bounds.max;
        
    }
}
