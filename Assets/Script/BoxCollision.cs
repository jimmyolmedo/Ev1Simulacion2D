using UnityEngine;

public class BoxCollision : MonoBehaviour
{

    [SerializeField] Transform player;

    [SerializeField] Vector2[] points;

    public bool inArea;

    void Update()
    {

    }


    public bool CheckCollision(Vector2 _obj)
    {


        if (_obj.x <= points[1].x && _obj.x >= points[0].x && _obj.y <= points[0].y && _obj.y >= points[3].y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(points[0], .2f);
        Gizmos.DrawWireSphere(points[1], .2f);
        Gizmos.DrawWireSphere(points[2], .2f);
        Gizmos.DrawWireSphere(points[3], .2f); 
    }
}
