using UnityEngine;

public class EndPoint : HittableObject
{
    public override Vector2[] Hit(Vector2 dir, RaycastHit2D hitInfo, float maxCastRange)
    {
        Debug.Log("Win");
        return base.Hit(dir, hitInfo, maxCastRange);
    }
}
