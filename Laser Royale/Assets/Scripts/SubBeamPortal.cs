using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubBeamPortal : HittableObject
{
    BeamPortal parentObj;

    public override void GridObjectStart()
    {
        base.GridObjectStart();
    
        parentObj = GetComponentInParent<BeamPortal>();
    }

    public override Vector2[] Hit(Vector2 dir, RaycastHit2D hitInfo, float maxCastRange, GameObject laser= null)
    {
        Debug.Log("hit");
        return parentObj.Hit(dir, hitInfo, maxCastRange, gameObject);
    }
}
