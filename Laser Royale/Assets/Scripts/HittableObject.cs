using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : GridObject
{
    //public virtual Vector2[] Hit(Vector2 dir, RaycastHit2D hitInfo, float maxCastRange)
    //{
    //    return new Vector2[]{ hitInfo.point};
    //}

    public virtual Vector2[] Hit(Vector2 dir, RaycastHit2D hitInfo, float maxCastRange, GameObject laser = null)
    {
        return new Vector2[] { hitInfo.point };
    }
}
